using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Paye.Domain.Entities;

namespace Paye.Infrastructure.Persistence.Configurations
{
    public class TaxReliefSubmissionConfiguration : IEntityTypeConfiguration<TaxReliefSubmission>
    {
        public void Configure(EntityTypeBuilder<TaxReliefSubmission> builder)
        {
            builder.HasKey(t => t.Id);
            builder.Property(t => t.TaxYear).IsRequired();
            builder.Property(t => t.OwnershipType).IsRequired();
            builder.Property(t => t.IsLocked).IsRequired();
            builder.Property(t => t.SubmittedAt).IsRequired();
            builder.HasOne(t => t.RentRelief)
                   .WithOne()
                   .HasForeignKey<TaxReliefSubmission>(t => t.Id)
                   .IsRequired(false);
            builder.HasOne(t => t.MortgageInterestRelief)
                   .WithOne()
                   .HasForeignKey<TaxReliefSubmission>(t => t.Id)
                   .IsRequired(false);
            builder.HasMany(typeof(SupportingDocument), "SupportingDocuments")
                   .WithOne()
                   .HasForeignKey("TaxReliefSubmissionId");
            builder.HasMany(typeof(AuditEntry), "AuditTrail")
                   .WithOne()
                   .HasForeignKey("TaxReliefSubmissionId");
        }
    }
}
