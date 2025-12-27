using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Paye.Domain.Entities;

namespace Paye.Infrastructure.Persistence.Configurations
{
    public class MortgageInterestReliefConfiguration : IEntityTypeConfiguration<MortgageInterestRelief>
    {
        public void Configure(EntityTypeBuilder<MortgageInterestRelief> builder)
        {
            builder.HasKey(m => m.UploadedAt); // Value object, use UploadedAt for uniqueness
            builder.Property(m => m.DeductibleInterest).IsRequired();
            builder.Property(m => m.UploadedBy).IsRequired().HasMaxLength(64);
            builder.Property(m => m.InterestEndDate).IsRequired();
            builder.Property(m => m.AmortizationScheduleFile).IsRequired().HasMaxLength(256);
            builder.Property(m => m.UploadedAt).IsRequired();
        }
    }
}
