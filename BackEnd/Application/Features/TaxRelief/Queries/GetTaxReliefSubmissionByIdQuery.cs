using MediatR;
using Paye.Application.DTOs;
using System;

namespace Paye.Application.Features.TaxRelief.Queries
{
    public class GetTaxReliefSubmissionByIdQuery : IRequest<TaxReliefSubmissionDto?>
    {
        public Guid Id { get; set; }

        public GetTaxReliefSubmissionByIdQuery(Guid id)
        {
            Id = id;
        }
    }
}
