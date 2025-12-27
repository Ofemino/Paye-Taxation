using MediatR;
using Paye.Application.DTOs;

namespace Paye.Application.Features.TaxRelief.Queries
{
    public class GetTaxReliefSubmissionQuery : IRequest<TaxReliefSubmissionDto>
    {
        public Guid StaffId { get; set; }
        public int TaxYear { get; set; }
        public GetTaxReliefSubmissionQuery(Guid staffId, int taxYear)
        {
            StaffId = staffId;
            TaxYear = taxYear;
        }
    }
}
