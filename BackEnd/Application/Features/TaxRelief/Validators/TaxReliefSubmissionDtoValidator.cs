using FluentValidation;
using Paye.Application.DTOs;

namespace Paye.Application.Features.TaxRelief.Validators
{
    public class TaxReliefSubmissionDtoValidator : AbstractValidator<TaxReliefSubmissionDto>
    {
        public TaxReliefSubmissionDtoValidator()
        {
            RuleFor(x => x.StaffId).NotEmpty();
            RuleFor(x => x.TaxYear).GreaterThan(2025);
            RuleFor(x => x.OwnershipType).NotEmpty();
            When(x => x.OwnershipType == "Tenant", () =>
            {
                RuleFor(x => x.AnnualRent).NotNull().GreaterThan(0);
            });
            When(x => x.OwnershipType == "Landlord", () =>
            {
                RuleFor(x => x.AnnualRent).Null();
            });
        }
    }
}
