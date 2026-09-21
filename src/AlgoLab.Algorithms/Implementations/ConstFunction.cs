using AlgoLab.Application.Common.Interfaces;

namespace AlgoLab.Algorithms.Implementations
{
    public sealed class ConstFunction : IAlgorithm
    {
        public string Code => "const-function";

        public void Execute(int[] input)
        {
            double result = 0.0;

            for (int i = 0; i < 250_000; i++)
            {
                result += Math.Sqrt(i % 100 + 1);
            }

            return;
        }
    }
}
