namespace AlgoLab.Domain.Entities
{
    public class Algorithm
    {
        public Guid Id { get; set; }
        public string Code { get; set; } = null!;
        public string Name { get; set; } = null!;
        public int InputArity { get; set; } = 1;
    }
}
