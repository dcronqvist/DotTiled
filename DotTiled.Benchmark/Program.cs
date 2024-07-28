using System;
using System.Security.Cryptography;
using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Running;

namespace MyBenchmarks
{
  public class MapLoader
  {
    public MapLoader()
    {
    }

    [Benchmark]
    public DotTiled.Map LoadWithDotTiled()
    {
      throw new NotImplementedException();
    }

    [Benchmark]
    public TiledLib.Map LoadWithTiledLib()
    {
      throw new NotImplementedException();
    }
  }

  public class Program
  {
    public static void Main(string[] args)
    {
      //var summary = BenchmarkRunner.Run<Md5VsSha256>();
    }
  }
}
