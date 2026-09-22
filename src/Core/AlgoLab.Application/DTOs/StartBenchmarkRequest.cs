namespace AlgoLab.Application.DTOs
{
    public record StartBenchmarkRequest(
        Guid AlgorithmId,
        int StartN,
        int EndN,
        int? StartM,
        int? EndM,
        int Step,
        bool ForceRecalculate
    );
}
