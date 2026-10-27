using FluentValidation;
using ProfilesApi.Application.DTOs.Responses;

namespace ProfilesApi.Application.Validators.Patient;

public sealed class PatientDtoValidator : AbstractValidator<PatientDto>
{
    public PatientDtoValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty();
        RuleFor(x => x.CreatedAt)
            .NotEmpty()
            .LessThan(DateTime.UtcNow);
        RuleFor(x => x.UpdatedAt)
            .NotEmpty()
            .LessThan(DateTime.UtcNow);
        RuleFor(x => x.AccountId)
            .NotEmpty();
        RuleFor(x => x.BirthDate)
            .NotEmpty()
            .LessThan(DateTime.UtcNow);
        RuleFor(x => x.InsuranceNumber)
           .ValidNationalInsuranceNumber();
    }
}
