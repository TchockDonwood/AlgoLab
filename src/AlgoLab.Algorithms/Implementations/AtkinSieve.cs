using AlgoLab.Application.Common.Interfaces;

namespace AlgoLab.Algorithms.Implementations
{
    public sealed class AtkinSieve : IAlgorithm<long>
    {
        public string Code => "atkin-sieve";
        public string Name => "Atkin Sieve";
        public void Execute(long limit)
        {
            var primes = new List<long>();

            if (limit < 2) return;

            // Базовые простые числа вносим вручную
            if (limit >= 2) primes.Add(2);
            if (limit >= 3) primes.Add(3);

            // Сито для маркировки чисел
            bool[] isPrime = new bool[limit + 1];
            long limitSqrt = (long)Math.Sqrt(limit);

            // ЭТАП 1: Анализ остатков деления квадратичных форм
            for (long x = 1; x <= limitSqrt; x++)
            {
                for (long y = 1; y <= limitSqrt; y++)
                {
                    long x2 = x * x;
                    long y2 = y * y;

                    // 1. Форма: n = 4x^2 + y^2
                    long n = (4 * x2) + y2;
                    if (n <= limit && (n % 12 == 1 || n % 12 == 5))
                    {
                        isPrime[n] ^= true;
                    }

                    // 2. Форма: n = 3x^2 + y^2
                    n = (3 * x2) + y2;
                    if (n <= limit && n % 12 == 7)
                    {
                        isPrime[n] ^= true;
                    }

                    // 3. Форма: n = 3x^2 - y^2 (при x > y)
                    if (x > y)
                    {
                        n = (3 * x2) - y2;
                        if (n <= limit && n % 12 == 11)
                        {
                            isPrime[n] ^= true;
                        }
                    }
                }
            }

            // ЭТАП 2: Исключение квадратов всех найденных простых чисел
            for (long n = 5; n <= limitSqrt; n++)
            {
                if (isPrime[n])
                {
                    long n2 = n * n;
                    for (long k = n2; k <= limit; k += n2)
                    {
                        isPrime[k] = false;
                    }
                }
            }

            // ЭТАП 3: Сборка итогового списка
            for (long n = 5; n <= limit; n++)
            {
                if (isPrime[n])
                {
                    primes.Add(n);
                }
            }
        }
    }
}