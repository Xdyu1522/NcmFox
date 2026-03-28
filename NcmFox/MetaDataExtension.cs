using NcmFox.Models;

namespace NcmFox;

public static class MetaDataExtension
{
    public static IReadOnlyList<string> GetArtists(this MetaData metaData)
    {
        var artists = metaData.Artists.Select(a => a.Name);
        return artists.ToList();
    }
}