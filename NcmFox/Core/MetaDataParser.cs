using System.Buffers.Text;
using System.Diagnostics.CodeAnalysis;
using System.Text;
using System.Text.Json;
using NcmFox.Models;
using NcmFox.Utils;
using NcmFox.Config;

namespace NcmFox.Core;

internal static class MetaDataParser
{
    public static MetaData? Parse(BinaryReader reader)
    {
        var length = reader.ReadInt32();

        if (length <= 0)
            return null;

        if (NcmConfig.Current.EnableMetaDataSizeCheck)
        {
            if (length > NcmConfig.Current.MaxMetadataSize)
            {
                throw new InvalidDataException($"Metadata size ({length} bytes) larger than max metadata size ({NcmConfig.Current.MaxMetadataSize} bytes)");
            }
        }

        var metadata = reader.ReadBytes(length);

        Xor(metadata);

        var json = DecipherJson(metadata);

        return ParseJson(json);
    }

    private static string DecipherJson(byte[] originBytes)
    {
        var base64Span = originBytes.AsSpan(22);

        var encrypted = new byte[Base64.GetMaxDecodedFromUtf8Length(base64Span.Length)];
        Base64.DecodeFromUtf8(base64Span, encrypted, out _, out int written);
        encrypted = encrypted.AsSpan(0, written).ToArray();

        var jsonBytes = CryptoUtils.AesDecrypt(encrypted, NcmConstants.MetaKey);

        var jsonSpan = jsonBytes.AsSpan(6);

        return Encoding.UTF8.GetString(jsonSpan);
    }

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



