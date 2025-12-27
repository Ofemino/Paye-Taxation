using MediatR;
using Paye.Application.DTOs;
using Paye.Application.Features.Reporting.Queries;
using Paye.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Paye.Infrastructure.Features.Reporting.Queries
{
    public class GetSubmissionsReportQueryHandler : IRequestHandler<GetSubmissionsReportQuery, List<TaxReliefSubmissionDto>>
    {
        private readonly PayeDbContext _db;
        public GetSubmissionsReportQueryHandler(PayeDbContext db)
        {
            _db = db;
        }

        public async Task<List<TaxReliefSubmissionDto>> Handle(GetSubmissionsReportQuery request, CancellationToken cancellationToken)
        {
            var query = _db.TaxReliefSubmissions
                .Include(t => t.RentRelief)
                .Include(t => t.MortgageInterestRelief)
                .Include(t => t.SupportingDocuments)
                .Join(_db.Staffs,
                      submission => submission.StaffId,
                      staff => staff.Id,
                      (submission, staff) => new { Submission = submission, Staff = staff })
                .AsQueryable();

            if (!string.IsNullOrWhiteSpace(request.StaffId))
                query = query.Where(x => x.Staff.StaffId == request.StaffId);

            if (!string.IsNullOrWhiteSpace(request.Branch))
                query = query.Where(x => x.Staff.Branch == request.Branch);

            if (!string.IsNullOrWhiteSpace(request.State))
                query = query.Where(x => x.Staff.State == request.State);

            if (request.TaxYear.HasValue)
                query = query.Where(x => x.Submission.TaxYear == request.TaxYear);

            var results = await query.Select(x => x.Submission).ToListAsync(cancellationToken);
            return results.Select(submission => new TaxReliefSubmissionDto
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
            }).ToList();
        }
    }
}
