using AlgoLab.Application.Common.Interfaces;

namespace AlgoLab.Algorithms.Implementations
{
    public sealed class BubbleSort : IAlgorithm
    {
        public string Code => "bubble-sort";

        public void Execute(int[] input)
        {
            for (int i = 0; i < input.Length - 1; i++)
            {
                for (int j = 0; j < input.Length - i - 1; j++)
                {
                    if (input[j] > input[j+1])
                    {
                        (input[j], input[j+1]) = (input[j+1], input[j]);
                    }
                }
            }
        }
    }
}
