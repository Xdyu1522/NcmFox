# NcmFox

[English](README_EN.md) | 简体中文

[![NuGet Version](https://img.shields.io/nuget/v/NcmFox?style=flat-square)](https://www.nuget.org/packages/NcmFox/)
[![NuGet Downloads](https://img.shields.io/nuget/dt/NcmFox?style=flat-square)](https://www.nuget.org/packages/NcmFox/)
[![License](https://img.shields.io/github/license/Xdyu1522/NcmFox?style=flat-square)](LICENSE)
[![.NET](https://img.shields.io/badge/.NET-8.0%20%7C%209.0%20%7C%2010.0-512BD4?style=flat-square)](https://dotnet.microsoft.com/)

一个高性能的 .NET NCM 文件解密库，专为网易云音乐加密音频文件设计。

## ✨ 项目特色

- 🚀 **极致性能** - 采用 SIMD (AVX2/SSE2) 向量化加速，解密速度可达 400-500 MB/s
- 💾 **低内存占用** - 使用 `ArrayPool` 内存池技术，大幅降低 GC 压力
- 🎯 **简洁 API** - 直观易用的接口设计，几行代码即可完成解密
- 📦 **完整元数据** - 支持提取歌曲信息、艺术家、专辑封面等完整元数据
- 🔧 **灵活配置** - 支持自定义缓冲区大小、安全限制等参数

## 📋 环境要求

- .NET 8.0 或更高版本
- 支持的平台：Windows、Linux、macOS

## 📦 安装

### 通过 NuGet 安装

```bash
dotnet add package NcmFox
```

### 通过 Package Manager 安装

```powershell
Install-Package NcmFox
```

### 从源码构建

```bash
git clone https://github.com/Xdyu1522/NcmFox.git
cd NcmFox/NcmFox
dotnet build
```

## 🚀 快速开始

### 基础用法

```csharp
using NcmFox;

// 打开 NCM 文件
var ncm = NcmDecoder.Open("song.ncm");

// 查看元数据
Console.WriteLine($"歌曲: {ncm.MetaData.SongName}");
Console.WriteLine($"艺术家: {string.Join(", ", ncm.MetaData.GetArtists())}");
Console.WriteLine($"专辑: {ncm.MetaData.AlbumName}");
Console.WriteLine($"格式: {ncm.SaveFormat}");

// 解密到文件
ncm.Decode("output.flac");

// 或解密到流
using var ms = new MemoryStream();
ncm.Decode(ms);
```

### 自定义配置

```csharp
using NcmFox.Config;

// 配置全局选项
NcmConfig.Configure(new NcmOptions
{
    BufferSize = 64 * 1024,           // 64 KB 缓冲区
    MaxCoverSize = 10 * 1024 * 1024,  // 最大封面 10 MB
    MaxMetadataSize = 512 * 1024,     // 最大元数据 512 KB
    EnableCoverSizeCheck = true,      // 启用封面大小检查
    EnableMetaDataSizeCheck = true    // 启用元数据大小检查
});
```

### 仅提取元数据

```csharp
using NcmFox;

// 从文件提取（无异常抛出）
if (MetaDataHelper.TryGetMetaData(new FileInfo("song.ncm"), out var meta))
{
    Console.WriteLine($"歌曲: {meta.SongName}");
    Console.WriteLine($"艺术家: {string.Join(", ", meta.GetArtists())}");
    Console.WriteLine($"专辑: {meta.AlbumName}");
    Console.WriteLine($"比特率: {meta.Bitrate}");
    Console.WriteLine($"时长: {meta.Duration}");
}

// 从加密字符串提取
string comment = "163 key(Don't modify):...";
if (MetaDataHelper.TryGetMetaData(comment, out var meta2))
{
    Console.WriteLine($"歌曲: {meta2.SongName}");
}
```

### 处理封面图像

```csharp
var ncm = NcmDecoder.Open("song.ncm");

if (ncm.CoverData != null)
{
    Console.WriteLine($"封面格式: {ncm.CoverData.Format}");
    Console.WriteLine($"封面大小: {ncm.CoverData.Size} 字节");
    
    // 保存封面到文件
    File.WriteAllBytes("cover.jpg", ncm.CoverData.Data);
}
```

## 📖 API 参考

### NcmDecoder

| 方法 | 说明 |
|------|------|
| `Open(string path)` | 打开并解析 NCM 文件 |
| `Open(FileInfo file)` | 通过 FileInfo 打开 NCM 文件 |

### NcmFile

| 属性 | 类型 | 说明 |
|------|------|------|
| `FileInfo` | `FileInfo` | 文件系统信息 |
| `MetaData` | `MetaData?` | 歌曲元数据 |
| `CoverData` | `NcmCover?` | 封面图像数据 |
| `SaveFormat` | `SaveFormat` | 音频格式 (FLAC/MP3) |
| `KeyBox` | `byte[]` | 解密密钥盒 |

### NcmFile 扩展方法

| 方法 | 说明 |
|------|------|
| `Decode(string outputPath)` | 解密并保存到文件 |
| `Decode(Stream output)` | 解密并写入流 |
| `Decode()` | 解密并返回字节数组 |

### MetaData

| 属性 | 类型 | 说明 |
|------|------|------|
| `Id` | `string` | 歌曲唯一标识符 |
| `SongName` | `string` | 歌曲名称 |
| `Artists` | `IReadOnlyList<Artist>` | 艺术家列表 |
| `Alias` | `IReadOnlyList<string>` | 歌曲别名列表 |
| `TranslatedNames` | `IReadOnlyList<string>` | 翻译名称列表 |
| `Duration` | `TimeSpan?` | 歌曲时长 |
| `AlbumName` | `string` | 专辑名称 |
| `AlbumId` | `string` | 专辑唯一标识符 |
| `AlbumCoverUrl` | `string` | 专辑封面 URL |
| `AlbumCoverDocId` | `string?` | 封面文档 ID |
| `SaveFormat` | `SaveFormat` | 音频格式 |
| `Bitrate` | `int?` | 比特率 (bps) |
| `Mp3DocId` | `string?` | MP3 文档 ID |
| `VolumeDelta` | `double?` | 音量调整值 (dB) |
| `MvId` | `string?` | MV 标识符 |
| `Fee` | `int?` | 费用类型 |
| `Privilege` | `Privilege?` | 权限信息 |

### Artist

| 属性 | 类型 | 说明 |
|------|------|------|
| `Name` | `string` | 艺术家名称 |
| `Id` | `string` | 艺术家唯一标识符 |

### NcmOptions

| 属性 | 默认值 | 说明 |
|------|--------|------|
| `BufferSize` | 1 MB | 解密缓冲区大小 |
| `MaxCoverSize` | 25 MB | 最大封面图像大小 |
| `MaxMetadataSize` | 1 MB | 最大元数据大小 |
| `EnableCoverSizeCheck` | true | 启用封面大小验证 |
| `EnableMetaDataSizeCheck` | true | 启用元数据大小验证 |

## ⚡ 性能

NcmFox 针对高吞吐量进行了深度优化：

| 优化技术 | 说明 |
|----------|------|
| SIMD 加速 | 使用 AVX2/SSE2 进行 XOR 运算 |
| 内存池 | `ArrayPool<byte>` 减少内存分配 |
| 循环展开 | 8 倍展开的标量回退路径 |
| 零拷贝 | 最小化中间内存分配 |

典型性能表现：

| 场景 | 吞吐量 |
|------|--------|
| FLAC 文件 | 400-500 MB/s |
| MP3 文件 | 300-400 MB/s |

## 🤝 贡献指南

欢迎提交 Issue 和 Pull Request！

1. Fork 本仓库
2. 创建特性分支 (`git checkout -b feature/AmazingFeature`)
3. 提交更改 (`git commit -m 'Add some AmazingFeature'`)
4. 推送到分支 (`git push origin feature/AmazingFeature`)
5. 创建 Pull Request

## 📄 开源协议

本项目采用 [MIT](LICENSE) 协议开源。

## ⚠️ 免责声明

本库仅供学习和个人使用。请尊重版权法律和网易云音乐的服务条款。

## 📮 联系方式

- GitHub Issues: [https://github.com/Xdyu1522/NcmFox/issues](https://github.com/Xdyu1522/NcmFox/issues)

## 🙏 致谢

感谢 JetBrains 为本项目提供 Rider IDE 的非商业许可证支持，这对项目的开发提供了极大的帮助。

[![JetBrains](https://img.shields.io/badge/Supported%20by-JetBrains-000000?style=flat-square&logo=jetbrains)](https://www.jetbrains.com/)

**Made with ❤️ by Xdyuorine**
