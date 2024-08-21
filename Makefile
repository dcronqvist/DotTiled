test:
	dotnet build src/DotTiled.sln
	dotnet test src/DotTiled.sln

docs-serve:
	docfx docs/docfx.json --serve

docs-build:
	cp README.md docs/index.md
	docfx docs/docfx.json

BENCHMARK_SOURCES = DotTiled.Benchmark/Program.cs DotTiled.Benchmark/DotTiled.Benchmark.csproj
BENCHMARK_OUTPUTDIR = DotTiled.Benchmark/BenchmarkDotNet.Artifacts
.PHONY: benchmark
benchmark: $(BENCHMARK_OUTPUTDIR)/results/MyBenchmarks.MapLoading-report-github.md

$(BENCHMARK_OUTPUTDIR)/results/MyBenchmarks.MapLoading-report-github.md: $(BENCHMARK_SOURCES)
	dotnet run --project DotTiled.Benchmark/DotTiled.Benchmark.csproj -c Release -- $(BENCHMARK_OUTPUTDIR)