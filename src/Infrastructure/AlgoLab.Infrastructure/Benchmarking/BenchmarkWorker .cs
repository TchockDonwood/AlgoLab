using AlgoLab.Application.Common.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace AlgoLab.Infrastructure.Benchmarking
{
    public class BenchmarkWorker : BackgroundService
    {
        private readonly IServiceScopeFactory _scopeFactory;
        private readonly IBenchmarkQueue _queue;

        public BenchmarkWorker(
            IServiceScopeFactory scopeFactory,
            IBenchmarkQueue queue)
        {
            _scopeFactory = scopeFactory;
            _queue = queue;
        }

        protected override async Task ExecuteAsync(
            CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                var sessionId =
                    await _queue.DequeueAsync(stoppingToken);

                using var scope =
                    _scopeFactory.CreateScope();

                var executor =
                    scope.ServiceProvider
                        .GetRequiredService<IBenchmarkExecutor>();

                await executor.ExecuteAsync(
                    sessionId,
                    stoppingToken);
            }
        }
    }
}
