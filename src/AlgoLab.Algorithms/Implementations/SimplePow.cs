using AlgoLab.Application.Common.Interfaces;
using AlgoLab.Domain.Models;

namespace AlgoLab.Algorithms.Implementations
{
    public sealed class SimplePow : IStepAlgorithm<PowInput>
    {
        public string Code => "simple-pow";
        public string Name => "Simple Pow";
        public long ExecuteCountingSteps(PowInput input)
        {
            var number = input.number;
            var exp = input.exp;

            long steps = 0;

            steps++; // if (exp < 0)
            if (exp < 0)
                throw new ArgumentException("Степень должна быть неотрицательной!", nameof(exp));

            long result = 1;
            steps++; // присваивание result

            steps++; // инициализация i = 0
            for (int i = 0; i < exp; i++)
            {
                steps++; // условие i < exp
                result *= number;
                steps++; // умножение
                steps++; // инкремент i++
            }
            steps++; // последняя проверка условия (false)
            
            steps++; // return
            return steps;
        }
    }
}
