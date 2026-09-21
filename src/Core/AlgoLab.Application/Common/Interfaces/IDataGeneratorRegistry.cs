namespace AlgoLab.Application.Common.Interfaces
{
    public interface IDataGeneratorRegistry
    {
        IDataGenerator GetFor(Type dataType);
    }
}
