using System.Buffers;
using System.Numerics;
using System.Runtime.CompilerServices;
using NcmFox.Config;
using NcmFox.Models;

namespace NcmFox.Core;

internal static class AudioDecipher
{
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static byte[] BuildKeyStream(byte[] keyBox)
    {
        var lut = new byte[256];
        var keyStream = new byte[256];

        for (int j = 0; j < 256; j++)
        {
            var keyIndex = (keyBox[j] + keyBox[(keyBox[j] + j) & 0xff]) & 0xff;
            lut[j] = keyBox[keyIndex];
        }

        for (int i = 0; i < 256; i++)
        {
            keyStream[i] = lut[(i + 1) & 0xff];
        }

        return keyStream;
    }

    public static void Decipher(Stream input, Stream output, byte[] keyStream)
    {
        var bufferSize = NcmConfig.Current.BufferSize;
        var buffer = ArrayPool<byte>.Shared.Rent(bufferSize);

        try
        {
            int offset = 0;
            int read;

            while ((read = input.Read(buffer, 0, bufferSize)) > 0)
            {
                Process(buffer.AsSpan(0, read), keyStream, offset);
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
        Decipher(fs, outputStream, file.KeyStream);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static void Process(Span<byte> buffer, byte[] keyStream, int offset)
    {
        if (Vector.IsHardwareAccelerated)
        {
            ProcessSimd(buffer, keyStream, offset);
        }
        else
        {
            ProcessScalar(buffer, keyStream, offset);
        }
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static void ProcessSimd(Span<byte> buffer, byte[] keyStream, int offset)
    {
        int i = 0;
        int vectorSize = Vector<byte>.Count;
        int j = offset & 0xff;

        Span<byte> temp = stackalloc byte[Vector<byte>.Count];

        for (; i <= buffer.Length - vectorSize; i += vectorSize)
        {
            if (j + vectorSize <= 256)
            {
                var keyVec = new Vector<byte>(keyStream, j);
                var dataVec = new Vector<byte>(buffer.Slice(i, vectorSize));
                (dataVec ^ keyVec).CopyTo(buffer.Slice(i, vectorSize));
            }
            else
            {
                int firstPart = 256 - j;

                keyStream.AsSpan(j, firstPart).CopyTo(temp);
                keyStream.AsSpan(0, vectorSize - firstPart).CopyTo(temp[firstPart..]);

                var keyVec = new Vector<byte>(temp);
                var dataVec = new Vector<byte>(buffer.Slice(i, vectorSize));
                (dataVec ^ keyVec).CopyTo(buffer.Slice(i, vectorSize));
            }

            j = (j + vectorSize) & 0xff;
        }

        ProcessScalar(buffer.Slice(i), keyStream, offset + i);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static void ProcessScalar(Span<byte> buffer, byte[] keyStream, int offset)
    {
        int j = offset & 0xff;
        int i = 0;

        for (; i <= buffer.Length - 8; i += 8)
        {
            buffer[i + 0] ^= keyStream[j];
            buffer[i + 1] ^= keyStream[(j + 1) & 0xff];
            buffer[i + 2] ^= keyStream[(j + 2) & 0xff];
            buffer[i + 3] ^= keyStream[(j + 3) & 0xff];
            buffer[i + 4] ^= keyStream[(j + 4) & 0xff];
            buffer[i + 5] ^= keyStream[(j + 5) & 0xff];
            buffer[i + 6] ^= keyStream[(j + 6) & 0xff];
            buffer[i + 7] ^= keyStream[(j + 7) & 0xff];

            j = (j + 8) & 0xff;
        }

        for (; i < buffer.Length; i++)
        {
            buffer[i] ^= keyStream[j];
            j = (j + 1) & 0xff;
        }
    }
}
