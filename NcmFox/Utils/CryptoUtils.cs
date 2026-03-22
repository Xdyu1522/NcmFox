using System.Security.Cryptography;

namespace NcmFox.Utils;

public static class CryptoUtils
{
    private static byte[]? _cachedKey;
    private static int _cachedKeyHash;

    public static byte[] AesDecrypt(ReadOnlySpan<byte> data, ReadOnlySpan<byte> key)
    {
        using var aes = Aes.Create();
        aes.Mode = CipherMode.ECB;
        aes.Padding = PaddingMode.PKCS7;

        var keyHash = GetKeyHash(key);
        if (_cachedKey != null && _cachedKeyHash == keyHash && _cachedKey.Length == key.Length)
        {
            aes.Key = _cachedKey;
        }
        else
        {
            _cachedKey = key.ToArray();
            _cachedKeyHash = keyHash;
            aes.Key = _cachedKey;
        }

        return aes.DecryptEcb(data, PaddingMode.PKCS7);
    }

    private static int GetKeyHash(ReadOnlySpan<byte> key)
    {
        int hash = 17;
        foreach (var b in key)
            hash = hash * 31 + b;
        return hash;
    }
}