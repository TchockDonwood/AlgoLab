using AlgoLab.Application.Common.Interfaces;

namespace AlgoLab.Infrastructure.Benchmarking.Generators
{
    public class IntArrayGenerator : ISingleArgGenerator<int[]>
    {
        public int[] Generate(int n, int seed)
        {
            var random = new Random(seed);
            var data = new int[n];
            for (int i = 0; i < n; i++) data[i] = random.Next();
            return data;
        }
    }
}
