using System;

namespace Paye.Domain.Entities
{
    public class MortgageInterestRelief
    {
        public decimal DeductibleInterest { get; private set; }
        public string UploadedBy { get; private set; } // FC&SP role
        public DateTime InterestEndDate { get; private set; }
        public string AmortizationScheduleFile { get; private set; }
        public DateTime UploadedAt { get; private set; }

        public MortgageInterestRelief(decimal deductibleInterest, string uploadedBy, DateTime interestEndDate, string amortizationScheduleFile, int taxYear)
        {
            if (string.IsNullOrWhiteSpace(uploadedBy))
                throw new InvalidOperationException("Mortgage interest can only be set by FC&SP role.");
            if (interestEndDate > new DateTime(taxYear, 12, 31))
                throw new InvalidOperationException("Interest end date must not exceed 31 December of the tax year.");
            DeductibleInterest = deductibleInterest;
            UploadedBy = uploadedBy;
            InterestEndDate = interestEndDate;
            AmortizationScheduleFile = amortizationScheduleFile;
            UploadedAt = DateTime.UtcNow;
        }
    }
}
