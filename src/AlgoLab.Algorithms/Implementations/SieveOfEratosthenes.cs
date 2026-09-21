using AlgoLab.Application.Common.Interfaces;

namespace AlgoLab.Algorithms.Implementations
{
    public sealed class SieveOfEratosthenes : IAlgorithm<uint>
    {
        public string Code => "sieve-of-eratosthenes";
        public string Name => "Sieve Of Eratosthenes";
        public void Execute(uint input)
        {
            var primes = new List<uint>();

            if (input < 2)
                return;

            bool[] isPrime = new bool[input + 1];

            for (uint i = 2; i <= input; i++)
            {
                isPrime[i] = true;
            }

            for (uint i = 2; i * i <= input; i++)
            {
                if (isPrime[i])
                {
                    for (uint j = i * i; j <= input; j += i)
                    {
                        isPrime[j] = false;
                    }
                }
            }

            for (uint i = 2; i <= input; i++)
            {
                if (isPrime[i])
                {
                    primes.Add(i);
                }
            }
        }
    }
}
