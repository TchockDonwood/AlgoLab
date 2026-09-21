using AlgoLab.Application.Common.Interfaces;

namespace AlgoLab.Infrastructure.Benchmarking.Generators
{
    public class DoubleArrayGenerator : ISingleArgGenerator<double[]>
    {
        public double[] Generate(int n, int seed)
        {
            var random = new Random(seed);
            var data = new double[n];
            for (int i = 0; i < n; i++) data[i] = random.NextDouble();
            return data;
        }
    }
}
