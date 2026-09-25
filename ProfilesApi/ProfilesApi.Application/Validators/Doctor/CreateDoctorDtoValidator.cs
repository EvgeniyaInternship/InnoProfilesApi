using FluentValidation;
using ProfilesApi.Application.DTOs.Requests.CreateRequests;

namespace ProfilesApi.Application.Validators.Doctor;

public class CreateDoctorDtoValidator : AbstractValidator<CreateDoctorDto>
{
    public CreateDoctorDtoValidator()
    {
        RuleFor(x => x.AccountId)
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
        RuleFor(x => x.WorkStartDate)
            .NotEmpty()
            .LessThan(DateTime.Now)
            .WithMessage("Work start date must be in the past.");
        RuleFor(x => x.OfficeId)
            .NotEmpty();
        RuleFor(x => x.SpecializationId)
           .NotEmpty();
    }
}
