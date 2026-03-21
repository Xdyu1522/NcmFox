using NcmFox.Core;
using NcmFox.Models;

using System.Text.Json;

namespace NcmFox;

public static class MetaDataHelper
{
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