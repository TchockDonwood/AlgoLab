using System.Collections.Concurrent;
using AlgoLab.Application.Common.Interfaces;

namespace AlgoLab.Infrastructure.Benchmarking;

public class BenchmarkCancellationManager : IBenchmarkCancellationManager
{
    private readonly ConcurrentDictionary<Guid, CancellationTokenSource> _tokens = new();
    public CancellationToken GetToken(Guid sessionId) => _tokens.GetOrAdd(sessionId, _ => new CancellationTokenSource()).Token;
    public void Cancel(Guid sessionId) { if (_tokens.TryGetValue(sessionId, out var cts)) cts.Cancel(); }
    public void Complete(Guid sessionId) { if (_tokens.TryRemove(sessionId, out var cts)) cts.Dispose(); }
}