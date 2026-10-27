using FluentValidation;
using ProfilesApi.Application.DTOs.Requests.UpdateRequests;

namespace ProfilesApi.Application.Validators.Doctor;

public sealed class UpdateDoctorDtoValidator : AbstractValidator<UpdateDoctorDto>
{
    public UpdateDoctorDtoValidator()
    {
        RuleFor(x => x.Id)
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
