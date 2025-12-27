using System;
using System.Collections.Generic;
using System.Linq;

namespace Paye.Domain.Entities
{
    public class TaxReliefSubmission
    {
        public Guid Id { get; private set; }
        public Guid StaffId { get; private set; }
        public int TaxYear { get; private set; }
        public OwnershipType OwnershipType { get; private set; }
        public RentRelief? RentRelief { get; private set; }
        public MortgageInterestRelief? MortgageInterestRelief { get; private set; }
        public IReadOnlyCollection<SupportingDocument> SupportingDocuments => _supportingDocuments.AsReadOnly();
        private readonly List<SupportingDocument> _supportingDocuments = new();
        public DateTime SubmittedAt { get; private set; }
        public bool IsLocked { get; private set; }
        public IReadOnlyCollection<AuditEntry> AuditTrail => _auditTrail.AsReadOnly();
        private readonly List<AuditEntry> _auditTrail = new();

        public TaxReliefSubmission(Guid staffId, int taxYear, OwnershipType ownershipType, RentRelief? rentRelief, MortgageInterestRelief? mortgageInterestRelief)
        {
            Id = Guid.NewGuid();
            StaffId = staffId;
            TaxYear = taxYear;
            OwnershipType = ownershipType;
            if (ownershipType == OwnershipType.Tenant && rentRelief == null)
                throw new InvalidOperationException("Rent relief is required for tenants.");
            if (ownershipType == OwnershipType.Landlord && rentRelief != null)
                throw new InvalidOperationException("Rent relief must not be provided for landlords.");
            RentRelief = rentRelief;
            MortgageInterestRelief = mortgageInterestRelief;
            SubmittedAt = DateTime.UtcNow;
            IsLocked = false;
        }

        public void AddSupportingDocument(SupportingDocument doc)
        {
            if (IsLocked) throw new InvalidOperationException("Submission is locked and cannot be modified.");
            _supportingDocuments.Add(doc);
        }

        public void LockSubmission()
        {
            IsLocked = true;
        }

        public void AddAuditEntry(AuditEntry entry)
        {
            _auditTrail.Add(entry);
        }
    }

    public enum OwnershipType
    {
        Tenant = 1,
        Landlord = 2
    }
}
