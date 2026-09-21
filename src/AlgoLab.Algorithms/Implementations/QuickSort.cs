
using AlgoLab.Application.Common.Interfaces;

namespace AlgoLab.Algorithms.Implementations
{
    public sealed class QuickSort : IAlgorithm<double[]>
    {
        public string Code => "quick-sort";

        public void Execute(double[] input)
        {
            var start = 0;
            var end = input.Length;
            Sort(input, start, end);
        }

        private static double[] Sort(double[] input, int start, int end)
        {
            if (end <= start)
            {
                return input;
            }

            else
            {
                double pivot = input[start];
                int pivotIndex = start;


                for (int i = start + 1; i <= end; i++)
                {
                    if (input[i] < pivot)
                    {
                        pivotIndex++;
                        (input[i], input[pivotIndex]) = (input[pivotIndex], input[i]);
                    }
                }

                // Помещаем опорный элемент на правильную позицию
                (input[start], input[pivotIndex]) = (input[pivotIndex], input[start]);

                Sort(input, start, pivotIndex - 1);
                Sort(input, pivotIndex + 1, end);
                return input;
            }
        }
    }
}
