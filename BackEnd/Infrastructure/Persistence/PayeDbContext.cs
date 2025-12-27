using Microsoft.EntityFrameworkCore;
using Paye.Domain.Entities;
using Paye.Infrastructure.Persistence.Interceptors;

namespace Paye.Infrastructure.Persistence
{
    public class PayeDbContext : DbContext
    {
        private readonly AuditableEntitySaveChangesInterceptor _auditableEntitySaveChangesInterceptor;

        public PayeDbContext(DbContextOptions<PayeDbContext> options, AuditableEntitySaveChangesInterceptor auditableEntitySaveChangesInterceptor) : base(options) 
        {
            _auditableEntitySaveChangesInterceptor = auditableEntitySaveChangesInterceptor;
        }

        public DbSet<Staff> Staffs { get; set; }
        public DbSet<TaxReliefSubmission> TaxReliefSubmissions { get; set; }
        public DbSet<RentRelief> RentReliefs { get; set; }
        public DbSet<MortgageInterestRelief> MortgageInterestReliefs { get; set; }
        public DbSet<SupportingDocument> SupportingDocuments { get; set; }
        public DbSet<AuditEntry> AuditEntries { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.AddInterceptors(_auditableEntitySaveChangesInterceptor);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(PayeDbContext).Assembly);
            base.OnModelCreating(modelBuilder);
        }
    }
}
