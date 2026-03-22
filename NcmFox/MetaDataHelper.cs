using NcmFox.Core;
using NcmFox.Models;

namespace NcmFox;

/// <summary>
/// Provides helper methods for extracting metadata from NCM files and encrypted strings.
/// </summary>
/// <remarks>
/// <para>
/// This class offers convenient methods to extract metadata without fully processing the NCM file.
/// It is useful when you only need song information without decrypting the audio content.
/// </para>
/// </remarks>
public static class MetaDataHelper
{
    /// <summary>
    /// Attempts to extract metadata from an NCM file.
    /// </summary>
    /// <param name="file">The <see cref="FileInfo"/> representing the NCM file.</param>
    /// <param name="metaData">
    /// When this method returns <c>true</c>, contains the extracted <see cref="MetaData"/>;
    /// otherwise, <c>null</c>.
    /// </param>
    /// <returns>
    /// <c>true</c> if metadata was successfully extracted; otherwise, <c>false</c>.
    /// </returns>
    /// <remarks>
    /// <para>
    /// This method catches all exceptions internally and returns <c>false</c> on any error.
    /// Use this method when you want to safely extract metadata without handling exceptions.
    /// </para>
    /// <para>
    /// Example:
    /// <code>
    /// if (MetaDataHelper.TryGetMetaData(new FileInfo("song.ncm"), out var meta))
    /// {
    ///     Console.WriteLine($"Song: {meta?.SongName}");
    /// }
    /// </code>
    /// </para>
    /// </remarks>
    public static bool TryGetMetaData(FileInfo file, out MetaData? metaData)
    {
        metaData = null;
        try
        {
            var ncmFile = NcmDecoder.Open(file);
            metaData = ncmFile.MetaData;
            return ncmFile.MetaData != null;
        }
        catch
        {
            return false;
        }
    }

    /// <summary>
    /// Attempts to extract metadata from an encrypted metadata string.
    /// </summary>
    /// <param name="encryptedString">
    /// The encrypted metadata string, typically in the format "163 key(Don't modify):...".
    /// This can be found in the comment field of decrypted audio files.
    /// </param>
    /// <param name="metaData">
    /// When this method returns <c>true</c>, contains the extracted <see cref="MetaData"/>;
    /// otherwise, <c>null</c>.
    /// </param>
    /// <returns>
    /// <c>true</c> if metadata was successfully extracted; otherwise, <c>false</c>.
    /// </returns>
    /// <remarks>
    /// <para>
    /// This method is useful for extracting metadata from audio files that were previously
    /// decrypted, where the metadata is stored in the comment/tag field.
    /// </para>
    /// <para>
    /// Example:
    /// <code>
    /// string comment = "163 key(Don't modify):L64FU3W4YxX3ZFTmbZ...";
    /// if (MetaDataHelper.TryGetMetaData(comment, out var meta))
    /// {
    ///     Console.WriteLine($"Song: {meta?.SongName}");
    /// }
    /// </code>
    /// </para>
    /// </remarks>
    public static bool TryGetMetaData(string encryptedString, out MetaData? metaData)
    {
        metaData = null;
        try
        {
            var json = MetaDataParser.DecipherJson(encryptedString);
            metaData = MetaDataParser.ParseJson(json);
            return true;
        }
        catch
        {
            return false;
        }
    }
}