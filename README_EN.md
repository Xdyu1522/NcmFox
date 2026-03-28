# NcmFox

English | [简体中文](README.md)

[![NuGet Version](https://img.shields.io/nuget/v/NcmFox?style=flat-square)](https://www.nuget.org/packages/NcmFox/)
[![NuGet Downloads](https://img.shields.io/nuget/dt/NcmFox?style=flat-square)](https://www.nuget.org/packages/NcmFox/)
[![License](https://img.shields.io/github/license/Xdyu1522/NcmFox?style=flat-square)](LICENSE)
[![.NET](https://img.shields.io/badge/.NET-8.0%20%7C%209.0%20%7C%2010.0-512BD4?style=flat-square)](https://dotnet.microsoft.com/)

A high-performance .NET library for decrypting NCM files from NetEase Cloud Music.

## ✨ Features

- 🚀 **Extreme Performance** - SIMD (AVX2/SSE2) vectorized acceleration with 400-500 MB/s throughput
- 💾 **Low Memory Footprint** - Uses `ArrayPool` memory pooling to minimize GC pressure
- 🎯 **Simple API** - Intuitive interface design, decrypt files with just a few lines of code
- 📦 **Complete Metadata** - Extract song info, artists, album cover, and more
- 🔧 **Flexible Configuration** - Customize buffer sizes, security limits, and more

## 📋 Requirements

- .NET 8.0 or higher
- Supported platforms: Windows, Linux, macOS

## 📦 Installation

### Via NuGet

```bash
dotnet add package NcmFox
```

### Via Package Manager

```powershell
Install-Package NcmFox
```

### Build from Source

```bash
git clone https://github.com/Xdyu1522/NcmFox.git
cd NcmFox/NcmFox
dotnet build
```

## 🚀 Quick Start

### Basic Usage

```csharp
using NcmFox;

// Open an NCM file
var ncm = NcmDecoder.Open("song.ncm");

// Access metadata
Console.WriteLine($"Song: {ncm.MetaData.SongName}");
Console.WriteLine($"Artists: {string.Join(", ", ncm.MetaData.GetArtists())}");
Console.WriteLine($"Album: {ncm.MetaData.AlbumName}");
Console.WriteLine($"Format: {ncm.SaveFormat}");

// Decrypt to file
ncm.Decode("output.flac");

// Or decrypt to stream
using var ms = new MemoryStream();
ncm.Decode(ms);
```

### Custom Configuration

```csharp
using NcmFox.Config;

// Configure global options
NcmConfig.Configure(new NcmOptions
{
    BufferSize = 64 * 1024,           // 64 KB buffer
    MaxCoverSize = 10 * 1024 * 1024,  // 10 MB max cover size
    MaxMetadataSize = 512 * 1024,     // 512 KB max metadata
    EnableCoverSizeCheck = true,      // Enable cover size validation
    EnableMetaDataSizeCheck = true    // Enable metadata size validation
});
```

### Extract Metadata Only

```csharp
using NcmFox;

// Extract from file (no exceptions thrown)
if (MetaDataHelper.TryGetMetaData(new FileInfo("song.ncm"), out var meta))
{
    Console.WriteLine($"Song: {meta.SongName}");
    Console.WriteLine($"Artists: {string.Join(", ", meta.GetArtists())}");
    Console.WriteLine($"Album: {meta.AlbumName}");
    Console.WriteLine($"Bitrate: {meta.Bitrate}");
    Console.WriteLine($"Duration: {meta.Duration}");
}

// Extract from encrypted string
string comment = "163 key(Don't modify):...";
if (MetaDataHelper.TryGetMetaData(comment, out var meta2))
{
    Console.WriteLine($"Song: {meta2.SongName}");
}
```

### Handle Cover Image

```csharp
var ncm = NcmDecoder.Open("song.ncm");

if (ncm.CoverData != null)
{
    Console.WriteLine($"Cover Format: {ncm.CoverData.Format}");
    Console.WriteLine($"Cover Size: {ncm.CoverData.Size} bytes");
    
    // Save cover to file
    File.WriteAllBytes("cover.jpg", ncm.CoverData.Data);
}
```

## 📖 API Reference

### NcmDecoder

| Method | Description |
|--------|-------------|
| `Open(string path)` | Opens and parses an NCM file |
| `Open(FileInfo file)` | Opens an NCM file via FileInfo |

### NcmFile

| Property | Type | Description |
|----------|------|-------------|
| `FileInfo` | `FileInfo` | File system information |
| `MetaData` | `MetaData?` | Song metadata |
| `CoverData` | `NcmCover?` | Cover image data |
| `SaveFormat` | `SaveFormat` | Audio format (FLAC/MP3) |
| `KeyBox` | `byte[]` | Decryption key box |

### NcmFile Extension Methods

| Method | Description |
|--------|-------------|
| `Decode(string outputPath)` | Decrypts and saves to file |
| `Decode(Stream output)` | Decrypts and writes to stream |
| `Decode()` | Decrypts and returns byte array |

### MetaData

| Property | Type | Description |
|----------|------|-------------|
| `Id` | `string` | Unique song identifier |
| `SongName` | `string` | Song title |
| `Artists` | `IReadOnlyList<Artist>` | List of artists |
| `Alias` | `IReadOnlyList<string>` | Song alias names |
| `TranslatedNames` | `IReadOnlyList<string>` | Translated song names |
| `Duration` | `TimeSpan?` | Song duration |
| `AlbumName` | `string` | Album name |
| `AlbumId` | `string` | Unique album identifier |
| `AlbumCoverUrl` | `string` | Album cover URL |
| `AlbumCoverDocId` | `string?` | Cover document ID |
| `SaveFormat` | `SaveFormat` | Audio format |
| `Bitrate` | `int?` | Bitrate (bps) |
| `Mp3DocId` | `string?` | MP3 document ID |
| `VolumeDelta` | `double?` | Volume adjustment (dB) |
| `MvId` | `string?` | Music video identifier |
| `Fee` | `int?` | Fee type |
| `Privilege` | `Privilege?` | Privilege information |

### Artist

| Property | Type | Description |
|----------|------|-------------|
| `Name` | `string` | Artist name |
| `Id` | `string` | Unique artist identifier |

### NcmOptions

| Property | Default | Description |
|----------|---------|-------------|
| `BufferSize` | 1 MB | Decryption buffer size |
| `MaxCoverSize` | 25 MB | Maximum cover image size |
| `MaxMetadataSize` | 1 MB | Maximum metadata size |
| `EnableCoverSizeCheck` | true | Enable cover size validation |
| `EnableMetaDataSizeCheck` | true | Enable metadata size validation |

## ⚡ Performance

NcmFox is deeply optimized for high throughput:

| Optimization | Description |
|--------------|-------------|
| SIMD Acceleration | Uses AVX2/SSE2 for XOR operations |
| Memory Pooling | `ArrayPool<byte>` reduces allocations |
| Loop Unrolling | 8x unrolled scalar fallback path |
| Zero-Copy | Minimizes intermediate allocations |

Typical performance:

| Scenario | Throughput |
|----------|------------|
| FLAC files | 400-500 MB/s |
| MP3 files | 300-400 MB/s |

## 🤝 Contributing

Issues and Pull Requests are welcome!

1. Fork this repository
2. Create a feature branch (`git checkout -b feature/AmazingFeature`)
3. Commit your changes (`git commit -m 'Add some AmazingFeature'`)
4. Push to the branch (`git push origin feature/AmazingFeature`)
5. Create a Pull Request

## 📄 License

This project is licensed under the [MIT](LICENSE) License.

## ⚠️ Disclaimer

This library is for educational and personal use only. Please respect copyright laws and NetEase Cloud Music's terms of service.

## 📮 Contact

- GitHub Issues: [https://github.com/Xdyu1522/NcmFox/issues](https://github.com/Xdyu1522/NcmFox/issues)

## 🙏 Acknowledgements

Special thanks to JetBrains for providing a non-commercial license for Rider, which greatly supports the development of this project.

[![JetBrains](https://img.shields.io/badge/Supported%20by-JetBrains-000000?style=flat-square&logo=jetbrains)](https://www.jetbrains.com/)

**Made with ❤️ by Xdyuorine**
