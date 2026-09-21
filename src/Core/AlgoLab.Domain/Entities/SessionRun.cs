namespace AlgoLab.Domain.Entities
{
    public class SessionRun
    {
        public Guid Id { get; set; }
        public Guid SessionId { get; set; }
        public BenchmarkSession Session { get; set; } = null!;
        public Guid BenchmarkRunId { get; set; }
        public BenchmarkRun BenchmarkRun { get; set; } = null!;
        public int N { get; set; }
        public double? ExecutionTimeMs { get; set; }
        public long? StepsCount { get; set; }
        public bool FromCache { get; set; }
    }
}
