using AlgoLab.Application.Common.Interfaces;

namespace AlgoLab.Algorithms.Implementations
{
    public sealed class PermutationsBacktrack : IAlgorithm<int[]>
    {
        public string Code => "permutations-backtrack";
        public string Name => "Permutations generator (Backtracking)";

        // Свойство для хранения результата после вызова Execute
        public List<IList<int>> Result { get; private set; } = new List<IList<int>>();

        public void Execute(int[] nums)
        {
            Result.Clear(); // Очищаем предыдущий результат
            
            if (nums == null) 
                throw new ArgumentNullException(nameof(nums));
            if (nums.Length == 0) 
                return;

            var current = new List<int>();
            var used = new bool[nums.Length];

            Backtrack(nums, current, used, Result);
        }

        private void Backtrack(int[] nums, List<int> current, bool[] used, List<IList<int>> result)
        {   
            //перестановка собрана
            if (current.Count == nums.Length)
            {   
                // Добавляем копию, чтобы избежать ссылки на один и тот же изменяемый список
                result.Add(new List<int>(current));
                return;
            }

            for (int i = 0; i < nums.Length; i++)
            {
                if (used[i]) continue;
                
                // Choose 
                used[i] = true;
                current.Add(nums[i]);
                
                // Explore 
                Backtrack(nums, current, used, result);
                
                // Undo 
                current.RemoveAt(current.Count - 1);
                used[i] = false;
            }
        }
    }
}