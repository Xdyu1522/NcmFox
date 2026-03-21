using System.Diagnostics.CodeAnalysis;
using System.Text;
using System.Text.Json;
using NcmFox.Models;
using NcmFox.Utils;

namespace NcmFox.Core;

internal static class MetaDataParser
{
    public static MetaData? Parse(BinaryReader reader)
    {
        var length = reader.ReadInt32();

        if (length <= 0)
            return null;

        var metadata = reader.ReadBytes(length);

        Xor(metadata);

        var json = DecipherJson(metadata);

        return ParseJson(json);
    }

    /// <summary>
    /// Deciphers a JSON string from the provided encrypted byte array.
    /// </summary>
    /// <param name="originBytes">The encrypted byte array to decipher.</param>
    /// <returns>The deciphered JSON string, or an empty string if decryption fails.</returns>
    private static string DecipherJson(byte[] originBytes)
    {
        // 跳过 "163 key(Don't modify):"
        var base64Span = originBytes.AsSpan(22);
        
        var encrypted = Convert.FromBase64String(
            Encoding.UTF8.GetString(base64Span)
        );
        
        var jsonBytes = CryptoUtils.AesDecrypt(
            encrypted,
            NcmConstants.MetaKey
        );
        
        // 跳过 "music:"
        var jsonSpan = jsonBytes.AsSpan(6);
        
        return Encoding.UTF8.GetString(jsonSpan);
    }

    /// <summary>
    /// Deciphers a JSON string from the provided encrypted string.
    /// </summary>
    /// <param name="originString">The encrypted string to decipher.</param>
    /// <returns>The deciphered JSON string, or an empty string if decryption fails.</returns>
    /// <exception cref="InvalidDataException">Thrown when the input string is null or empty.</exception>
    public static string DecipherJson(string originString)
    {
        if (string.IsNullOrEmpty(originString))
            throw new InvalidDataException();
        var originBytes = Encoding.UTF8.GetBytes(originString);
        return DecipherJson(originBytes);
    }

    private static void Xor(byte[] data)
    {
        for (var i = 0; i < data.Length; i++)
            data[i] ^= 0x63;
    }

    public static MetaData ParseJson(string json)
    {
        var dto = JsonSerializer.Deserialize<NcmMetaDataDto>(json);

        if (dto == null)
        {
            throw new InvalidDataException($"Can't parse json: {json}");
        }

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
            AlbumName = dto.album,
            AlbumCoverUrl = dto.albumPic,
            SaveFormat = saveFormat,
            Artists = dto.artist.Select(a => a[0]?.ToString())
                .Where(a => a != null)
                .ToArray()
        };
    }
}



