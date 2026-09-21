namespace AlgoLab.Application.DTOs
{
    public record BenchmarkSessionDto(
        Guid Id,
        Guid AlgorithmId,
        string AlgorithmName,
        int StartN,
        int EndN,
        int Step,
        string Status,
        DateTime CreatedAt,
        DateTime? StartedAt,
        DateTime? FinishedAt
    );
}
