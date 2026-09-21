using AlgoLab.Application.Common.Interfaces;

namespace AlgoLab.Algorithms.Implementations
{
    public sealed class SumFunction : IAlgorithm<int[]>
    {
        public string Code => "sum-function";

        public void Execute(int[] input)
        {
            var sum = 0;
            for (int i = 0; i < input.Length; i++)
            {
                sum += input[i];
            }
        }
    }
}