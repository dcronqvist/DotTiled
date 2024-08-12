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
  [HideColumns(["StdDev", "Error", "RatioSD"])]
  public class MapLoading
  {
    private string _tmxPath = @"C:\Users\Daniel\winrepos\DotTiled\DotTiled.Tests\Serialization\TestData\Map\empty-map-csv.tmx";
    private string _tmxContents = "";

    private string _tmjPath = @"C:\Users\Daniel\winrepos\DotTiled\DotTiled.Tests\Serialization\TestData\Map\empty-map-csv.tmj";
    private string _tmjContents = "";

    public MapLoading()
    {
      _tmxContents = System.IO.File.ReadAllText(_tmxPath);
      _tmjContents = System.IO.File.ReadAllText(_tmjPath);
    }

    [BenchmarkCategory("MapFromInMemoryTmxString")]
    [Benchmark(Baseline = true, Description = "DotTiled")]
    public DotTiled.Map LoadWithDotTiledFromInMemoryTmxString()
    {
      using var stringReader = new StringReader(_tmxContents);
      using var xmlReader = XmlReader.Create(stringReader);
      using var mapReader = new DotTiled.TmxMapReader(xmlReader, _ => throw new Exception(), _ => throw new Exception(), []);
      return mapReader.ReadMap();
    }

    [BenchmarkCategory("MapFromInMemoryTmjString")]
    [Benchmark(Baseline = true, Description = "DotTiled")]
    public DotTiled.Map LoadWithDotTiledFromInMemoryTmjString()
    {
      using var mapReader = new DotTiled.TmjMapReader(_tmjContents, _ => throw new Exception(), _ => throw new Exception(), []);
      return mapReader.ReadMap();
    }

    [BenchmarkCategory("MapFromInMemoryTmxString")]
    [Benchmark(Description = "TiledLib")]
    public TiledLib.Map LoadWithTiledLibFromInMemoryTmxString()
    {
      using var memStream = new MemoryStream(Encoding.UTF8.GetBytes(_tmxContents));
      return TiledLib.Map.FromStream(memStream);
    }

    [BenchmarkCategory("MapFromInMemoryTmxString")]
    [Benchmark(Description = "TiledCSPlus")]
    public TiledCSPlus.TiledMap LoadWithTiledCSPlusFromInMemoryTmxString()
    {
      using var memStream = new MemoryStream(Encoding.UTF8.GetBytes(_tmxContents));
      return new TiledCSPlus.TiledMap(memStream);
    }
  }

  public class Program
  {
    public static void Main(string[] args)
    {
      var config = BenchmarkDotNet.Configs.DefaultConfig.Instance
        .WithArtifactsPath(args[0])
        .WithOptions(ConfigOptions.DisableOptimizationsValidator)
        .AddDiagnoser(BenchmarkDotNet.Diagnosers.MemoryDiagnoser.Default);
      var summary = BenchmarkRunner.Run<MapLoading>(config);
    }
  }
}
