using AlgoLab.Application.Common.Interfaces;

namespace AlgoLab.Algorithms.Implementations
{
    public sealed class NaivePolynomial : IAlgorithm<double[]>
    {
        public string Code => "naive-polynomial";

        public void Execute(double[] input)
        {
            double result = 0;
            double x = 1.5;

            for (int i = 0; i < input.Length; i++)
            {
                result += input[i] * Math.Pow(x, i);
            }
        }
    }
}
