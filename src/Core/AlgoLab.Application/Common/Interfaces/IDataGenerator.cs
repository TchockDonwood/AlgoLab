namespace AlgoLab.Application.Common.Interfaces
{
    public interface IDataGenerator
    {
        Type DataType { get; }
    }

    public interface ISingleArgGenerator : IDataGenerator
    {
        object GenerateObject(int n, int seed);
    }

    public interface ISingleArgGenerator<TData> : ISingleArgGenerator
    {
        TData Generate(int n, int seed);
        Type IDataGenerator.DataType => typeof(TData);
        object ISingleArgGenerator.GenerateObject(int n, int seed)
            => Generate(n, seed)!;
    }

    public interface IDoubleArgGenerator : IDataGenerator
    {
        object GenerateObject(int n, int m, int seed);
    }

    public interface IDoubleArgGenerator<TData> : IDoubleArgGenerator
    {
        TData Generate(int n, int m, int seed);
        Type IDataGenerator.DataType => typeof(TData);
        object IDoubleArgGenerator.GenerateObject(int n, int m, int seed)
            => Generate(n, m, seed)!;
    }
}
