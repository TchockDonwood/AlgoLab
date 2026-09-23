using AlgoLab.Application.Common.Interfaces;
using AlgoLab.Application.Common.Models;
using System.Diagnostics;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace AlgoLab.Infrastructure.Benchmarking;

public class BenchmarkRunner : IBenchmarkRunner
{
    private const int REPETITIONS = 5;
    private readonly IDataGeneratorRegistry _generators;
    private readonly IBenchmarkStatisticsService _statistics;

    public BenchmarkRunner(
        IDataGeneratorRegistry generators,
        IBenchmarkStatisticsService statistics)
    {
        _generators = generators;
        _statistics = statistics;
    }

    public Task<BenchmarkResult> MeasureAsync(
    IAlgorithm algorithm,
    GenerationRequest request,
    CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();
        var generator = _generators.GetFor(algorithm.InputType);

        // Warm-up
        for (var i = 0; i < 3; i++)
        {
            algorithm.Execute(Generate(generator, request));
            cancellationToken.ThrowIfCancellationRequested();
        }

        var data = Generate(generator, request);
        BenchmarkResult result;

        if (algorithm is IStepAlgorithm stepAlgorithm)
        {
            var steps = stepAlgorithm.ExecuteCountingSteps(data);
            result = new BenchmarkResult(0.0, steps);
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