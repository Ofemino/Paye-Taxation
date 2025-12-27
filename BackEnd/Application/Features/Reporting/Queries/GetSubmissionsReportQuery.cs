using MediatR;
using Paye.Application.DTOs;
using System.Collections.Generic;

namespace Paye.Application.Features.Reporting.Queries
{
    public class GetSubmissionsReportQuery : IRequest<List<TaxReliefSubmissionDto>>
    {
        public string? StaffId { get; set; }
        public string? Branch { get; set; }
        public string? State { get; set; }
        public int? TaxYear { get; set; }
    }
}
