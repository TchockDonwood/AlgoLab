using AlgoLab.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AlgoLab.Infrastructure.Persistence.Configurations
{
    public class BenchmarkRunConfiguration : IEntityTypeConfiguration<BenchmarkRun>
    {
        public void Configure(EntityTypeBuilder<BenchmarkRun> builder) 
        {
            builder.ToTable("benchmark_runs");

            builder.HasKey(x => x.Id);

            builder.Property(x => x.ExecutionTimeMs)
                .IsRequired();

            builder.HasIndex(x => new
            {
                x.AlgorithmId,
                x.N
            })
            .IsUnique();

            builder.HasOne(x => x.Algorithm)
                .WithMany()
                .HasForeignKey(x => x.AlgorithmId);
        }
    }
}
