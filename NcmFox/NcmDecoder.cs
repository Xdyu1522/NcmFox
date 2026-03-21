using NcmFox.Core;
using NcmFox.Models;
using NcmFox.Utils;

namespace NcmFox;

public static class NcmDecoder
{
    public static NcmFile Open(string filePath)
    {
        if (!File.Exists(filePath))
            throw new FileNotFoundException($"NCM file not found: {filePath}", filePath);

        if (!FileChecker.IsValidNcmFile(filePath))
            throw new InvalidDataException($"Invalid NCM file format: {filePath}");
        
        using var fs = File.OpenRead(filePath);
        
        if (fs.Length < 20)  // 最小有效 NCM 文件大小
            throw new InvalidDataException("File too small to be a valid NCM file");
        
        using var reader = new BinaryReader(fs);
        reader.ReadBytes(10); // 跳过 MagicHeader(8) 和 Gap(2)
        
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