namespace Paye.Client.DTOs
{
    public class LoginDto
    {
        public string UserName { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
    }

    public class LoginResponse
    {
        public string Token { get; set; } = string.Empty;
    }

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

    public class ApiResponse<T>
    {
        public bool Success { get; set; }
        public string Message { get; set; } = string.Empty;
        public T? Data { get; set; }
        public Dictionary<string, List<string>>? Errors { get; set; }
    }
}
