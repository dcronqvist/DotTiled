test:
	dotnet build src/DotTiled.sln
	dotnet test src/DotTiled.sln
	dotnet run --project src/DotTiled.Examples/DotTiled.Example.Console/DotTiled.Example.Console.csproj -- src/DotTiled.Examples/DotTiled.Example.Console

docs-serve: docs/index.md
	docfx docs/docfx.json --serve

docs-build: docs/index.md
	docfx docs/docfx.json

docs/index.md: 
	cp README.md docs/index.md

lint:
	dotnet format style --verify-no-changes src/DotTiled.sln
	dotnet format analyzers --verify-no-changes src/DotTiled.sln

pack: 
	dotnet pack src/DotTiled/DotTiled.csproj -c Release -o ./nupkg -p:IncludeSymbols=true -p:SymbolPackageFormat=snupkg

BENCHMARK_SOURCES = src/DotTiled.Benchmark/Program.cs src/DotTiled.Benchmark/DotTiled.Benchmark.csproj
BENCHMARK_OUTPUTDIR = src/DotTiled.Benchmark/BenchmarkDotNet.Artifacts
.PHONY: benchmark
benchmark: $(BENCHMARK_OUTPUTDIR)/results/MyBenchmarks.MapLoading-report-github.md
						
$(BENCHMARK_OUTPUTDIR)/results/MyBenchmarks.MapLoading-report-github.md: $(BENCHMARK_SOURCES)
	dotnet run --project src/DotTiled.Benchmark/DotTiled.Benchmark.csproj -c Release -- $(BENCHMARK_OUTPUTDIR)