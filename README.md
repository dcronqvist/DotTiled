# 📚 DotTiled

<img src="https://www.mapeditor.org/img/tiled-logo-white.png" align="right" width="20%"/>

DotTiled is a simple and easy-to-use library for loading, saving, and managing [Tiled maps and tilesets](https://mapeditor.org) in your .NET projects. After [TiledCS](https://github.com/TheBoneJarmer/TiledCS) unfortunately became unmaintained (since 2022), I aimed to create a new library that could fill its shoes. DotTiled is the result of that effort.

DotTiled is designed to be a lightweight and efficient library that provides a simple API for loading and managing Tiled maps and tilesets. It is built with performance in mind and aims to be as fast and memory-efficient as possible. Targeting `netstandard2.0` and `net8.0` allows DotTiled to be used in popular game engines like Unity and Godot, as well as in popular game development frameworks like MonoGame.

- [Alternative libraries and comparison + benchmarks](#alternative-libraries-and-comparison)
- [Quickstart](#quickstart)
  - [Installing DotTiled](#installing-dottiled)

# Alternative libraries and comparison

Other similar libraries exist, and you may want to consider them for your project as well:

|**Comparison**|**DotTiled**|[TiledLib](https://github.com/Ragath/TiledLib.Net)|[TiledCSPlus](https://github.com/nolemretaWxd/TiledCSPlus)|[TiledSharp](https://github.com/marshallward/TiledSharp)|[TiledCS](https://github.com/TheBoneJarmer/TiledCS)|[TiledNet](https://github.com/napen123/Tiled.Net)|
|---------------------------------|:-----------------------:|:--------:|:-----------:|:----------:|:-------:|:------:|
| Actively maintained             |            ✅          |     ✅   |     ✅      |      ❌   |    ❌  |   ❌   |
| Benchmark (time)*               |           1.00          |   1.81   |     2.12    |      -     |    -    |    -   |
| Benchmark (memory)*             |           1.00          |   1.42   |     2.03    |      -     |    -    |    -   |
| .NET Targets                    | `net8.0`<br>`netstandard2.0` |`net6.0`<br>`net7.0`|`netstandard2.1`|`netstandard2.0`|`netstandard2.0`|`net45`|
| Docs                            |Usage,<br>XML Docs|Usage|Usage, API,<br>XML Docs|Usage, API|Usage, XML Docs|Usage, XML Docs|
| License                         |           MIT           |   MIT    |     MIT     | Apache-2.0 |   MIT   | BSD 3-Clause |
| *Feature coverage<br>comparison below*|✅/❌|✅/❌|✅/❌|✅/❌|✅/❌|✅/❌|

> [!NOTE]
> *Both benchmark time and memory ratios are relative to DotTiled. Lower is better. Benchmark (time) refers to the execution time of loading the same map from an in-memory string that contains XML data in the `.tmx` format. Benchmark (memory) refers to the memory allocated during that loading process. For further details on the benchmark results, see the collapsible section below.

<details>
<summary>
Feature coverage comparison
</summary>

| **Comparison**|**DotTiled**|[TiledLib](https://github.com/Ragath/TiledLib.Net)|[TiledCSPlus](https://github.com/nolemretaWxd/TiledCSPlus)|[TiledSharp](https://github.com/marshallward/TiledSharp)|[TiledCS](https://github.com/TheBoneJarmer/TiledCS)|[TiledNet](https://github.com/napen123/Tiled.Net)|
|---------------------------------|:-:|:-:|:-:|:-:|:-:|:-:|
| Full XML support `.tmx`         |✅/❌|✅/❌|✅/❌|✅/❌|✅/❌|✅/❌|
| Full JSON support `.tmj`        |✅/❌|✅/❌|✅/❌|✅/❌|✅/❌|✅/❌|
| Load from string (implies file) |✅/❌|✅/❌|✅/❌|✅/❌|✅/❌|✅/❌|
| Load from file                  |✅/❌|✅/❌|✅/❌|✅/❌|✅/❌|✅/❌|
| External tilesets               |✅/❌|✅/❌|✅/❌|✅/❌|✅/❌|✅/❌|
| Template files                  |✅/❌|✅/❌|✅/❌|✅/❌|✅/❌|✅/❌|
| Property custom types           |✅/❌|✅/❌|✅/❌|✅/❌|✅/❌|✅/❌|
| Hierarchical layers (groups)    |✅/❌|✅/❌|✅/❌|✅/❌|✅/❌|✅/❌|
| Infinite maps                   |✅/❌|✅/❌|✅/❌|✅/❌|✅/❌|✅/❌|

</details>

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
| Method      | Categories               | Mean      | Error     | StdDev    | Ratio | RatioSD | Gen0   | Gen1   | Allocated | Alloc Ratio |
|------------ |------------------------- |----------:|----------:|----------:|------:|--------:|-------:|-------:|----------:|------------:|
| DotTiled    | MapFromInMemoryTmxString |  2.991 μs | 0.0266 μs | 0.0236 μs |  1.00 |    0.00 | 1.2817 | 0.0610 |  16.37 KB |        1.00 |
| TiledLib    | MapFromInMemoryTmxString |  5.405 μs | 0.0466 μs | 0.0413 μs |  1.81 |    0.02 | 1.8158 | 0.1068 |  23.32 KB |        1.42 |
| TiledCSPlus | MapFromInMemoryTmxString |  6.354 μs | 0.0703 μs | 0.0587 μs |  2.12 |    0.03 | 2.5940 | 0.1831 |  33.23 KB |        2.03 |
|             |                          |           |           |           |       |         |        |        |           |             |
| DotTiled    | MapFromTmxFile           | 28.570 μs | 0.1216 μs | 0.1137 μs |  1.00 |    0.00 | 1.0376 |      - |  13.88 KB |        1.00 |
| TiledCSPlus | MapFromTmxFile           | 33.377 μs | 0.1086 μs | 0.1016 μs |  1.17 |    0.01 | 2.8076 | 0.1221 |  36.93 KB |        2.66 |
| TiledLib    | MapFromTmxFile           | 36.077 μs | 0.1900 μs | 0.1777 μs |  1.26 |    0.01 | 2.0752 | 0.1221 |   27.1 KB |        1.95 |

</details>

[MonoGame](https://www.monogame.net) users may also want to consider using [MonoGame.Extended](https://github.com/craftworkgames/MonoGame.Extended) for loading Tiled maps and tilesets. Like MonoGame.Extended, DotTiled also provides a way to properly import Tiled maps and tilesets with the MonoGame content pipeline (with the DotTiled.MonoGame.Pipeline NuGet). However, unlike MonoGame.Extended, DotTiled does *not* include any kind of rendering capabilities, and it is up to you as a developer to implement any kind of rendering for your maps when using DotTiled. The feature coverage by MonoGame.Extended is less than that of DotTiled, so you may want to consider using DotTiled if you need access to more Tiled features and flexibility.

# Quickstart

### Installing DotTiled

DotTiled is available as a NuGet package. You can install it by using the NuGet Package Manager UI in Visual Studio, or equivalent, or using the following command for the .NET CLI:

```pwsh
dotnet add package DotTiled
```
