using NcmFox.Core;

namespace NcmFox.Utils;

public static class FileChecker
{
    public static bool IsValidNcmFile(string filePath)
    {
        if (!File.Exists(filePath))
            throw new FileNotFoundException();
        
        // 创建缓冲区，栈内存分配（不产生 GC 压力）
        Span<byte> buffer = stackalloc byte[8];

        using var fs = File.OpenRead(filePath);
        // 读取前 8 字节
        var bytesRead = fs.Read(buffer);
        
        // 比较缓冲区与魔数
        return bytesRead == 8 && buffer.SequenceEqual(NcmConstants.MagicHeader);
    }

    public static bool IsValidNcmFile(Stream stream)
    {
        Span<byte> buffer = stackalloc byte[8];
        var bytesRead = stream.Read(buffer);
        return bytesRead == 8 && buffer.SequenceEqual(NcmConstants.MagicHeader);
    }
}