using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Paye.Domain.Entities;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Paye.Infrastructure.Persistence.Interceptors
{
    public class AuditableEntitySaveChangesInterceptor : SaveChangesInterceptor
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public AuditableEntitySaveChangesInterceptor(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public override InterceptionResult<int> SavingChanges(DbContextEventData eventData, InterceptionResult<int> result)
        {
            UpdateEntities(eventData.Context);
            return base.SavingChanges(eventData, result);
        }

        public override ValueTask<InterceptionResult<int>> SavingChangesAsync(DbContextEventData eventData, InterceptionResult<int> result, CancellationToken cancellationToken = default)
        {
            UpdateEntities(eventData.Context);
            return base.SavingChangesAsync(eventData, result, cancellationToken);
        }

        public void UpdateEntities(DbContext? context)
        {
            if (context == null) return;

            var user = _httpContextAccessor.HttpContext?.User?.Identity?.Name ?? "System";

            foreach (var entry in context.ChangeTracker.Entries<TaxReliefSubmission>())
            {
                if (entry.State == EntityState.Added)
                {
                    entry.Entity.AddAuditEntry(new AuditEntry("Create", user, "Submission created"));
                }
                else if (entry.State == EntityState.Modified)
                {
                    // Basic modification tracking
                    // In a real system, we might track specific property changes
                    var properties = entry.Properties.Where(p => p.IsModified && !p.Metadata.IsPrimaryKey());
                    var details = string.Join(", ", properties.Select(p => $"{p.Metadata.Name} changed"));
                    
                    if (!string.IsNullOrEmpty(details))
                    {
                        entry.Entity.AddAuditEntry(new AuditEntry("Update", user, details));
                    }
                }
            }
        }
    }
}
