namespace AlgoLab.Application.DTOs
{
    public record StartBenchmarkRequest(
        Guid AlgorithmId,
        int StartN,
        int EndN,
        int Step,
        bool ForceRecalculate
    );
}
