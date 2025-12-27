using System;
using Paye.Domain.Entities;
using Xunit;

namespace Paye.Tests.Domain
{
    public class StaffTests
    {
        [Fact]
        public void Cannot_Add_More_Than_One_Submission_Per_Year()
        {
            var staff = new Staff("S001", "John", "Doe", "john@company.com", "HQ", "Lagos", true);
            var submission1 = new TaxReliefSubmission(staff.Id, 2026, OwnershipType.Tenant, new RentRelief(1000000, "v1"), null);
            staff.AddSubmission(submission1);
            var submission2 = new TaxReliefSubmission(staff.Id, 2026, OwnershipType.Tenant, new RentRelief(900000, "v1"), null);
            Assert.Throws<InvalidOperationException>(() => staff.AddSubmission(submission2));
        }

        [Fact]
        public void Inactive_Staff_Cannot_Add_Submission()
        {
            var staff = new Staff("S002", "Jane", "Smith", "jane@company.com", "Branch", "Abuja", false);
            var submission = new TaxReliefSubmission(staff.Id, 2026, OwnershipType.Tenant, new RentRelief(800000, "v1"), null);
            Assert.Throws<InvalidOperationException>(() => staff.AddSubmission(submission));
        }
    }
}
