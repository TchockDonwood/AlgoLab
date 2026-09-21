namespace AlgoLab.Application.DTOs
{
    public record BenchmarkPointDto(
        int N,
        double? ExecutionTimeMs,
        long? Steps,
        bool FromCache
    );
}
