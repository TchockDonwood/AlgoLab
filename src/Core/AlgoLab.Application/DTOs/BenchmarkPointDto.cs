namespace AlgoLab.Application.DTOs
{
    public record BenchmarkPointDto(
        int N,
        int? M,
        double? ExecutionTimeMs,
        long? Steps,
        bool FromCache,
        bool IsOutlier
    );
}
