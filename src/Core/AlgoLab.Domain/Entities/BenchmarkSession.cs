using AlgoLab.Domain.Enums;

namespace AlgoLab.Domain.Entities
{
    public class BenchmarkSession
    {
        public Guid Id { get; set; }
        public Guid AlgorithmId { get; set; }
        public Algorithm Algorithm { get; set; } = null!;
        public int StartN { get; set; }
        public int EndN { get; set; }
        public int? StartM { get; set; }
        public int? EndM { get; set; }
        public int Step { get; set; }
        public SessionStatus Status { get; set; }
        public bool ForceRecalculate { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? StartedAt { get; set; }
        public DateTime? FinishedAt { get; set; }

        public string? ErrorMessage { get; set; }
        public ICollection<SessionRun> Runs { get; set; } 
            = new List<SessionRun>();
    }
}
