using System;

namespace NcmFox.Core;

internal static class KeyBoxBuilder
{
    public static byte[] Build(ReadOnlySpan<byte> key)
    {
        var box = new byte[256];

        for (var i = 0; i < 256; i++)
            box[i] = (byte)i;

        var j = 0;

        for (var i = 0; i < 256; i++)
        {
            j = (j + box[i] + key[i % key.Length]) & 0xff;

            (box[i], box[j]) = (box[j], box[i]);
        }

        return box;
    }
}