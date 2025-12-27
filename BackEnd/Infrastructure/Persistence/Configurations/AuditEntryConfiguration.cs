using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Paye.Domain.Entities;

namespace Paye.Infrastructure.Persistence.Configurations
{
    public class AuditEntryConfiguration : IEntityTypeConfiguration<AuditEntry>
    {
        public void Configure(EntityTypeBuilder<AuditEntry> builder)
        {
            builder.HasKey(a => a.Id);
            builder.Property(a => a.Action).IsRequired().HasMaxLength(128);
            builder.Property(a => a.PerformedBy).IsRequired().HasMaxLength(128);
            builder.Property(a => a.PerformedAt).IsRequired();
            builder.Property(a => a.Details).IsRequired().HasMaxLength(1024);
        }
    }
}
