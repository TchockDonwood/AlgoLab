using AlgoLab.Algorithms;
using AlgoLab.Algorithms.Implementations;
using AlgoLab.Application.Common.Interfaces;
using AlgoLab.Application.Features.Algorithms.GetAlgorithms;
using AlgoLab.Application.Features.Benchmarks.CancelBenchmark;
using AlgoLab.Application.Features.Benchmarks.GetComparison;
using AlgoLab.Application.Features.Benchmarks.GetHistory;
using AlgoLab.Application.Features.Benchmarks.GetSessionDetails;
using AlgoLab.Application.Features.Benchmarks.StartBenchmark;
using AlgoLab.Infrastructure.Benchmarking;
using AlgoLab.Infrastructure.Benchmarking.Generators;
using Microsoft.Extensions.DependencyInjection;

namespace AlgoLab.Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddBenchmarking(this IServiceCollection services)
        {
            // Генераторы — по одному на каждый тип входа
            services.AddSingleton<IDataGenerator, IntArrayGenerator>();
            services.AddSingleton<IDataGenerator, DoubleArrayGenerator>();
            services.AddSingleton<IDataGenerator, MatrixPairGenerator>();

            services.AddSingleton<IDataGeneratorRegistry, DataGeneratorRegistry>();
            services.AddSingleton<IBenchmarkRunner, BenchmarkRunner>();

            services.AddSingleton<IBenchmarkQueue, BenchmarkQueue>();
            services.AddScoped<IBenchmarkResultStore, BenchmarkResultStore>();
            services.AddScoped<IBenchmarkExecutor, BenchmarkExecutor>();
            services.AddHostedService<BenchmarkWorker>();

            return services;
        }

        public static IServiceCollection AddAlgorithms(this IServiceCollection services)
        {
            services.AddSingleton<IAlgorithm, ConstFunction>();
            services.AddSingleton<IAlgorithm, SumFunction>();
            services.AddSingleton<IAlgorithm, ProductFunction>();
            services.AddSingleton<IAlgorithm, NaivePolynomial>();
            services.AddSingleton<IAlgorithm, HornerPolynomial>();
            services.AddSingleton<IAlgorithm, BubbleSort>();
            services.AddSingleton<IAlgorithm, QuickSort>();
            services.AddSingleton<IAlgorithm, TimSort>();
            services.AddSingleton<IAlgorithm, MultiplyMatrix>();
            services.AddSingleton<IAlgorithm, SmoothSort>();
            services.AddSingleton<IAlgorithm, SieveOfEratosthenes>();
            //services.AddSingleton<IAlgorithm, >();
            services.AddSingleton<IAlgorithm, SimplePow>();
            services.AddSingleton<IAlgorithm, RecursivePow>();
            services.AddSingleton<IAlgorithm, QuickRecursivePow>();

            services.AddSingleton<IAlgorithmRegistry, AlgorithmRegistry>();

            return services;
        }

        public static IServiceCollection AddApplication(this IServiceCollection services)
        {
            services.AddScoped<StartBenchmarkHandler>();
            services.AddScoped<CancelBenchmarkHandler>();
            services.AddScoped<GetHistoryHandler>();
            services.AddScoped<GetSessionDetailsHandler>();
            services.AddScoped<GetComparisonHandler>();
            services.AddScoped<GetAlgorithmsHandler>();

            return services;
        }
    }
}
