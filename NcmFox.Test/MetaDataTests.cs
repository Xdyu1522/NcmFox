using FluentAssertions;
using NcmFox.Config;
using NcmFox.Core;
using NcmFox.Models;
using TagLib;
using File = System.IO.File;

namespace NcmFox.Test;

public class MetaDataTests
{
    [Fact]
    public void Ncm_File_MetaData_Should_Parse()
    {
        var ncm = NcmDecoder.Open(@"E:\CloudMusic\VipSongsDownload\闹闹丶,洛天依 - 星球卑.ncm");
        var meta = ncm.MetaData;
        var coverData = ncm.CoverData;
        meta.Should().NotBeNull();
        meta.SongName.Should().Be("星球卑");
        meta.Artists.Should().HaveCount(2).And.Contain("洛天依");
        meta.AlbumName.Should().Be("星球卑");
        coverData.Should().NotBeNull();
        coverData.Format.Should().Be(CoverFormat.Png);
    }

    [Fact]
    public void Ncm_Data_Should_Parse()
    {
        NcmConfig.Configure(new NcmOptions(){BufferSize = 65536});
        var ncm = NcmDecoder.Open(@"E:\CloudMusic\VipSongsDownload\闹闹丶,洛天依 - 星球卑.ncm");
        using var outputStream = File.OpenWrite(@"D:\Testsss.flac");
        ncm.Decode(outputStream);
    }

    [Fact]
    public void String_MetaData_Should_Parse()
    {
        using var file = TagLib.File.Create(@"E:\CloudMusic\ARForest - Inverted World.flac");
        var comment = file.Tag.Description;
        var result = MetaDataHelper.TryGetMetaData(comment, out var metaData);
        result.Should().BeTrue();
        metaData.Should().NotBeNull();
        metaData.SongName.Should().Be("Inverted World");
    }
}