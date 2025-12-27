using MediatR;
using Paye.Application.DTOs;

namespace Paye.Application.Features.TaxRelief.Commands
{
    public class CreateTaxReliefSubmissionCommand : IRequest<Guid>
    {
        public TaxReliefSubmissionDto Submission { get; set; }
        public CreateTaxReliefSubmissionCommand(TaxReliefSubmissionDto submission)
        {
            Submission = submission;
        }
    }
}
