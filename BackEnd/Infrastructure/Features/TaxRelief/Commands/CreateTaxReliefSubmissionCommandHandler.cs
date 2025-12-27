using MediatR;
using Paye.Application.DTOs;
using Paye.Application.Features.TaxRelief.Commands;
using Paye.Infrastructure.Persistence;
using Paye.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System.Threading;
using System.Threading.Tasks;
using System;
using System.Collections.Generic;

namespace Paye.Infrastructure.Features.TaxRelief.Commands
{
    public class CreateTaxReliefSubmissionCommandHandler : IRequestHandler<CreateTaxReliefSubmissionCommand, Guid>
    {
        private readonly PayeDbContext _db;
        public CreateTaxReliefSubmissionCommandHandler(PayeDbContext db)
        {
            _db = db;
        }

        public async Task<Guid> Handle(CreateTaxReliefSubmissionCommand request, CancellationToken cancellationToken)
        {
            var dto = request.Submission;
            var staff = await _db.Staffs.Include(s => s.Submissions)
                .FirstOrDefaultAsync(s => s.Id == dto.StaffId, cancellationToken);
            if (staff == null)
                throw new KeyNotFoundException("Staff not found");

            // Map DTO to domain entities
            RentRelief? rentRelief = null;
            if (dto.OwnershipType == "Tenant" && dto.AnnualRent.HasValue)
                rentRelief = new RentRelief(dto.AnnualRent.Value, "v1");

            MortgageInterestRelief? mortgageInterest = null;
            if (dto.DeductibleInterest.HasValue && !string.IsNullOrWhiteSpace(dto.AmortizationScheduleFile))
                mortgageInterest = new MortgageInterestRelief(dto.DeductibleInterest.Value, "FCSP", new System.DateTime(dto.TaxYear, 12, 31), dto.AmortizationScheduleFile, dto.TaxYear);

            var submission = new TaxReliefSubmission(staff.Id, dto.TaxYear,
                dto.OwnershipType == "Tenant" ? OwnershipType.Tenant : OwnershipType.Landlord,
                rentRelief, mortgageInterest);

            foreach (var doc in dto.SupportingDocuments)
            {
                submission.AddSupportingDocument(new SupportingDocument(doc.FileName, doc.FileType, doc.StoragePath, staff.Id));
            }

            staff.AddSubmission(submission);
            _db.TaxReliefSubmissions.Add(submission);
            await _db.SaveChangesAsync(cancellationToken);
            return submission.Id;
        }
    }
}
