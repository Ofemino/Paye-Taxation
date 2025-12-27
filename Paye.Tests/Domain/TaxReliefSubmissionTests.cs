using System;
using Paye.Domain.Entities;
using Xunit;

namespace Paye.Tests.Domain
{
    public class TaxReliefSubmissionTests
    {
        [Fact]
        public void Tenant_Must_Have_RentRelief()
        {
            Assert.Throws<InvalidOperationException>(() =>
                new TaxReliefSubmission(Guid.NewGuid(), 2026, OwnershipType.Tenant, null, null));
        }

        [Fact]
        public void Landlord_Cannot_Have_RentRelief()
        {
            Assert.Throws<InvalidOperationException>(() =>
                new TaxReliefSubmission(Guid.NewGuid(), 2026, OwnershipType.Landlord, new RentRelief(1000000, "v1"), null));
        }

        [Fact]
        public void Submission_Is_Immutable_After_Lock()
        {
            var submission = new TaxReliefSubmission(Guid.NewGuid(), 2026, OwnershipType.Tenant, new RentRelief(1000000, "v1"), null);
            submission.LockSubmission();
            Assert.Throws<InvalidOperationException>(() => submission.AddSupportingDocument(
                new SupportingDocument("doc.pdf", "pdf", "/files/doc.pdf", Guid.NewGuid())));
        }
    }
}
