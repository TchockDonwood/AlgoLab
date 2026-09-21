using AlgoLab.Application.Common.Interfaces;
using AlgoLab.Infrastructure.Benchmarking;
using AlgoLab.Infrastructure.Benchmarking.Generators;
using AlgoLab.Algorithms;
using AlgoLab.Algorithms.Implementations;
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
            services.AddScoped<IBenchmarkExecutor, BenchmarkExecutor>();
            services.AddHostedService<BenchmarkWorker>();

            return services;
        }

        public static IServiceCollection AddAlgorithms(this IServiceCollection services)
        {
            services.AddSingleton<IAlgorithm, BubbleSort>();
            //services.AddSingleton<IAlgorithm, QuickSort>();
            //services.AddSingleton<IAlgorithm, MergeSort>();
            //services.AddSingleton<IAlgorithm, TimSort>();
            //services.AddSingleton<IAlgorithm, ShakerSort>();
            

            services.AddSingleton<IAlgorithmRegistry, AlgorithmRegistry>();

            return services;
        }
    }
}
