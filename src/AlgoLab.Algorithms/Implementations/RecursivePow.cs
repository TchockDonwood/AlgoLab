using AlgoLab.Application.Common.Interfaces;
using AlgoLab.Domain.Models;

namespace AlgoLab.Algorithms.Implementations
{
    public sealed class RecursivePow : IStepAlgorithm<PowInput>
    {
        public string Code => "recursive-pow";
        public string Name => "Recursive Pow";

        public long ExecuteCountingSteps(PowInput input)
        {
            var number = input.number;
            var exp = input.exp;

            long steps = 0;

            Pow(number, exp, ref steps);

            return steps;
        }

        private static long Pow(long number, int exp, ref long steps)
        {
            steps++; // if (exp < 0)
            if (exp < 0)
                throw new ArgumentException("Степень должна быть неотрицательной!", nameof(exp));

            steps++; // if (exp == 0)
            if (exp == 0)
            {
                steps++; // return 1
                return 1;
            }

            steps++; // exp - 1
            steps++; // вызов Pow
            steps++; // number * Pow
            steps++; // return
            return number * Pow(number, exp - 1, ref steps);
        }
    }
}
