namespace Paye.Application.DTOs
{
    public class TaxReliefSubmissionDto
    {
        public Guid StaffId { get; set; }
        public int TaxYear { get; set; }
        public string OwnershipType { get; set; } = string.Empty;
        public decimal? AnnualRent { get; set; }
        public decimal? DeductibleInterest { get; set; }
        public string? AmortizationScheduleFile { get; set; }
        public List<SupportingDocumentDto> SupportingDocuments { get; set; } = new();
    }

    public class SupportingDocumentDto
    {
        public string FileName { get; set; } = string.Empty;
        public string FileType { get; set; } = string.Empty;
        public string StoragePath { get; set; } = string.Empty;
    }
}
