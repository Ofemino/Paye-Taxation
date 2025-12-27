using System;

namespace Paye.Domain.Entities
{
    public class AuditEntry
    {
        public Guid Id { get; private set; }
        public string Action { get; private set; }
        public string PerformedBy { get; private set; }
        public DateTime PerformedAt { get; private set; }
        public string Details { get; private set; }

        public AuditEntry(string action, string performedBy, string details)
        {
            Id = Guid.NewGuid();
            Action = action;
            PerformedBy = performedBy;
            PerformedAt = DateTime.UtcNow;
            Details = details;
        }
    }
}
