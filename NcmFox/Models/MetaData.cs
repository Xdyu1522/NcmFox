namespace NcmFox.Models;

public sealed class MetaData
{
    public required string Id { get; init; }
    public required string SongName { get; init; }
    public required string?[]? Artists { get; init; }
    public required string AlbumName { get; init; }
    public required string AlbumCoverUrl { get; init; }
    public required SaveFormat SaveFormat { get; init; }
}