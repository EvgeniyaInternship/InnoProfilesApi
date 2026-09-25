using FluentValidation;
using ProfilesApi.Application.DTOs.Requests.UpdateRequests;

namespace ProfilesApi.Application.Validators.Patient;

public class UpdatePatientDtoValidator : AbstractValidator<UpdatePatientDto>
{
    public UpdatePatientDtoValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty();
        RuleFor(x => x.BirthDate)
            .NotEmpty()
            .LessThan(DateTime.Now);
        RuleFor(x => x.FirstName)
            .NotEmpty()
            .MaximumLength(50)
            .WithMessage("First name cannot exceed 50 characters.");
        RuleFor(x => x.LastName)
            .NotEmpty()
            .MaximumLength(50)
            .WithMessage("Last name cannot exceed 50 characters.");
        RuleFor(x => x.MiddleName)
            .MaximumLength(50)
            .WithMessage("Middle name cannot exceed 50 characters.");
        RuleFor(x => x.InsuranceNumber)
           .NotEmpty()
           .WithMessage("Insurance number is required.")
           .Matches(@"^(?i)(?!(?:BG|GB|KN|NK|NT|TN|ZZ))[A-CEGHJ-PR-TW-Z][A-NP-Z](?:\s*\d{2}){3}\s*[A-D]$")
           .WithMessage("Invalid National Insurance Number format.");
    }
}
