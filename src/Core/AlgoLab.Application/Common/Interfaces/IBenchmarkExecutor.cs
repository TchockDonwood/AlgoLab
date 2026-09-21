namespace AlgoLab.Application.Common.Interfaces
{
    public interface IBenchmarkExecutor
    {
        Task ExecuteAsync(
            Guid sessionId,
            CancellationToken cancellationToken);
    }
}
