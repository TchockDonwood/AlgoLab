namespace AlgoLab.Application.DTOs
{
    public record BenchmarkDetailsDto(
        Guid SessionId,
        string AlgorithmName,
        string Status,
        IReadOnlyCollection<BenchmarkPointDto> Points
    );
}
