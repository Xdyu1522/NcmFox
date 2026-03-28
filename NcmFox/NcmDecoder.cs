using NcmFox.Core;
using NcmFox.Models;

namespace NcmFox;

/// <summary>
/// Provides methods for opening and parsing NCM encrypted audio files.
/// </summary>
/// <remarks>
/// <para>
/// NCM is a proprietary encrypted audio format used by NetEase Cloud Music.
/// This class provides the entry point for decrypting NCM files.
/// </para>
/// <para>
/// Example usage:
/// <code>
/// var ncmFile = NcmDecoder.Open("song.ncm");
/// Console.WriteLine($"Song: {ncmFile.MetaData?.SongName}");
/// ncmFile.Decode("output.flac");
/// </code>
/// </para>
/// </remarks>
public static class NcmDecoder
{
    /// <summary>
    /// Opens and parses an NCM file from the specified file path.
    /// </summary>
    /// <param name="filePath">The path to the NCM file to open.</param>
    /// <returns>An <see cref="NcmFile"/> instance containing parsed metadata and decryption information.</returns>
    /// <exception cref="FileNotFoundException">Thrown when the specified file does not exist.</exception>
    /// <exception cref="InvalidDataException">
    /// Thrown when:
    /// <list type="bullet">
    /// <item><description>The file is too small to be a valid NCM file.</description></item>
    /// <item><description>The file does not have a valid NCM magic header.</description></item>
    /// <item><description>The file contains corrupted or invalid data.</description></item>
    /// </list>
    /// </exception>
    /// <remarks>
    /// This method performs the following operations:
    /// <list type="number">
    /// <item><description>Validates file existence and size.</description></item>
    /// <item><description>Verifies the NCM magic header.</description></item>
    /// <item><description>Parses the encrypted key data.</description></item>
    /// <item><description>Extracts metadata (if present).</description></item>
    /// <item><description>Extracts cover image (if present).</description></item>
    /// </list>
    /// </remarks>
    public static NcmFile Open(string filePath)
    {
        if (!File.Exists(filePath))
            throw new FileNotFoundException($"NCM file not found: {filePath}", filePath);

        using var fs = File.OpenRead(filePath);

        if (fs.Length < 20)
            throw new InvalidDataException("File too small to be a valid NCM file");

        Span<byte> magicBuffer = stackalloc byte[8];
        if (fs.Read(magicBuffer) != 8 || !magicBuffer.SequenceEqual(NcmConstants.MagicHeader))
            throw new InvalidDataException($"Invalid NCM file format: {filePath}");

        using var reader = new BinaryReader(fs);
        reader.ReadBytes(2);

        var keyBox = KeyDataParser.Parse(reader);
        var keyStream = AudioDecipher.BuildKeyStream(keyBox);
        var metaData = MetaDataParser.Parse(reader);
        var coverData = CoverParser.Parse(reader);
        var audioOffset = reader.BaseStream.Position;

        return new NcmFile
        {
            FileInfo = new FileInfo(filePath),
            KeyBox = keyBox,
            KeyStream = keyStream,
            AudioOffset = audioOffset,
            CoverData = coverData,
            MetaData = metaData,
            IsInitialized = true
        };
    }

    /// <summary>
    /// Opens and parses an NCM file from the specified <see cref="FileInfo"/>.
    /// </summary>
    /// <param name="fileInfo">The <see cref="FileInfo"/> representing the NCM file to open.</param>
    /// <returns>An <see cref="NcmFile"/> instance containing parsed metadata and decryption information.</returns>
    /// <exception cref="FileNotFoundException">Thrown when the specified file does not exist.</exception>
    /// <exception cref="InvalidDataException">Thrown when the file is not a valid NCM file.</exception>
    public static NcmFile Open(FileInfo fileInfo)
    {
        return Open(fileInfo.FullName);
    }
}