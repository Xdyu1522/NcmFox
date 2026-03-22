using System.Buffers;
using System.Numerics;
using System.Runtime.CompilerServices;
using NcmFox.Config;
using NcmFox.Models;

namespace NcmFox.Core;

internal static class AudioDecipher
{
    private static byte[] BuildLookupTable(byte[] keyBox)
    {
        var lut = new byte[256];

        for (int j = 0; j < 256; j++)
        {
            var keyIndex = (keyBox[j] + keyBox[(keyBox[j] + j) & 0xff]) & 0xff;
            lut[j] = keyBox[keyIndex];
        }

        return lut;
    }

    public static void Decipher(Stream input, Stream output, byte[] keyBox)
    {
        var lut = BuildLookupTable(keyBox);
        var bufferSize = NcmConfig.Current.BufferSize;
        var buffer = ArrayPool<byte>.Shared.Rent(bufferSize);

        try
        {
            int offset = 0;
            int read;

            while ((read = input.Read(buffer, 0, bufferSize)) > 0)
            {
                Process(buffer.AsSpan(0, read), lut, offset);
                output.Write(buffer, 0, read);
                offset += read;
            }
        }
        finally
        {
            ArrayPool<byte>.Shared.Return(buffer);
        }
    }

    public static void Decipher(NcmFile file, Stream outputStream)
    {
        if (!file.IsInitialized)
            throw new InvalidDataException();

        using var fs = file.FileInfo.OpenRead();
        fs.Position = file.AudioOffset;
        Decipher(fs, outputStream, file.KeyBox);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static void Process(Span<byte> buffer, byte[] lut, int offset)
    {
        if (Vector.IsHardwareAccelerated)
        {
            ProcessSimd(buffer, lut, offset);
        }
        else
        {
            ProcessScalar(buffer, lut, offset);
        }
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static void ProcessSimd(Span<byte> buffer, byte[] lut, int offset)
    {
        int i = 0;
        int vectorSize = Vector<byte>.Count;
        int j = (offset + 1) & 0xff;

        Span<byte> keyBytes = stackalloc byte[vectorSize];

        for (; i <= buffer.Length - vectorSize; i += vectorSize)
        {
            for (int k = 0; k < vectorSize; k++)
            {
                keyBytes[k] = lut[(j + k) & 0xff];
            }

            var dataVec = new Vector<byte>(buffer.Slice(i, vectorSize));
            var keyVec = new Vector<byte>(keyBytes);
            (dataVec ^ keyVec).CopyTo(buffer.Slice(i, vectorSize));

            j = (j + vectorSize) & 0xff;
        }

        ProcessScalar(buffer.Slice(i), lut, offset + i);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static void ProcessScalar(Span<byte> buffer, byte[] lut, int offset)
    {
        int j = (offset + 1) & 0xff;
        int i = 0;

        for (; i <= buffer.Length - 8; i += 8)
        {
            buffer[i + 0] ^= lut[j];
            buffer[i + 1] ^= lut[(j + 1) & 0xff];
            buffer[i + 2] ^= lut[(j + 2) & 0xff];
            buffer[i + 3] ^= lut[(j + 3) & 0xff];
            buffer[i + 4] ^= lut[(j + 4) & 0xff];
            buffer[i + 5] ^= lut[(j + 5) & 0xff];
            buffer[i + 6] ^= lut[(j + 6) & 0xff];
            buffer[i + 7] ^= lut[(j + 7) & 0xff];
            j = (j + 8) & 0xff;
        }

        for (; i < buffer.Length; i++)
        {
            buffer[i] ^= lut[j];
            j = (j + 1) & 0xff;
        }
    }
}