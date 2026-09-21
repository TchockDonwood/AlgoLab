using AlgoLab.Application.Common.Interfaces;
using AlgoLab.Domain.Models;

namespace AlgoLab.Algorithms.Implementations
{
    public sealed class MultiplyMatrix : IAlgorithm<MatrixPair>
    {
        public string Code => "multiply-matrix";

        public void Execute(MatrixPair input)
        {
            var first = input.Left;
            var second = input.Right;

            var n = first.Rows;
            var m = first.Cols;
            var b = second.Cols;

            var result = new Matrix(n, b);

            for (int i = 0; i < n; i++)
            {
                for (int j = 0; j < b; j++)
                {
                    var sum = 0;

                    for (int k = 0; k < m; k++)
                    {
                        sum += first[i, k] * second[k, j];
                    }

                    result[i, j] = sum;
                }
            }
        }
    }
}
