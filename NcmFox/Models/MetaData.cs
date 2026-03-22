namespace NcmFox.Models;

/// <summary>
/// Represents song metadata extracted from an NCM file.
/// </summary>
/// <remarks>
/// <para>
/// This class contains information about the song, including its name, artists,
/// album, and format. It is typically populated from the JSON metadata embedded
/// in the NCM file.
/// </para>
/// </remarks>
public sealed class MetaData
{
    /// <summary>
    /// Gets the unique identifier for the song.
    /// </summary>
    /// <value>
    /// A string containing the song ID from NetEase Cloud Music.
    /// </value>
    public required string Id { get; init; }

    /// <summary>
    /// Gets the name of the song.
    /// </summary>
    /// <value>
    /// A string containing the song title.
    /// </value>
    public required string SongName { get; init; }

    /// <summary>
    /// Gets the list of artists who performed the song.
    /// </summary>
    /// <value>
    /// An array of artist names, or <c>null</c> if not available.
    /// </value>
    /// <remarks>
    /// For songs with multiple artists, this array contains all performer names.
    /// Individual elements may be <c>null</c> if the artist name could not be parsed.
    /// </remarks>
    public required string?[]? Artists { get; init; }

    /// <summary>
    /// Gets the name of the album containing the song.
    /// </summary>
    /// <value>
    /// A string containing the album name.
    /// </value>
    public required string AlbumName { get; init; }

    /// <summary>
    /// Gets the URL for the album cover image.
    /// </summary>
    /// <value>
    /// A string containing the URL to the album cover image on NetEase servers.
    /// </value>
    /// <remarks>
    /// This URL points to an external resource and may require network access to retrieve.
    /// The embedded cover image in <see cref="NcmFile.CoverData"/> is typically preferred.
    /// </remarks>
    public required string AlbumCoverUrl { get; init; }

    /// <summary>
    /// Gets the audio format of the song.
    /// </summary>
    /// <value>
    /// A <see cref="SaveFormat"/> value indicating whether the audio is FLAC or MP3.
    /// </value>
    public required SaveFormat SaveFormat { get; init; }
}