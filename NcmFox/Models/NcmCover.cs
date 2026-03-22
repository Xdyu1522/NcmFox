namespace NcmFox.Models;

/// <summary>
/// Represents the cover image embedded in an NCM file.
/// </summary>
/// <remarks>
/// <para>
/// This class contains the binary data and format information for the album cover
/// image extracted from an NCM file.
/// </para>
/// <para>
/// The cover image is typically in JPEG or PNG format.
/// </para>
/// </remarks>
public class NcmCover
{
    /// <summary>
    /// Gets the binary data of the cover image.
    /// </summary>
    /// <value>
    /// A byte array containing the raw image data.
    /// </value>
    public required byte[] Data { get; init; }

    /// <summary>
    /// Gets the format of the cover image.
    /// </summary>
    /// <value>
    /// A <see cref="CoverFormat"/> value indicating the image format (JPEG, PNG, or Unknown).
    /// </value>
    public required CoverFormat Format { get; init; }

    /// <summary>
    /// Gets the size of the cover image in bytes.
    /// </summary>
    /// <value>
    /// The length of the <see cref="Data"/> array.
    /// </value>
    public int Size => Data.Length;
}