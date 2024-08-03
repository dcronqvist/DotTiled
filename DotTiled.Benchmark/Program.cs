using System;
using System.Collections.Immutable;
using System.Security.Cryptography;
using System.Text;
using System.Xml;
using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Columns;
using BenchmarkDotNet.Configs;
using BenchmarkDotNet.Order;
using BenchmarkDotNet.Reports;
using BenchmarkDotNet.Running;

namespace MyBenchmarks
{
  [GroupBenchmarksBy(BenchmarkLogicalGroupRule.ByCategory)]
  [CategoriesColumn]
  [Orderer(SummaryOrderPolicy.FastestToSlowest)]
  public class MapLoading
  {
    private string _tmxPath = @"C:\Users\Daniel\winrepos\DotTiled\DotTiled.Tests\Serialization\Tmx\TestData\Map\empty-map-csv.tmx";
    private string _tmxContents = "";

    public MapLoading()
    {
      _tmxContents = System.IO.File.ReadAllText(_tmxPath);
    }

    [BenchmarkCategory("MapFromInMemoryTmxString")]
    [Benchmark(Baseline = true, Description = "DotTiled")]
    public DotTiled.Map LoadWithDotTiledFromInMemoryString()
    {
      using var stringReader = new StringReader(_tmxContents);
      using var xmlReader = XmlReader.Create(stringReader);
      using var mapReader = new DotTiled.TmxMapReader(xmlReader, _ => throw new Exception(), _ => throw new Exception());
      return mapReader.ReadMap();
    }

    [BenchmarkCategory("MapFromTmxFile")]
    [Benchmark(Baseline = true, Description = "DotTiled")]
    public DotTiled.Map LoadWithDotTiledFromFile()
    {
      using var fileStream = System.IO.File.OpenRead(_tmxPath);
      using var xmlReader = XmlReader.Create(fileStream);
      using var mapReader = new DotTiled.TmxMapReader(xmlReader, _ => throw new Exception(), _ => throw new Exception());
      return mapReader.ReadMap();
    }

    [BenchmarkCategory("MapFromInMemoryTmxString")]
    [Benchmark(Description = "TiledLib")]
    public TiledLib.Map LoadWithTiledLibFromInMemoryString()
    {
      using var memStream = new MemoryStream(Encoding.UTF8.GetBytes(_tmxContents));
      return TiledLib.Map.FromStream(memStream);
    }

    [BenchmarkCategory("MapFromTmxFile")]
    [Benchmark(Description = "TiledLib")]
    public TiledLib.Map LoadWithTiledLibFromFile()
    {
      using var fileStream = System.IO.File.OpenRead(_tmxPath);
      var map = TiledLib.Map.FromStream(fileStream);
      return map;
    }

    [BenchmarkCategory("MapFromInMemoryTmxString")]
    [Benchmark(Description = "TiledCSPlus")]
    public TiledCSPlus.TiledMap LoadWithTiledCSPlusFromInMemoryString()
    {
      using var memStream = new MemoryStream(Encoding.UTF8.GetBytes(_tmxContents));
      return new TiledCSPlus.TiledMap(memStream);
    }

    [BenchmarkCategory("MapFromTmxFile")]
    [Benchmark(Description = "TiledCSPlus")]
    public TiledCSPlus.TiledMap LoadWithTiledCSPlusFromFile()
    {
      using var fileStream = System.IO.File.OpenRead(_tmxPath);
      return new TiledCSPlus.TiledMap(fileStream);
    }
  }

  public class Program
  {
    public static void Main(string[] args)
    {
      var config = BenchmarkDotNet.Configs.DefaultConfig.Instance
        .WithOptions(ConfigOptions.DisableOptimizationsValidator)
        .AddDiagnoser(BenchmarkDotNet.Diagnosers.MemoryDiagnoser.Default);
      var summary = BenchmarkRunner.Run<MapLoading>(config);
    }
  }
}
