using FluentValidation;
using ProfilesApi.Application.DTOs.Requests.CreateRequests;

namespace ProfilesApi.Application.Validators.Doctor;

public sealed class CreateDoctorDtoValidator : AbstractValidator<CreateDoctorDto>
{
    public CreateDoctorDtoValidator()
    {
        RuleFor(x => x.AccountId)
            .NotEmpty();
        RuleFor(x => x.BirthDate)
            .NotEmpty()
            .LessThan(DateTime.UtcNow);
        RuleFor(x => x.WorkStartDate)
            .NotEmpty()
            .LessThan(DateTime.UtcNow)
            .WithMessage("Work start date must be in the past.");
        RuleFor(x => x.OfficeId)
            .NotEmpty();
        RuleFor(x => x.SpecializationId)
           .NotEmpty();
    }
}
