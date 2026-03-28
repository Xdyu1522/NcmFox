using NcmFox.Core;
using NcmFox.Models;

namespace NcmFox;

/// <summary>
/// Provides extension methods for <see cref="NcmFile"/> to handle audio decryption.
/// </summary>
public static class NcmFileExtensions
{
    /// <summary>
    /// Decrypts the NCM file and writes the output to the specified file path.
    /// </summary>
    /// <param name="file">The <see cref="NcmFile"/> to decrypt.</param>
    /// <param name="outputPath">The file path where the decrypted audio will be saved.</param>
    /// <exception cref="InvalidDataException">Thrown when the file is not properly initialized.</exception>
    /// <exception cref="IOException">Thrown when an I/O error occurs during file operations.</exception>
    /// <remarks>
    /// <para>
    /// The output format (FLAC or MP3) is determined by the <see cref="NcmFile.SaveFormat"/> property.
    /// </para>
    /// <para>
    /// Example:
    /// <code>
    /// var ncm = NcmDecoder.Open("song.ncm");
    /// ncm.Decode("output.flac");
    /// </code>
    /// </para>
    /// </remarks>
    public static void Decode(this NcmFile file, string outputPath)
    {
        using var outputStream = File.Create(outputPath);
        AudioDecipher.Decipher(file, outputStream);
    }

    /// <summary>
    /// Decrypts the NCM file and writes the output to the specified stream.
    /// </summary>
    /// <param name="file">The <see cref="NcmFile"/> to decrypt.</param>
    /// <param name="outputStream">The stream where the decrypted audio will be written.</param>
    /// <exception cref="InvalidDataException">Thrown when the file is not properly initialized.</exception>
    /// <remarks>
    /// <para>
    /// This method does not close or dispose the output stream. The caller is responsible for managing the stream lifecycle.
    /// </para>
    /// <para>
    /// Example:
    /// <code>
    /// var ncm = NcmDecoder.Open("song.ncm");
    /// using var ms = new MemoryStream();
    /// ncm.Decode(ms);
    /// </code>
    /// </para>
    /// </remarks>
    public static void Decode(this NcmFile file, Stream outputStream)
    {
        AudioDecipher.Decipher(file, outputStream);
    }

    /// <summary>
    /// Decrypts the NCM file and returns the decrypted audio data as a byte array.
    /// </summary>
    /// <param name="file">The <see cref="NcmFile"/> to decrypt.</param>
    /// <returns>A byte array containing the decrypted audio data.</returns>
    /// <exception cref="InvalidDataException">Thrown when the file is not properly initialized.</exception>
    /// <remarks>
    /// <para>
    /// This method loads the entire decrypted audio into memory. For large files,
    /// consider using <see cref="Decode(NcmFile, string)"/> or <see cref="Decode(NcmFile, Stream)"/>
    /// to avoid high memory consumption.
    /// </para>
    /// <para>
    /// Example:
    /// <code>
    /// var ncm = NcmDecoder.Open("song.ncm");
    /// byte[] audioData = ncm.Decode();
    /// </code>
    /// </para>
    /// </remarks>
    public static byte[] Decode(this NcmFile file)
    {
        using var output = new MemoryStream();
        AudioDecipher.Decipher(file, output);
        return output.ToArray();
    }
}
