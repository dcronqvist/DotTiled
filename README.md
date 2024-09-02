# 📚 DotTiled

<img src="https://www.mapeditor.org/img/tiled-logo-white.png" align="right" width="20%"/>

DotTiled is a simple and easy-to-use library for loading, saving, and managing [Tiled maps and tilesets](https://mapeditor.org) in your .NET projects. After [TiledCS](https://github.com/TheBoneJarmer/TiledCS) unfortunately became unmaintained (since 2022), I aimed to create a new library that could fill its shoes. DotTiled is the result of that effort.

DotTiled is designed to be a lightweight and efficient library that provides a simple API for loading and managing Tiled maps and tilesets. It is built with performance in mind and aims to be as fast and memory-efficient as possible.

- [Alternative libraries and comparison + benchmarks](#alternative-libraries-and-comparison)
- [Feature coverage comparison](#feature-coverage-comparison)
- [Quick Start](#quick-start)

# Alternative libraries and comparison

Other similar libraries exist, and you may want to consider them for your project as well:

|**Comparison**|**DotTiled**|[TiledLib](https://github.com/Ragath/TiledLib.Net)|[TiledCSPlus](https://github.com/nolemretaWxd/TiledCSPlus)|[TiledSharp](https://github.com/marshallward/TiledSharp)|[TiledCS](https://github.com/TheBoneJarmer/TiledCS)|[TiledNet](https://github.com/napen123/Tiled.Net)|
|---------------------------------|:-----------------------:|:--------:|:-----------:|:----------:|:-------:|:------:|
| Actively maintained             |            ✅          |     ✅   |     ✅      |      ❌   |    ❌  |   ❌   |
| Benchmark (time)*               |           1.00          |   1.83   |     2.16    |      -     |    -    |    -   |
| Benchmark (memory)*             |           1.00          |   1.43   |     2.03    |      -     |    -    |    -   |
| .NET Targets                    | `net8.0` | `net8.0` |`netstandard2.1`|`netstandard2.0`|`netstandard2.0`|`net45`|
| Docs                            |Usage, API,<br>XML Docs|Usage|Usage, API,<br>XML Docs|Usage, API|Usage, XML Docs|Usage, XML Docs|
| License                         |           MIT           |   MIT    |     MIT     | Apache-2.0 |   MIT   | BSD 3-Clause |

> [!NOTE]
> *Both benchmark time and memory ratios are relative to DotTiled. Lower is better. Benchmark (time) refers to the execution time of loading the same map from an in-memory string that contains XML data in the `.tmx` format. Benchmark (memory) refers to the memory allocated during that loading process. For further details on the benchmark results, see the collapsible section below.

[MonoGame](https://www.monogame.net) users may also want to consider using [MonoGame.Extended](https://github.com/craftworkgames/MonoGame.Extended) for loading Tiled maps and tilesets. Like MonoGame.Extended, DotTiled also provides a way to properly import Tiled maps and tilesets with the MonoGame content pipeline (with the DotTiled.MonoGame.Pipeline NuGet). However, unlike MonoGame.Extended, DotTiled does *not* include any kind of rendering capabilities, and it is up to you as a developer to implement any kind of rendering for your maps when using DotTiled. The feature coverage by MonoGame.Extended is less than that of DotTiled, so you may want to consider using DotTiled if you need access to more Tiled features and flexibility.

<details>
<summary>
Benchmark details
</summary>

The following benchmark results were gathered using the `DotTiled.Benchmark` project which uses [BenchmarkDotNet](https://benchmarkdotnet.org/) to compare the performance of DotTiled with other similar libraries. The benchmark results are grouped by category and show the mean execution time, memory consumption metrics, and ratio to DotTiled.

```
BenchmarkDotNet v0.13.12, Windows 10 (10.0.19045.4651/22H2/2022Update)
12th Gen Intel Core i7-12700K, 1 CPU, 20 logical and 12 physical cores
.NET SDK 8.0.202
  [Host]     : .NET 8.0.3 (8.0.324.11423), X64 RyuJIT AVX2
  DefaultJob : .NET 8.0.3 (8.0.324.11423), X64 RyuJIT AVX2
```
| Method      | Categories               | Mean     | Ratio | Gen0   | Gen1   | Allocated | Alloc Ratio |
|------------ |------------------------- |---------:|------:|-------:|-------:|----------:|------------:|
| DotTiled    | MapFromInMemoryTmjString | 4.431 μs |  1.00 | 0.4349 |      - |   5.58 KB |        1.00 |
| TiledLib    | MapFromInMemoryTmjString | 6.369 μs |  1.44 | 0.7019 | 0.0153 |   9.01 KB |        1.61 |
|             |                          |          |       |        |        |           |             |
| DotTiled    | MapFromInMemoryTmxString | 3.125 μs |  1.00 | 1.2817 | 0.0610 |  16.36 KB |        1.00 |
| TiledLib    | MapFromInMemoryTmxString | 5.709 μs |  1.83 | 1.8005 | 0.0916 |  23.32 KB |        1.43 |
| TiledCSPlus | MapFromInMemoryTmxString | 6.757 μs |  2.16 | 2.5940 | 0.1831 |  33.16 KB |        2.03 |

It is important to note that the above benchmark results come from loading a very small map with a single tile layer as I had to find a common denominator between the libraries so that they all could load the same map. The results aim to be indicative of the performance of the libraries, but should be taken with a grain of salt. Only the actively maintained libraries are included in the benchmark results. TiledCSPlus does not support the `.tmj` format, so it was not included for that benchmark category.

</details>

# Feature coverage comparison

Below is a comparison of the feature coverage of DotTiled and other similar libraries. This comparison is based on the features provided by the Tiled map editor and the support for those features in each library. The comparison is not exhaustive, and you may want to refer to the respective library's documentation or implementation for details. Due to some libraries not having obvious documentation or feature lists, some features may be incorrectly marked as not supported. If you find any inaccuracies, please let me know.

| **Comparison**|**DotTiled**|[TiledLib](https://github.com/Ragath/TiledLib.Net)|[TiledCSPlus](https://github.com/nolemretaWxd/TiledCSPlus)|[TiledSharp](https://github.com/marshallward/TiledSharp)|[TiledCS](https://github.com/TheBoneJarmer/TiledCS)|[TiledNet](https://github.com/napen123/Tiled.Net)|
|---------------------------------|:-:|:-:|:-:|:-:|:-:|:-:|
| XML format `.tmx`               |✅ |⚠️|⚠️|⚠️|⚠️|⚠️|
| JSON format `.tmj`              |✅ |⚠️|❌|❌|❌|❌|
| External tileset callback       |✅ |✅|❌|✅|❌|❌|
| Object templates                |✅ |❌|❌|❌|❌|❌|
| Custom types (properties)       |✅ |❌|❌|❌|❌|❌|
| Hierarchical layers (groups)    |✅ |❌|❌|✅|❌|✅|
| Infinite maps                   |✅ |❌|✅|✅|✅|❌|
| Wangsets                        |✅ |❌|⚠️|⚠️|❌|⚠️|

> [!NOTE]
> ✅ Full support. ⚠️ Partial support, see respective library for details about supported features. ❌ No support.

# Quick Start

DotTiled is available as a NuGet package. You can install it by using the NuGet Package Manager UI in Visual Studio, or equivalent, or using the following command for the .NET CLI:

```pwsh
dotnet add package DotTiled
```

Then head to the detailed [documentation](https://dcronqvist.github.io/DotTiled/docs/quickstart.html) for more information on how to use DotTiled in your project.
