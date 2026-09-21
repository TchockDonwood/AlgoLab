namespace AlgoLab.Application.Common.Interfaces
{
    public interface IAlgorithm
    {
        string Code { get; }
        Type InputType { get; }
        void Execute(object input);
    }

    public interface IAlgorithm<TInput> : IAlgorithm
    {
        void Execute(TInput input);
        Type IAlgorithm.InputType => typeof(TInput);
        void IAlgorithm.Execute(object input) => Execute((TInput)input);
    }

    public interface IStepAlgorithm : IAlgorithm
    {
        long ExecuteCountingSteps(object input);
        void IAlgorithm.Execute(object input) => ExecuteCountingSteps(input);
    }

    public interface IStepAlgorithm<TInput> : IStepAlgorithm
    {
        long ExecuteCountingSteps(TInput input);

        Type IAlgorithm.InputType => typeof(TInput);
        long IStepAlgorithm.ExecuteCountingSteps(object input)
            => ExecuteCountingSteps((TInput)input);
    }
}
