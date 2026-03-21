using System.Diagnostics.CodeAnalysis;

namespace NcmFox.Models;

[SuppressMessage("ReSharper", "InconsistentNaming")]
public class NcmMetaDataDto
{
    public required string musicId { get; init; }
    public required string musicName { get; init; }
    public required string[][] artist { get; init; }
    public required string album { get; init; }
    public required string albumPic { get; init; }
    public required string format { get; init; }
}