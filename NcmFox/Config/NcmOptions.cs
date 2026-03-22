namespace NcmFox.Config;

public sealed record NcmOptions
{
    public int BufferSize { get; init; } = 1024 * 1024;
    public int MaxCoverSize { get; init; } = 25 * 1024 * 1024;
    public int MaxMetadataSize { get; init; } = 1024 * 1024;
    public bool EnableMetaDataSizeCheck { get; init; } = true;
    public bool EnableCoverSizeCheck { get; init; } = true;
    
    public static NcmOptions Default { get; } = new();
}