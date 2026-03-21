using NcmFox.Utils;

namespace NcmFox.Core;

internal static class KeyDataParser
{
    public static byte[] Parse(BinaryReader reader)
    {
        var length = reader.ReadInt32();

        var keyData = reader.ReadBytes(length);

        XorKeyData(keyData);

        var decrypted = CryptoUtils.AesDecrypt(
            keyData,
            NcmConstants.CoreKey);

        var key = decrypted.AsSpan(17);

        return KeyBoxBuilder.Build(key);
    }

    private static void XorKeyData(Span<byte> data)
    {
        for (var i = 0; i < data.Length; i++)
        {
            data[i] ^= 0x64;
        }
    }
}