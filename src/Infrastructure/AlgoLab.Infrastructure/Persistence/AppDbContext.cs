using AlgoLab.Application.Common.Interfaces;
using AlgoLab.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace AlgoLab.Infrastructure.Persistence
{
    public class AppDbContext : DbContext, IApplicationDbContext
    {
        public AppDbContext(
            DbContextOptions<AppDbContext> options)
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
                new Algorithm { Id = Guid.Parse("11111111-1111-1111-1111-111111111111"), Code = "const-function", Name = "Const Function" },
                new Algorithm { Id = Guid.Parse("11111111-1111-1111-1111-111111111112"), Code = "sum-function", Name = "Sum Function" },
                new Algorithm { Id = Guid.Parse("11111111-1111-1111-1111-111111111113"), Code = "product-function", Name = "Product Function" },
                new Algorithm { Id = Guid.Parse("11111111-1111-1111-1111-111111111114"), Code = "naive-polynomial", Name = "Naive Polynomial" },
                new Algorithm { Id = Guid.Parse("11111111-1111-1111-1111-111111111115"), Code = "horner-polynomial", Name = "Horner Polynomial" },
                new Algorithm { Id = Guid.Parse("11111111-1111-1111-1111-111111111116"), Code = "bubble-sort", Name = "Bubble Sort" },
                new Algorithm { Id = Guid.Parse("11111111-1111-1111-1111-111111111117"), Code = "quick-sort", Name = "Quick Sort" },
                new Algorithm { Id = Guid.Parse("11111111-1111-1111-1111-111111111118"), Code = "tim-sort", Name = "Tim Sort" },
                new Algorithm { Id = Guid.Parse("11111111-1111-1111-1111-111111111119"), Code = "multiply-matrix", Name = "Matrix Multiplication" },
                new Algorithm { Id = Guid.Parse("11111111-1111-1111-1111-11111111111A"), Code = "smooth-sort", Name = "Smooth Sort" },
                new Algorithm { Id = Guid.Parse("11111111-1111-1111-1111-11111111111B"), Code = "sieve-of-eratosthenes", Name = "Sieve of Eratosthenes" },
                new Algorithm { Id = Guid.Parse("11111111-1111-1111-1111-11111111111C"), Code = "simple-pow", Name = "Simple Pow" },
                new Algorithm { Id = Guid.Parse("11111111-1111-1111-1111-11111111111D"), Code = "recursive-pow", Name = "Recursive Pow" },
                new Algorithm { Id = Guid.Parse("11111111-1111-1111-1111-11111111111E"), Code = "quick-recursive-pow", Name = "Quick Recursive Pow" }
            );
        }
    }
}
