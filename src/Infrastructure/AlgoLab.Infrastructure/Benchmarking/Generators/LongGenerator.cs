using AlgoLab.Application.Common.Interfaces;

namespace AlgoLab.Infrastructure.Benchmarking.Generators
{
    public class LongGenerator : ISingleArgGenerator<long>
    {
        public long Generate(int n, int seed)
        {
            return (long)n;
        }
    }
}
