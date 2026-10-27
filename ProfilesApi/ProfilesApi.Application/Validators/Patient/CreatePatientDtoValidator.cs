using FluentValidation;
using ProfilesApi.Application.DTOs.Requests.CreateRequests;

namespace ProfilesApi.Application.Validators.Patient;

public sealed class CreatePatientDtoValidator : AbstractValidator<CreatePatientDto>
{
    public CreatePatientDtoValidator()
    {
        RuleFor(x => x.AccountId)
            .NotEmpty();
        RuleFor(x => x.BirthDate)
            .NotEmpty()
            .LessThan(DateTime.UtcNow);
        RuleFor(x => x.InsuranceNumber)
            .ValidNationalInsuranceNumber();
    }
}
