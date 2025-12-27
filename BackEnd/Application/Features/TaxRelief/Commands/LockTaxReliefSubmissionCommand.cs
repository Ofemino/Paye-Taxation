using MediatR;
using Paye.Application.Contracts;
using System;

namespace Paye.Application.Features.TaxRelief.Commands
{
    public class LockTaxReliefSubmissionCommand : IRequest<Result>
    {
        public Guid SubmissionId { get; set; }

        public LockTaxReliefSubmissionCommand(Guid submissionId)
        {
            SubmissionId = submissionId;
        }
    }
}
