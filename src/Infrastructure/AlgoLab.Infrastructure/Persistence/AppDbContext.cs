using AlgoLab.Application.Common.Interfaces;
using AlgoLab.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace AlgoLab.Infrastructure.Persistence
{
    public class AppDbContext : DbContext, IApplicationDbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options)
        {
        }

        public DbSet<Algorithm> Algorithms => Set<Algorithm>();
        public DbSet<BenchmarkSession> BenchmarkSessions => Set<BenchmarkSession>();
        public DbSet<BenchmarkRun> BenchmarkRuns => Set<BenchmarkRun>();
        public DbSet<SessionRun> SessionRuns => Set<SessionRun>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(AppDbContext).Assembly);

            modelBuilder.Entity<Algorithm>().HasData(
                new Algorithm { Id = Guid.Parse("11111111-1111-1111-1111-111111111111"), Code = "const-function", Name = "Константная функция", InputArity = 1 },
                new Algorithm { Id = Guid.Parse("11111111-1111-1111-1111-111111111112"), Code = "sum-function", Name = "Сумма элементов", InputArity = 1 },
                new Algorithm { Id = Guid.Parse("11111111-1111-1111-1111-111111111113"), Code = "product-function", Name = "Произведение элементов", InputArity = 1 },
                new Algorithm { Id = Guid.Parse("11111111-1111-1111-1111-111111111114"), Code = "naive-polynomial", Name = "Наивный полином", InputArity = 1 },
                new Algorithm { Id = Guid.Parse("11111111-1111-1111-1111-111111111115"), Code = "horner-polynomial", Name = "Схема Горнера", InputArity = 1 },
                new Algorithm { Id = Guid.Parse("11111111-1111-1111-1111-111111111116"), Code = "bubble-sort", Name = "Сортировка пузырьком", InputArity = 1 },
                new Algorithm { Id = Guid.Parse("11111111-1111-1111-1111-111111111117"), Code = "quick-sort", Name = "Быстрая сортировка", InputArity = 1 },
                new Algorithm { Id = Guid.Parse("11111111-1111-1111-1111-111111111118"), Code = "tim-sort", Name = "Timsort", InputArity = 1 },
                new Algorithm { Id = Guid.Parse("11111111-1111-1111-1111-111111111119"), Code = "multiply-matrix", Name = "Умножение матриц", InputArity = 2 },
                new Algorithm { Id = Guid.Parse("11111111-1111-1111-1111-11111111111A"), Code = "smooth-sort", Name = "Плавная сортировка", InputArity = 1 },
                new Algorithm { Id = Guid.Parse("11111111-1111-1111-1111-11111111111B"), Code = "sieve-of-eratosthenes", Name = "Решето Эратосфена", InputArity = 1 },
                new Algorithm { Id = Guid.Parse("11111111-1111-1111-1111-11111111111C"), Code = "simple-pow", Name = "Простое возведение в степень", InputArity = 1 },
                new Algorithm { Id = Guid.Parse("11111111-1111-1111-1111-11111111111D"), Code = "recursive-pow", Name = "Рекурсивное возведение в степень", InputArity = 1 },
                new Algorithm { Id = Guid.Parse("11111111-1111-1111-1111-11111111111E"), Code = "quick-recursive-pow", Name = "Быстрое рекурсивное возведение в степень", InputArity = 1 },
                new Algorithm { Id = Guid.Parse("11111111-1111-1111-1111-11111111111F"), Code = "atkin-sieve", Name = "Решето Аткина", InputArity = 1 }
            );
        }
    }
}