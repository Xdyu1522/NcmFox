namespace NcmFox.Models;

/// <summary>
/// Specifies the format of a cover image.
/// </summary>
public enum CoverFormat
{
    /// <summary>
    /// The image is in JPEG format.
    /// </summary>
    Jpeg = 0,

    /// <summary>
    /// The image is in PNG format.
    /// </summary>
    Png = 1,

    /// <summary>
    /// The image format is unknown or could not be determined.
    /// </summary>
    Unknown = 2
}