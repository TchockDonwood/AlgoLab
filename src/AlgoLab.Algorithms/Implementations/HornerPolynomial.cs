using AlgoLab.Application.Common.Interfaces;

namespace AlgoLab.Algorithms.Implementations
{
    public sealed class HornerPolynomial : IAlgorithm<double[]>
    {
        public string Code => "horner-polynomial";

        public void Execute(double[] input)
        {
            double result = input[input.Length - 1];
            double x = 1.5;

            for (int i = input.Length - 2; i >= 0; i--)
            {
                result = input[i] + x * result;
            }
        }
    }
}
