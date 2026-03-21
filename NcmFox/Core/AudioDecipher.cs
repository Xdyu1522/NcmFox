using System.Numerics;
using NcmFox.Models;

namespace NcmFox.Core;

internal static class AudioDecipher
{
    private const int BufferSize = 1024 * 1024; // 1MB

    // =========================
    // LUT 构建
    // =========================
    private static byte[] BuildLookupTable(byte[] keyBox)
    {
        var lut = new byte[256];

        for (int j = 0; j < 256; j++)
        {
            var keyIndex =
                (keyBox[j] + keyBox[(keyBox[j] + j) & 0xff]) & 0xff;

            lut[j] = keyBox[keyIndex];
        }

        return lut;
    }

    // =========================
    // LUT 扩展（关键优化）
    // =========================
    private static byte[] ExpandLookupTable(byte[] lut)
    {
        // 扩大到更安全的范围（至少 buffer size）
        var expanded = new byte[BufferSize]; // 1MB级别

        for (int i = 0; i < expanded.Length; i++)
        {
            expanded[i] = lut[i & 0xff];
        }

        return expanded;
    }

    // =========================
    // 主解密入口（Stream）
    // =========================
    public static void Decipher(
        Stream input,
        Stream output,
        byte[] keyBox)
    {
        var lut = BuildLookupTable(keyBox);
        var expandedLut = ExpandLookupTable(lut);

        var buffer = new byte[BufferSize];
        int offset = 0;

        int read;

        while ((read = input.Read(buffer, 0, buffer.Length)) > 0)
        {
            ProcessSimd(buffer.AsSpan(0, read), expandedLut, offset);

            output.Write(buffer, 0, read);

            offset += read;
        }
    }

    // =========================
    // NcmFile 重载
    // =========================
    public static void Decipher(NcmFile file, Stream outputStream)
    {
        if (!file.IsInitialized)
            throw new InvalidDataException();

        using var fs = file.FileInfo.OpenRead();
        fs.Position = file.AudioOffset;

        Decipher(fs, outputStream, file.KeyBox);
    }

    // =========================
    // SIMD 核心处理
    // =========================
    private static void ProcessSimd(
        Span<byte> buffer,
        byte[] expandedLut,
        int offset)
    {
        int i = 0;
        int vectorSize = Vector<byte>.Count;

        int baseOffset = offset & 0xff;

        for (; i <= buffer.Length - vectorSize; i += vectorSize)
        {
            int lutIndex = baseOffset + i;

            var dataVec = new Vector<byte>(buffer.Slice(i, vectorSize));
            var keyVec = new Vector<byte>(expandedLut, lutIndex);

            (dataVec ^ keyVec).CopyTo(buffer.Slice(i, vectorSize));
        }

        for (; i < buffer.Length; i++)
        {
            buffer[i] ^= expandedLut[baseOffset + i];
        }
    }
}