using AlgoLab.Application.Common.Interfaces;
using AlgoLab.Domain.Models;

namespace AlgoLab.Algorithms.Implementations
{
    public sealed class QuickRecursivePow : IStepAlgorithm<PowInput>
    {
        public string Code => "quick-recursive-pow";

        public long ExecuteCountingSteps(PowInput input)
        {
            var number = input.number;
            var exp = input.exp;

            long steps = 0;

            Pow(number, exp, ref steps);
            
            return steps;
        }

        private static long Pow(long number, long exp, ref long steps)
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

            steps++; // вызов RecursivePow
            long result = Pow(number, exp / 2, ref steps);

            steps++; // if (exp % 2 != 0)
            if (exp % 2 != 0)
            {
                steps++; // умножение result * result
                steps++; // умножение на number
                return result * result * number;
            }
            else
            {
                steps++; // умножение result * result
                return result * result;
            }
        }
    }
}
