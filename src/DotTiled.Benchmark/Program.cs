using System.Runtime.CompilerServices;
using System.Text;
using System.Xml;
using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Columns;
using BenchmarkDotNet.Configs;
using BenchmarkDotNet.Order;
using BenchmarkDotNet.Running;

namespace DotTiled.Benchmark
{
  [GroupBenchmarksBy(BenchmarkLogicalGroupRule.ByCategory)]
  [CategoriesColumn]
  [Orderer(SummaryOrderPolicy.FastestToSlowest)]
  [HideColumns(["StdDev", "Error", "RatioSD"])]
  public class MapLoading
  {
    private readonly string _tmxPath = @"DotTiled.Tests/TestData/Maps/default-map/default-map.tmx";
    private readonly string _tmxContents = "";

    private readonly string _tmjPath = @"DotTiled.Tests/TestData/Maps/default-map/default-map.tmj";
    private readonly string _tmjContents = "";

    public MapLoading()
    {
      var basePath = Path.GetDirectoryName(WhereAmI())!;
      var tmxPath = Path.Combine(basePath, $"../{_tmxPath}");
      var tmjPath = Path.Combine(basePath, $"../{_tmjPath}");

      _tmxContents = File.ReadAllText(tmxPath);
      _tmjContents = File.ReadAllText(tmjPath);
    }

    private static string WhereAmI([CallerFilePath] string callerFilePath = "") => callerFilePath;

    [BenchmarkCategory("MapFromInMemoryTmxString")]
    [Benchmark(Baseline = true, Description = "DotTiled")]
    public DotTiled.Map LoadWithDotTiledFromInMemoryTmxString()
    {
      using var stringReader = new StringReader(_tmxContents);
      using var xmlReader = XmlReader.Create(stringReader);
      using var mapReader = new DotTiled.Serialization.Tmx.TmxMapReader(xmlReader, _ => throw new NotSupportedException(), _ => throw new NotSupportedException(), _ => throw new NotSupportedException());
      return mapReader.ReadMap();
    }

    [BenchmarkCategory("MapFromInMemoryTmjString")]
    [Benchmark(Baseline = true, Description = "DotTiled")]
    public DotTiled.Map LoadWithDotTiledFromInMemoryTmjString()
    {
      using var mapReader = new DotTiled.Serialization.Tmj.TmjMapReader(_tmjContents, _ => throw new NotSupportedException(), _ => throw new NotSupportedException(), _ => throw new NotSupportedException());
      return mapReader.ReadMap();
    }

    [BenchmarkCategory("MapFromInMemoryTmxString")]
    [Benchmark(Description = "TiledLib")]
    public TiledLib.Map LoadWithTiledLibFromInMemoryTmxString()
    {
      using var memStream = new MemoryStream(Encoding.UTF8.GetBytes(_tmxContents));
      return TiledLib.Map.FromStream(memStream);
    }

    [BenchmarkCategory("MapFromInMemoryTmjString")]
    [Benchmark(Description = "TiledLib")]
    public TiledLib.Map LoadWithTiledLibFromInMemoryTmjString()
    {
      using var memStream = new MemoryStream(Encoding.UTF8.GetBytes(_tmjContents));
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
      var config = DefaultConfig.Instance
        .WithArtifactsPath(args[0])
        .WithOptions(ConfigOptions.DisableOptimizationsValidator)
        .AddDiagnoser(BenchmarkDotNet.Diagnosers.MemoryDiagnoser.Default);
      _ = BenchmarkRunner.Run<MapLoading>(config);
    }
  }
}
