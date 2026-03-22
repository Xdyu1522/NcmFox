using NcmFox.Core;
using NcmFox.Models;

namespace NcmFox;

public static class NcmDecoder
{
    public static NcmFile Open(string filePath)
    {
        if (!File.Exists(filePath))
            throw new FileNotFoundException($"NCM file not found: {filePath}", filePath);

        using var fs = File.OpenRead(filePath);

        if (fs.Length < 20)
            throw new InvalidDataException("File too small to be a valid NCM file");

        // 验证魔数
        Span<byte> magicBuffer = stackalloc byte[8];
        if (fs.Read(magicBuffer) != 8 || !magicBuffer.SequenceEqual(NcmConstants.MagicHeader))
            throw new InvalidDataException($"Invalid NCM file format: {filePath}");

        using var reader = new BinaryReader(fs);
        reader.ReadBytes(2); // 跳过 Gap(2)

        var keyBox = KeyDataParser.Parse(reader);
        var metaData = MetaDataParser.Parse(reader);
        var coverData = CoverParser.Parse(reader);
        var audioOffset = reader.BaseStream.Position;

        return new NcmFile
        {
            FileInfo = new FileInfo(filePath),
            KeyBox = keyBox,
            AudioOffset = audioOffset,
            CoverData = coverData,
            MetaData = metaData,
            IsInitialized = true
        };
    }

    public static NcmFile Open(FileInfo fileInfo)
    {
        return Open(fileInfo.FullName);
    }
}