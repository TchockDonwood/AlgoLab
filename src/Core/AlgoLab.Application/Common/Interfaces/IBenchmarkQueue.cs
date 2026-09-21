namespace AlgoLab.Application.Common.Interfaces
{
    public interface IBenchmarkQueue
    {
        ValueTask EnqueueAsync(
            Guid sessionId,
            CancellationToken cancellationToken);

        ValueTask<Guid> DequeueAsync(
            CancellationToken cancellationToken);
    }
}
