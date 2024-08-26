test:
	dotnet build src/DotTiled.sln
	dotnet test src/DotTiled.sln

docs-serve:
	docfx docs/docfx.json --serve

docs-build:
	cp README.md docs/index.md
	docfx docs/docfx.json

lint:
	dotnet format style --verify-no-changes src/DotTiled.sln
	dotnet format analyzers --verify-no-changes src/DotTiled.sln

BENCHMARK_SOURCES = src/DotTiled.Benchmark/Program.cs src/DotTiled.Benchmark/DotTiled.Benchmark.csproj
BENCHMARK_OUTPUTDIR = src/DotTiled.Benchmark/BenchmarkDotNet.Artifacts
.PHONY: benchmark
benchmark: $(BENCHMARK_OUTPUTDIR)/results/MyBenchmarks.MapLoading-report-github.md

$(BENCHMARK_OUTPUTDIR)/results/MyBenchmarks.MapLoading-report-github.md: $(BENCHMARK_SOURCES)
	dotnet run --project src/DotTiled.Benchmark/DotTiled.Benchmark.csproj -c Release -- $(BENCHMARK_OUTPUTDIR)