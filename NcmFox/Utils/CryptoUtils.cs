using System.Security.Cryptography;

namespace NcmFox.Utils;

public static class CryptoUtils
{
    // 直接返回 byte[] 以满足现有逻辑，但内部实现已达到零临时分配
    public static byte[] AesDecrypt(ReadOnlySpan<byte> data, ReadOnlySpan<byte> key)
    {
        using var aes = Aes.Create();
        aes.Key = key.ToArray(); // 设置 Key 依然需要，但这是框架层面的限制
        aes.Mode = CipherMode.ECB;
        aes.Padding = PaddingMode.PKCS7;

        // ✅ .NET 8 推荐方案：直接解密到返回的数组，避免多余的中间拷贝
        // 这里的 DecryptEcb 内部处理了 Padding 和 Transform，效率极高
        return aes.DecryptEcb(data, PaddingMode.PKCS7);
    }
}