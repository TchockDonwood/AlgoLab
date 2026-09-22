using AlgoLab.Application.Common.Interfaces;
using AlgoLab.Application.Common.Models;
using System.Diagnostics;

namespace AlgoLab.Infrastructure.Benchmarking;

public class BenchmarkRunner : IBenchmarkRunner
{
    private const int MAX_RUNS = 5;
    private readonly IDataGeneratorRegistry _generators;

    public BenchmarkRunner(IDataGeneratorRegistry generators)
        => _generators = generators;

    public Task<BenchmarkResult> MeasureAsync(
        IAlgorithm algorithm,
        GenerationRequest request,
        CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();

        var generator = _generators.GetFor(algorithm.InputType);
        var seed = Random.Shared.Next();

        // Warm-up
        for (var i = 0; i < MAX_RUNS; i++)
        {
            algorithm.Execute(Generate(generator, request));
            cancellationToken.ThrowIfCancellationRequested();
        }

        var data = Generate(generator, request);

        BenchmarkResult result;

        if (algorithm is IStepAlgorithm stepAlgorithm)
        {
            result = BenchmarkResult.FromSteps(stepAlgorithm.ExecuteCountingSteps(data));
        }
        else
        {
            var stopwatch = Stopwatch.StartNew();
            algorithm.Execute(data);
            stopwatch.Stop();
            result = BenchmarkResult.FromTime(stopwatch.Elapsed);
        }

        return Task.FromResult(result);
    }

    private static object Generate(IDataGenerator generator, GenerationRequest request)
        => generator switch
        {
            IDoubleArgGenerator paired => paired.GenerateObject(
                request.N,
                request.M ?? throw new InvalidOperationException(
                    $"Algorithm requires a two-dimensional input, but request.M is null for '{generator.DataType.Name}'."),
                request.Seed),

            ISingleArgGenerator single => single.GenerateObject(request.N, request.Seed),

            _ => throw new InvalidOperationException(
                $"Generator '{generator.GetType().Name}' does not support any known generation signature.")
        };
}