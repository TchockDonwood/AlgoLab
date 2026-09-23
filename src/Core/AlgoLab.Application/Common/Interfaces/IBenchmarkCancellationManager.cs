namespace AlgoLab.Application.Common.Interfaces
{
    public interface IBenchmarkCancellationManager
    {
        CancellationToken GetToken(Guid sessionId);
        void Cancel(Guid sessionId);
        void Complete(Guid sessionId);
    }
}


