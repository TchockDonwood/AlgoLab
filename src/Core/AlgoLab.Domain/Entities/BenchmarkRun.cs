namespace AlgoLab.Domain.Entities
{
    public class BenchmarkRun
    {
        public Guid Id { get; set; }
        public Guid AlgorithmId { get; set; }
        public Algorithm Algorithm { get; set; } = null!;
        public int N { get; set; }
        public double? ExecutionTimeMs { get; set; }
        public long? StepsCount { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
}
