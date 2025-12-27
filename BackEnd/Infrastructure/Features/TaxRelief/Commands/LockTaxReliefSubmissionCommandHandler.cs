using MediatR;
using Paye.Application.Contracts;
using Paye.Application.Features.TaxRelief.Commands;
using Paye.Infrastructure.Persistence;
using System.Threading;
using System.Threading.Tasks;

namespace Paye.Infrastructure.Features.TaxRelief.Commands
{
    public class LockTaxReliefSubmissionCommandHandler : IRequestHandler<LockTaxReliefSubmissionCommand, Result>
    {
        private readonly PayeDbContext _context;

        public LockTaxReliefSubmissionCommandHandler(PayeDbContext context)
        {
            _context = context;
        }

        public async Task<Result> Handle(LockTaxReliefSubmissionCommand request, CancellationToken cancellationToken)
        {
            var submission = await _context.TaxReliefSubmissions.FindAsync(new object[] { request.SubmissionId }, cancellationToken);

            if (submission == null)
            {
                return new Result { Success = false, Error = "Submission not found." };
            }

            if (submission.IsLocked)
            {
                return new Result { Success = false, Error = "Submission is already locked." };
            }

            submission.LockSubmission();
            await _context.SaveChangesAsync(cancellationToken);

            return new Result { Success = true };
        }
    }
}
