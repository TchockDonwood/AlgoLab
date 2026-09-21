using AlgoLab.Application.Common.Interfaces;

namespace AlgoLab.Infrastructure.Benchmarking;

public class DataGeneratorRegistry : IDataGeneratorRegistry
{
    private readonly IReadOnlyDictionary<Type, IDataGenerator> _byType;

    public DataGeneratorRegistry(IEnumerable<IDataGenerator> generators)
    {
        _byType = generators.ToDictionary(g => g.DataType);
    }

    public IDataGenerator GetFor(Type dataType)
    {
        if (_byType.TryGetValue(dataType, out var generator))
            return generator;

        throw new InvalidOperationException(
            $"No data generator registered for input type '{dataType.FullName}'. " +
            $"Register an IDataGenerator<{dataType.Name}> in the DI container.");
    }
}