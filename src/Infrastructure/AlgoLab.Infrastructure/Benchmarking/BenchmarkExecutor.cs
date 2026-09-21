using AlgoLab.Application.Common.Interfaces;
using AlgoLab.Infrastructure.Persistence;

namespace AlgoLab.Infrastructure.Benchmarking
{
    public class BenchmarkExecutor : IBenchmarkExecutor
    {
        private readonly AppDbContext _db;
        private readonly IBenchmarkResultStore _resultStore;
        private readonly IBenchmarkRunner _runner;
        private readonly IAlgorithmRegistry _algorithms;

        public BenchmarkExecutor(
            AppDbContext db,
            IBenchmarkResultStore resultStore,
            IBenchmarkRunner runner,
            IAlgorithmRegistry algorithms)
        {
            _db = db;
            _resultStore = resultStore;
            _runner = runner;
            _algorithms = algorithms;
        }

        public async Task ExecuteAsync(
            Guid sessionId,
            CancellationToken cancellationToken)
        {
            // 1. Получить сессию

            // 2. Проверить, не отменена ли она

            // 3. Поставить RUNNING

            // 4. Получить algorithm

            // 5. Найти реализацию в registry

            // 6. Цикл N

            // 7. Проверить cache

            // 8. Если cache нет → запустить algorithm

            // 9. Создать SessionRun

            // 10. COMPLETED
        }
    }
}