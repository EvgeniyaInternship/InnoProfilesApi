using FluentValidation;
using ProfilesApi.Application.Validators.Constants;

namespace ProfilesApi.Application.Validators;

public static class ValidationExtensions
{
    public static IRuleBuilderOptions<T, string> ValidNationalInsuranceNumber<T>(this IRuleBuilder<T, string> ruleBuilder)
    {
        return ruleBuilder
            .NotEmpty()
            .WithMessage("Insurance number is required.")
            .Matches(ValidationConstants.NationalInsuranceNumberRegex)
            .WithMessage("Invalid National Insurance Number format.");
    }
}