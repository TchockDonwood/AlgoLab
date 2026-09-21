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
            modelBuilder.ApplyConfigurationsFromAssembly(
                typeof(AppDbContext).Assembly);
        }
    }
}
