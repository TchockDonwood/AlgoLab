namespace AlgoLab.Application.Common.Models
{
    public sealed record GenerationRequest(int N, int? M, int Seed)
    {
        /// <summary>Одномерная задача: только N, без M.</summary>
        public static GenerationRequest Single(int n, int seed) => new(n, null, seed);

        /// <summary>Двумерная задача: N и M явно заданы (например, матрица n × m).</summary>
        public static GenerationRequest Pair(int n, int m, int seed) => new(n, m, seed);
    }
}
