using NcmFox.Models;

namespace NcmFox.Core;

internal static class CoverParser
{
    public static NcmCover? Parse(BinaryReader reader)
    {
        reader.ReadUInt32(); // CRC

        reader.ReadBytes(5); // unknown

        var imageSize = reader.ReadInt32();

        if (imageSize <= 0)
            return null;

        var imageData = reader.ReadBytes(imageSize);

        return new NcmCover
        {
            Data = imageData,
            Format = DetectFormat(imageData)
        };
    }
    
    private static CoverFormat DetectFormat(byte[] data)
    {
        if (data.Length < 4)
            return CoverFormat.Unknown;

        if (data[0] == 0xFF && data[1] == 0xD8)
            return CoverFormat.Jpeg;

        if (data[0] == 0x89 &&
            data[1] == 0x50 &&
            data[2] == 0x4E &&
            data[3] == 0x47)
            return CoverFormat.Png;

        return CoverFormat.Unknown;
    }
}