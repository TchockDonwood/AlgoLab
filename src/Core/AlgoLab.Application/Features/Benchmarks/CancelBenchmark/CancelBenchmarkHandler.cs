using AlgoLab.Application.Common.Interfaces;
using AlgoLab.Domain.Enums;
using Microsoft.EntityFrameworkCore;

namespace AlgoLab.Application.Features.Benchmarks.CancelBenchmark
{
    public class CancelBenchmarkHandler
    {
        private readonly IApplicationDbContext _db;
        private readonly IBenchmarkCancellationManager _cancellationManager;

        public CancelBenchmarkHandler(
            IApplicationDbContext db,
            IBenchmarkCancellationManager cancellationManager)
        {
            _db = db;
            _cancellationManager = cancellationManager;
        }

        public async Task HandleAsync(
            Guid sessionId,
            CancellationToken cancellationToken)
        {
            var session = await _db.BenchmarkSessions
                .FirstOrDefaultAsync(s => s.Id == sessionId, cancellationToken);

            if (session is null)
                throw new KeyNotFoundException($"Benchmark session {sessionId} not found.");

            if (session.Status is SessionStatus.Completed
                or SessionStatus.Cancelled
                or SessionStatus.Failed)
            {
                return;
            }

            session.Status = SessionStatus.Cancelled;
            session.FinishedAt = DateTime.UtcNow;
            await _db.SaveChangesAsync(cancellationToken);

            _cancellationManager.Cancel(sessionId);
        }
    }
}