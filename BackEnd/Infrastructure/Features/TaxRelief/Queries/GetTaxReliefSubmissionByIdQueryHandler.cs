using MediatR;
using Microsoft.EntityFrameworkCore;
using Paye.Application.DTOs;
using Paye.Application.Features.TaxRelief.Queries;
using Paye.Infrastructure.Persistence;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Paye.Infrastructure.Features.TaxRelief.Queries
{
    public class GetTaxReliefSubmissionByIdQueryHandler : IRequestHandler<GetTaxReliefSubmissionByIdQuery, TaxReliefSubmissionDto?>
    {
        private readonly PayeDbContext _context;

        public GetTaxReliefSubmissionByIdQueryHandler(PayeDbContext context)
        {
            _context = context;
        }

        public async Task<TaxReliefSubmissionDto?> Handle(GetTaxReliefSubmissionByIdQuery request, CancellationToken cancellationToken)
        {
            var submission = await _context.TaxReliefSubmissions
                .Include(s => s.RentRelief)
                .Include(s => s.MortgageInterestRelief)
                .Include(s => s.SupportingDocuments)
                .FirstOrDefaultAsync(s => s.Id == request.Id, cancellationToken);

            if (submission == null) return null;

            return new TaxReliefSubmissionDto
            {
                StaffId = submission.StaffId,
                TaxYear = submission.TaxYear,
                OwnershipType = submission.OwnershipType.ToString(),
                AnnualRent = submission.RentRelief?.AnnualRent,
                DeductibleInterest = submission.MortgageInterestRelief?.DeductibleInterest,
                AmortizationScheduleFile = submission.MortgageInterestRelief?.AmortizationScheduleFile,
                SupportingDocuments = submission.SupportingDocuments.Select(d => new SupportingDocumentDto
                {
                    FileName = d.FileName,
                    FileType = d.FileType,
                    StoragePath = d.StoragePath
                }).ToList()
            };
        }
    }
}
