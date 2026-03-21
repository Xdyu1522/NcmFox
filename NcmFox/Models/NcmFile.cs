namespace NcmFox.Models;

public sealed class NcmFile
{
    public required FileInfo FileInfo;
    public required MetaData? MetaData;
    public required byte[] KeyBox { get; init; }
    public long AudioOffset { get; init; }
    public required NcmCover? CoverData { get; init; }
    public SaveFormat SaveFormat => MetaData?.SaveFormat ?? SaveFormat.Unknown;
    public bool IsInitialized { get; init; } = false;
}