using AlgoLab.Domain.Enums;
using AlgoLab.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace AlgoLab.Infrastructure.Benchmarking
{
    /// <summary>
    /// При старте приложения переводит «зомби-сессии» (Pending/Running, оставшиеся
    /// от предыдущего запуска) в Failed, чтобы они не висели вечно в UI.
    /// </summary>
    public class BenchmarkStartupRecoveryService : IHostedService
    {
        private readonly IServiceScopeFactory _scopeFactory;
        private readonly ILogger<BenchmarkStartupRecoveryService> _logger;

        public BenchmarkStartupRecoveryService(
            IServiceScopeFactory scopeFactory,
            ILogger<BenchmarkStartupRecoveryService> logger)
        {
            _scopeFactory = scopeFactory;
            _logger = logger;
        }

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            try
            {
                using var scope = _scopeFactory.CreateScope();
                var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();

                var zombie = await db.BenchmarkSessions
                    .Where(s => s.Status == SessionStatus.Pending
                             || s.Status == SessionStatus.Running)
                    .ToListAsync(cancellationToken);

                if (zombie.Count == 0) return;

                _logger.LogWarning(
                    "Recovering {Count} interrupted benchmark sessions.", zombie.Count);

                var now = DateTime.UtcNow;
                foreach (var s in zombie)
                {
                    s.Status = SessionStatus.Failed;
                    s.ErrorMessage = "Application restarted before session completed.";
                    s.FinishedAt = now;
                }

                await db.SaveChangesAsync(cancellationToken);
            }
            catch (Exception ex)
            {
                // Не валим приложение, если БД ещё не готова (например, миграции).
                _logger.LogError(ex, "Failed to recover benchmark sessions on startup.");
            }
        }

        public Task StopAsync(CancellationToken cancellationToken) => Task.CompletedTask;
    }
}