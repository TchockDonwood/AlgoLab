using AlgoLab.Application.Common.Interfaces;
using AlgoLab.Domain.Models;

namespace AlgoLab.Infrastructure.Benchmarking.Generators
{
    public class PowInputGenerator : ISingleArgGenerator<PowInput>
    {
        public PowInput Generate(int n, int seed)
        {
            return new PowInput(number: 2, exp: n);
        }
    }
}