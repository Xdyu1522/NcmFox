namespace NcmFox.Models;

public class NcmCover
{
    public required byte[] Data { get; init; }
    public required CoverFormat Format { get; init; }
    public int Size => Data.Length;
}