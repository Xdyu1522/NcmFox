using System.Diagnostics.CodeAnalysis;
using System.Text.Json;

namespace NcmFox.Models;

[SuppressMessage("ReSharper", "InconsistentNaming")]
internal class NcmMetaDataDto
{
    public required string musicId { get; set; }
    public required string musicName { get; set; }
    public required JsonElement[][] artist { get; set; }
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

internal static class MedaDataDtoMapper
{
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
            Artists = dto.artist
                          .Select(a => new Artist
                          {
                              Name = GetArtistName(a),
                              Id = GetArtistId(a)
                          })
                          .ToList(),
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

    private static string GetArtistName(JsonElement[] artist)
    {
        if (artist.Length == 0)
            return "Unknown";

        return artist[0].ValueKind switch
        {
            JsonValueKind.String => artist[0].GetString() ?? "Unknown",
            _ => "Unknown"
        };
    }

    private static string GetArtistId(JsonElement[] artist)
    {
        if (artist.Length < 2)
            return string.Empty;

        return artist[1].ValueKind switch
        {
            JsonValueKind.String => artist[1].GetString() ?? string.Empty,
            JsonValueKind.Number when artist[1].GetInt32() == 0 => string.Empty,
            JsonValueKind.Number => artist[1].GetInt32().ToString(),
            _ => string.Empty
        };
    }
}
