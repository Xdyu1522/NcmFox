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
        meta.Artists.Should().HaveCount(2).And.Contain(a => a.Name == "洛天依");
        meta.GetArtists();
        meta.AlbumName.Should().Be("星球卑");
        meta.Bitrate.Should().Be(3999000);
        meta.VolumeDelta.Should().Be(-8.4195);
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
        var comment = "163 key(Don't modify):L64FU3W4YxX3ZFTmbZ+8/W4HcKVCxy2JN1Q8nMrdgD0nS8fJXSW/HdU6vdcylkmoQFYBGvEvZ/gP3h8hrLdx3iX1WhRZYmx1g7ycjDzPWRM8EGejoJvU8j6A1vmPM19c7pSfRDCkM5VIhnJXv5E60GKyC6C0zTXSFFZOHNU7Kb0Tqifd40KM/sfG2DilvHkFngayVOQWfrB/8JWpAZQjm5t6on/Ea9/RCrLUkXfX8cL08SvXi+lJq+PNzlYJji4KZxWWUUlbqsxTBJYTCLT+8QsYNA9EULI+u+PAi/lxDG/MOULjJpyUHncgm4S1zN36picU2KPFmvWOuypL12oLRt5LQ3IEzBhU9o1AjUpVfG6PGcCRQuvzZV4BlXpX1ga3evyPwCw4AWslzxAVTnReGaFZtb5uEoR8ade5/3BSg/Yr9280shc0HqrlkMZIVSmmnpxIEi7uYeGjZPyhkJs5RrM1mpfrRPDSTPr3Y7b7NMljyt7xnon3797z3UGbWGuDx/8aRdBp5mgbBXY4kv31PZaGnPVq7QQ5ALuCnqBQpmbzrwbncX//3ZpMiH3I5BEhiGNXu8VHtrkmP8Pj9rZp82bu/UU7zuvz5obWatgsXAPQN/luNuQd6hmHuLrPN+IP";
        var result = MetaDataHelper.TryGetMetaData(comment, out var metaData);
        result.Should().BeTrue();
        metaData.Should().NotBeNull();
        metaData.SongName.Should().Be("Inverted World");
    }

    [Fact]
    public void Multi_NcmFile_Decrypt()
    {
        var paths = GetTestFiles();
        var outputStream = Stream.Null;
        foreach (var f in paths)
        {
            var file = NcmDecoder.Open(f);
            file.Decode(outputStream);
        }
    }

    public List<string> GetTestFiles()
    {
        var paths = Directory.GetFiles(@"D:\VipSongsDownload", "*.ncm") .Take(32);
        return paths.ToList();
    }

    public string GetEncryptedMetaDataFromFile(string filePath)
    {
        using var file = TagLib.File.Create(filePath);
        var comment = file.Tag.Description;
        return comment;
    }
}