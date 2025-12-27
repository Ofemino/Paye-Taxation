using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Paye.Domain.Entities;

namespace Paye.Infrastructure.Persistence.Configurations
{
    public class StaffConfiguration : IEntityTypeConfiguration<Staff>
    {
        public void Configure(EntityTypeBuilder<Staff> builder)
        {
            builder.HasKey(s => s.Id);
            builder.Property(s => s.StaffId).IsRequired().HasMaxLength(32);
            builder.Property(s => s.FirstName).IsRequired().HasMaxLength(64);
            builder.Property(s => s.LastName).IsRequired().HasMaxLength(64);
            builder.Property(s => s.Email).IsRequired().HasMaxLength(128);
            builder.Property(s => s.Branch).IsRequired().HasMaxLength(64);
            builder.Property(s => s.State).IsRequired().HasMaxLength(64);
            builder.Property(s => s.IsActive).IsRequired();
            builder.HasMany(s => s.Submissions)
                   .WithOne()
                   .HasForeignKey(t => t.StaffId);
        }
    }
}
