using System.Diagnostics.CodeAnalysis;

namespace NcmFox.Models;

/// <summary>
/// Data transfer object for NCM file metadata JSON deserialization.
/// </summary>
/// <remarks>
/// This class uses lowercase property names to match the JSON format from NCM files.
/// </remarks>
[SuppressMessage("ReSharper", "InconsistentNaming")]
internal class NcmMetaDataDto
{
    public required string musicId { get; set; }
    public required string musicName { get; set; }
    public required string[][] artist { get; set; }
    public string[]? alias { get; set; }
    public string[]? transNames { get; set; }
    public int? duration { get; set; }
    public required string albumId { get; set; }
    public required string album { get; set; }
    public required string albumPic { get; set; }
    public string? albumPicDocId { get; set; }
    public required string format { get; set; }
    public int? bitrate { get; set; }
    public string? mp3DocId { get; set; }
    public double? volumeDelta { get; set; }
    public string? mvId { get; set; }
    public int? fee { get; set; }
    public Privilege? privilege { get; set; }
}

/// <summary>
/// Provides extension methods for converting DTOs to domain models.
/// </summary>
internal static class MedaDataDtoMapper
{
    /// <summary>
    /// Converts an <see cref="NcmMetaDataDto"/> to a <see cref="MetaData"/> instance.
    /// </summary>
    /// <param name="dto">The DTO to convert.</param>
    /// <returns>A new <see cref="MetaData"/> instance populated from the DTO.</returns>
    /// <exception cref="InvalidDataException">
    /// Thrown when the format field contains an invalid value.
    /// </exception>
    public static MetaData ToMetaData(this NcmMetaDataDto dto)
    {
        var saveFormat = dto.format switch
        {
            "flac" => SaveFormat.Flac,
            "mp3" => SaveFormat.Mp3,
            _ => throw new InvalidDataException($"Format should be 'flac' or 'mp3' instead of {dto.format}")
        };

        return new MetaData
        {
            Id = dto.musicId,
            SongName = dto.musicName,
            Artists = dto.artist?
                          .Where(a => a.Length > 0)
                          .Select(a => new Artist
                          {
                              Name = a.ElementAtOrDefault(0) ?? "Unknown",
                              Id = a.ElementAtOrDefault(1) ?? string.Empty
                          })
                          .ToList() ?? [],
            Alias = dto.alias?.ToList() ?? [],
            TranslatedNames = dto.transNames?.ToList() ?? [],
            Duration = dto.duration.HasValue ? TimeSpan.FromMilliseconds(dto.duration.Value) : null,
            AlbumName = dto.album,
            AlbumId = dto.albumId,
            AlbumCoverUrl = dto.albumPic,
            AlbumCoverDocId = dto.albumPicDocId,
            SaveFormat = saveFormat,
            Bitrate = dto.bitrate,
            Mp3DocId = dto.mp3DocId,
            VolumeDelta = dto.volumeDelta,
            MvId = dto.mvId,
            Fee = dto.fee,
            Privilege = dto.privilege
        };
    }
}
