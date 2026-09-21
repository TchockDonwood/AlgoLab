using AlgoLab.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AlgoLab.Infrastructure.Persistence.Configurations
{
    public class AlgorithmConfiguration : IEntityTypeConfiguration<Algorithm>
    {
        public void Configure(EntityTypeBuilder<Algorithm> builder)
        {
            builder.ToTable("algorithms");

            builder.HasKey(x => x.Id);

            builder.Property(x => x.Code)
                .IsRequired()
                .HasMaxLength(100);

            builder.Property(x => x.Name)
                .IsRequired()
                .HasMaxLength(100);

            builder.HasIndex(x => x.Code)
                .IsUnique();
        }
    }
}
