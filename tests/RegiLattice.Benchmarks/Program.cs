// tests/RegiLattice.Benchmarks/Program.cs
// Entry point for BenchmarkDotNet micro-benchmarks.
// Run with: dotnet run --project tests/RegiLattice.Benchmarks -c Release

using BenchmarkDotNet.Running;

BenchmarkSwitcher.FromAssembly(typeof(Program).Assembly).Run(args);
