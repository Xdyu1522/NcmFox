namespace NcmFox.Models;

/// <summary>
/// Specifies the audio format of the decrypted content.
/// </summary>
public enum SaveFormat
{
    /// <summary>
    /// The audio is in FLAC (Free Lossless Audio Codec) format.
    /// </summary>
    Flac = 0,

    /// <summary>
    /// The audio is in MP3 (MPEG Audio Layer III) format.
    /// </summary>
    Mp3 = 1,

    /// <summary>
    /// The audio format is unknown or could not be determined.
    /// </summary>
    Unknown = 2
}