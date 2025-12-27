using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Paye.Domain.Entities;

namespace Paye.Infrastructure.Persistence.Configurations
{
    public class RentReliefConfiguration : IEntityTypeConfiguration<RentRelief>
    {
        public void Configure(EntityTypeBuilder<RentRelief> builder)
        {
            builder.HasKey(r => r.ComputedAt); // Value object, no natural PK, use ComputedAt for uniqueness
            builder.Property(r => r.AnnualRent).IsRequired();
            builder.Property(r => r.ComputedAmount).IsRequired();
            builder.Property(r => r.Cap).IsRequired();
            builder.Property(r => r.FormulaVersion).IsRequired().HasMaxLength(16);
            builder.Property(r => r.ComputedAt).IsRequired();
        }
    }
}
