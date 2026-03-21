using NcmFox.Core;
using NcmFox.Models;

namespace NcmFox;

public static class NcmFileExtensions
{
    public static void Decode(this NcmFile file, string outputPath)
    {
        using var outputStream = File.Create(outputPath);
        AudioDecipher.Decipher(file, outputStream);
    }
    
    public static void Decode(this NcmFile file, Stream outputStream)
    {
        if (!file.IsInitialized)
            throw new InvalidDataException();
        
        using var fs = file.FileInfo.OpenRead();
        fs.Position = file.AudioOffset;
        AudioDecipher.Decipher(fs, outputStream, file.KeyBox);
    }

    public static byte[] Decode(this NcmFile file)
    {
        using var output = new MemoryStream();
        AudioDecipher.Decipher(file, output);
        return output.ToArray();
    }
}