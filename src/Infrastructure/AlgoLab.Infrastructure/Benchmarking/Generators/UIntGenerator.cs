using AlgoLab.Application.Common.Interfaces;

namespace AlgoLab.Infrastructure.Benchmarking.Generators
{
    public class UIntGenerator : ISingleArgGenerator<uint>
    {
        public uint Generate(int n, int seed)
        {
            return (uint)n;
        }
    }
}