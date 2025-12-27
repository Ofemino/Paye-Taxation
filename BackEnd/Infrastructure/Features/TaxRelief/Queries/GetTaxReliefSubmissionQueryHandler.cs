using MediatR;
using Paye.Application.DTOs;
using Paye.Application.Features.TaxRelief.Queries;
using Paye.Infrastructure.Persistence;
using Paye.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System.Threading;
using System.Threading.Tasks;
using System.Linq;

namespace Paye.Infrastructure.Features.TaxRelief.Queries
{
    public class GetTaxReliefSubmissionQueryHandler : IRequestHandler<GetTaxReliefSubmissionQuery, TaxReliefSubmissionDto>
    {
        private readonly PayeDbContext _db;
        public GetTaxReliefSubmissionQueryHandler(PayeDbContext db)
        {
            _db = db;
        }

        public async Task<TaxReliefSubmissionDto> Handle(GetTaxReliefSubmissionQuery request, CancellationToken cancellationToken)
        {
            var submission = await _db.TaxReliefSubmissions
                .Include(t => t.RentRelief)
                .Include(t => t.MortgageInterestRelief)
                .Include(t => t.SupportingDocuments)
                .FirstOrDefaultAsync(t => t.StaffId == request.StaffId && t.TaxYear == request.TaxYear, cancellationToken);

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
