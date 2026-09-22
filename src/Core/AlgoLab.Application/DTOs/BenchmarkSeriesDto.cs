namespace AlgoLab.Application.DTOs
{
    public record BenchmarkSeriesDto(
        Guid SessionId,
        Guid AlgorithmId,
        string AlgorithmName,
        IReadOnlyList<BenchmarkPointDto> Points
    );
}