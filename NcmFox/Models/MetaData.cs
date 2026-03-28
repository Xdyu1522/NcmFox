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
    /// A read-only list of <see cref="Artist"/> objects containing artist names and IDs.
    /// </value>
    /// <remarks>
    /// For songs with multiple artists, this list contains all performers.
    /// </remarks>
    public required IReadOnlyList<Artist> Artists { get; init; }

    /// <summary>
    /// Gets the alias names of the song.
    /// </summary>
    /// <value>
    /// A read-only list of alias names, or an empty list if not available.
    /// </value>
    public IReadOnlyList<string> Alias { get; init; } = [];

    /// <summary>
    /// Gets the translated names of the song.
    /// </summary>
    /// <value>
    /// A read-only list of translated song names, or an empty list if not available.
    /// </value>
    public IReadOnlyList<string> TranslatedNames { get; init; } = [];

    /// <summary>
    /// Gets the duration of the song.
    /// </summary>
    /// <value>
    /// A <see cref="TimeSpan"/> representing the song duration, or <c>null</c> if not available.
    /// </value>
    public TimeSpan? Duration { get; init; }

    /// <summary>
    /// Gets the name of the album containing the song.
    /// </summary>
    /// <value>
    /// A string containing the album name.
    /// </value>
    public required string AlbumName { get; init; }

    /// <summary>
    /// Gets the unique identifier for the album.
    /// </summary>
    /// <value>
    /// A string containing the album ID from NetEase Cloud Music.
    /// </value>
    public required string AlbumId { get; init; }

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
    /// Gets the document ID for the album cover image.
    /// </summary>
    /// <value>
    /// A string containing the document ID, or <c>null</c> if not available.
    /// </value>
    public string? AlbumCoverDocId { get; init; }

    /// <summary>
    /// Gets the audio format of the song.
    /// </summary>
    /// <value>
    /// A <see cref="SaveFormat"/> value indicating whether the audio is FLAC or MP3.
    /// </value>
    public required SaveFormat SaveFormat { get; init; }

    /// <summary>
    /// Gets the bitrate of the audio file.
    /// </summary>
    /// <value>
    /// An integer representing the bitrate in bits per second, or <c>null</c> if not available.
    /// </value>
    public int? Bitrate { get; init; }

    /// <summary>
    /// Gets the document ID for the MP3 file.
    /// </summary>
    /// <value>
    /// A string containing the MP3 document ID, or <c>null</c> if not available.
    /// </value>
    public string? Mp3DocId { get; init; }

    /// <summary>
    /// Gets the volume adjustment value.
    /// </summary>
    /// <value>
    /// A double representing the volume delta in decibels, or <c>null</c> if not available.
    /// </value>
    public double? VolumeDelta { get; init; }

    /// <summary>
    /// Gets the unique identifier for the music video.
    /// </summary>
    /// <value>
    /// A string containing the MV ID, or <c>null</c> if not available.
    /// </value>
    public string? MvId { get; init; }

    /// <summary>
    /// Gets the fee type of the song.
    /// </summary>
    /// <value>
    /// An integer representing the fee type, or <c>null</c> if not available.
    /// </value>
    /// <remarks>
    /// Common values: 0 (free), 1 (VIP), 4 (purchased), 8 (free low quality).
    /// </remarks>
    public int? Fee { get; init; }

    /// <summary>
    /// Gets the privilege information for the song.
    /// </summary>
    /// <value>
    /// A <see cref="Privilege"/> object containing access rights, or <c>null</c> if not available.
    /// </value>
    public Privilege? Privilege { get; init; }
}

/// <summary>
/// Represents an artist with name and identifier.
/// </summary>
public sealed class Artist
{
    /// <summary>
    /// Gets the name of the artist.
    /// </summary>
    /// <value>
    /// A string containing the artist name.
    /// </value>
    public required string Name { get; init; }

    /// <summary>
    /// Gets the unique identifier for the artist.
    /// </summary>
    /// <value>
    /// A string containing the artist ID from NetEase Cloud Music.
    /// </value>
    public required string Id { get; init; }
}

/// <summary>
/// Represents privilege information for a song.
/// </summary>
public sealed class Privilege
{
    /// <summary>
    /// Gets the privilege flag value.
    /// </summary>
    /// <value>
    /// An integer representing the privilege flags.
    /// </value>
    public int Flag { get; init; }
}
