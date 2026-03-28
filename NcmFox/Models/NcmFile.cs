namespace NcmFox.Models;

/// <summary>
/// Represents a parsed NCM file with its metadata and decryption information.
/// </summary>
/// <remarks>
/// <para>
/// This class holds all information extracted from an NCM file, including metadata,
/// cover image, and the key box needed for audio decryption.
/// </para>
/// <para>
/// Instances of this class are created by <see cref="NcmDecoder.Open(string)"/> and
/// should not be constructed directly.
/// </para>
/// </remarks>
public sealed class NcmFile
{
    /// <summary>
    /// Gets the file information for the NCM file.
    /// </summary>
    /// <value>
    /// A <see cref="FileInfo"/> containing the path and file system information.
    /// </value>
    public required FileInfo FileInfo;

    /// <summary>
    /// Gets the song metadata extracted from the NCM file.
    /// </summary>
    /// <value>
    /// A <see cref="MetaData"/> instance containing song information, or <c>null</c> if not present.
    /// </value>
    /// <remarks>
    /// Metadata includes song name, artists, album, and other information.
    /// Some NCM files may not contain metadata.
    /// </remarks>
    public required MetaData? MetaData;

    /// <summary>
    /// Gets the key box used for audio decryption.
    /// </summary>
    /// <value>
    /// A 256-byte array containing the decryption key box.
    /// </value>
    /// <remarks>
    /// The key box is derived from the encrypted key data in the NCM file header
    /// and is used to decrypt the audio content.
    /// </remarks>
    public required byte[] KeyBox { get; init; }

    /// <summary>
    /// Gets the pre-computed key stream for decryption.
    /// </summary>
    /// <value>
    /// A 256-byte array containing the pre-computed key stream.
    /// </value>
    internal byte[] KeyStream { get; init; } = null!;

    /// <summary>
    /// Gets the offset in the file where the encrypted audio data begins.
    /// </summary>
    /// <value>
    /// The byte offset from the start of the file.
    /// </value>
    public long AudioOffset { get; init; }

    /// <summary>
    /// Gets the cover image data extracted from the NCM file.
    /// </summary>
    /// <value>
    /// An <see cref="NcmCover"/> instance containing the cover image, or <c>null</c> if not present.
    /// </value>
    public required NcmCover? CoverData { get; init; }

    /// <summary>
    /// Gets the audio format of the decrypted content.
    /// </summary>
    /// <value>
    /// A <see cref="SaveFormat"/> value indicating the format (FLAC, MP3, or Unknown).
    /// </value>
    /// <remarks>
    /// This value is derived from <see cref="MetaData"/>. If metadata is not available,
    /// returns <see cref="SaveFormat.Unknown"/>.
    /// </remarks>
    public SaveFormat SaveFormat => MetaData?.SaveFormat ?? SaveFormat.Unknown;

    /// <summary>
    /// Gets a value indicating whether the file has been properly initialized.
    /// </summary>
    /// <value>
    /// <c>true</c> if the file was successfully parsed; otherwise, <c>false</c>.
    /// </value>
    public bool IsInitialized { get; init; } = false;
}
