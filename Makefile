test:
	dotnet test

.PHONY: benchmark
benchmark: DotTiled.Benchmark/BenchmarkDotNet.Artifacts/results/MyBenchmarks.MapLoading-report-github.md

BENCHMARK_SOURCES = DotTiled.Benchmark/Program.cs
DotTiled.Benchmark/BenchmarkDotNet.Artifacts/results/MyBenchmarks.MapLoading-report-github.md: $(BENCHMARK_SOURCES)
	dotnet run --project DotTiled.Benchmark/DotTiled.Benchmark.csproj -c Release