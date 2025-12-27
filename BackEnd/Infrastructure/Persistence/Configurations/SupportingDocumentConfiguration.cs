using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Paye.Domain.Entities;

namespace Paye.Infrastructure.Persistence.Configurations
{
    public class SupportingDocumentConfiguration : IEntityTypeConfiguration<SupportingDocument>
    {
        public void Configure(EntityTypeBuilder<SupportingDocument> builder)
        {
            builder.HasKey(d => d.Id);
            builder.Property(d => d.FileName).IsRequired().HasMaxLength(256);
            builder.Property(d => d.FileType).IsRequired().HasMaxLength(16);
            builder.Property(d => d.StoragePath).IsRequired().HasMaxLength(512);
            builder.Property(d => d.UploadedAt).IsRequired();
            builder.Property(d => d.UploadedBy).IsRequired();
        }
    }
}
