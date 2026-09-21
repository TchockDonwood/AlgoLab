using AlgoLab.Application.Common.Interfaces;

namespace AlgoLab.Algorithms
{
    public class AlgorithmRegistry : IAlgorithmRegistry
    {
        private readonly Dictionary<string, IAlgorithm> _algorithms;

        public AlgorithmRegistry(IEnumerable<IAlgorithm> algorithms)
        {
            _algorithms = algorithms.ToDictionary(
                x => x.Code,
                StringComparer.OrdinalIgnoreCase);
        }

        public IAlgorithm Get(string code)
        {
            if (!_algorithms.TryGetValue(code, out var algoritm))
            {
                throw new KeyNotFoundException($"Algorithm '{code}' not found.");
            }

            return algoritm;
        }

        public IReadOnlyCollection<IAlgorithm> GetAll()
        {
            return _algorithms.Values.ToList();
        }
    }
}
