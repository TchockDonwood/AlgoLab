using AlgoLab.Application.Common.Interfaces;

namespace AlgoLab.Algorithms.Implementations
{
    public sealed class ProductFunction : IAlgorithm<int[]>
    {
        public string Code => "product-function";
        public string Name => "Product Function";
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
