namespace AlgoLab.Application.DTOs
{
    public record BenchmarkDetailsDto(
        Guid SessionId,
        string AlgorithmName,
        int InputArity,
        string Status,
        string? ApproximationModel,
        IReadOnlyCollection<double>? ApproximationPoints,
        IReadOnlyCollection<BenchmarkPointDto> Points
    );
}
