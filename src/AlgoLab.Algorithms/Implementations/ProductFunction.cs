using AlgoLab.Application.Common.Interfaces;

namespace AlgoLab.Algorithms.Implementations
{
    public sealed class ProductFunction : IAlgorithm
    {
        public string Code => "product-function";

        public void Execute(int[] input)
        {
            var product = 1;
            for (int i = 0; i < input.Length; i++)
            {
                product *= input[i];
            }
        }
    }
}
