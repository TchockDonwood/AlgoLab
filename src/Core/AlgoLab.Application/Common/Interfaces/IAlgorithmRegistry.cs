namespace AlgoLab.Application.Common.Interfaces
{
    public interface IAlgorithmRegistry
    {
        IAlgorithm Get(string code);
        IReadOnlyCollection<IAlgorithm> GetAll();
    }
}
