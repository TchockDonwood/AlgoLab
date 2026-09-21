using AlgoLab.Application.Common.Interfaces;
using AlgoLab.Domain.Models;

namespace AlgoLab.Infrastructure.Benchmarking.Generators
{
    public class MatrixPairGenerator : IDoubleArgGenerator<MatrixPair>
    {
        public MatrixPair Generate(int n, int m, int seed)
        {
            var random = new Random(seed);
            
            var left = new Matrix(n, m);
            var right = new Matrix(m, n);

            for (int i = 0; i < left.Rows; i++)
                for (int j = 0; j < left.Cols; j++)
                    left[i, j] = random.Next();

            for (int i = 0; i < right.Rows; i++)
                for (int j = 0; j < right.Cols; j++)
                    right[i, j] = random.Next();

            return new MatrixPair(left, right);
        }
    }
}
