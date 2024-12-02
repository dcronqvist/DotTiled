# 📚 DotTiled

DotTiled is a simple and easy-to-use library for loading [Tiled maps and tilesets](https://mapeditor.org) in your .NET projects. After [TiledCS](https://github.com/TheBoneJarmer/TiledCS) unfortunately became unmaintained (since 2022), I aimed to create a new library that could fill its shoes. DotTiled is the result of that effort.

DotTiled is designed to be a lightweight and efficient library that provides a simple API for loading and managing Tiled maps and tilesets. It is built with performance in mind and aims to be as fast and memory-efficient as possible.

- [Alternative libraries and comparison + benchmarks](#alternative-libraries-and-comparison)
- [Feature coverage comparison](#feature-coverage-comparison)
- [Quick Start](#quick-start)

# Alternative libraries and comparison

Other similar libraries exist, and you may want to consider them for your project as well:

|**Comparison**|**DotTiled**|[TiledLib](https://github.com/Ragath/TiledLib.Net)|[TiledCSPlus](https://github.com/nolemretaWxd/TiledCSPlus)|[TiledSharp](https://github.com/marshallward/TiledSharp)|[TiledCS](https://github.com/TheBoneJarmer/TiledCS)|[TiledNet](https://github.com/napen123/Tiled.Net)|
|---------------------------------|:-----------------------:|:--------:|:-----------:|:----------:|:-------:|:------:|
| Actively maintained             |            ✅          |     ✅   |     ❌      |      ❌   |    ❌  |   ❌   |
| Benchmark (time)*               |           1.00          |   1.78   |     2.11    |      -     |    -    |    -   |
| Benchmark (memory)*             |           1.00          |   1.32   |     1.88    |      -     |    -    |    -   |
| .NET Targets                    | `net8.0` | `net8.0` |`netstandard2.1`|`netstandard2.0`|`netstandard2.0`|`net45`|
| Docs                            |Usage, API,<br>XML Docs|Usage|Usage, API,<br>XML Docs|Usage, API|Usage, XML Docs|Usage, XML Docs|
| License                         |           MIT           |   MIT    |     MIT     | Apache-2.0 |   MIT   | BSD 3-Clause |

> *Both benchmark time and memory ratios are relative to DotTiled. Lower is better. Benchmark (time) refers to the execution time of loading the same map from an in-memory string that contains XML data in the `.tmx` format. Benchmark (memory) refers to the memory allocated during that loading process.

[MonoGame](https://www.monogame.net) users may also want to consider using [MonoGame.Extended](https://github.com/craftworkgames/MonoGame.Extended) for loading Tiled maps and tilesets. The feature coverage by MonoGame.Extended is less than that of DotTiled, so you may want to consider using DotTiled if you need access to more Tiled features and flexibility.

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

> ✅ Full support. ⚠️ Partial support, see respective library for details about supported features. ❌ No support.

# Quick Start

Check out the detailed [documentation](https://dcronqvist.github.io/DotTiled/docs/quickstart.html) for more information on how to use DotTiled in your project.
