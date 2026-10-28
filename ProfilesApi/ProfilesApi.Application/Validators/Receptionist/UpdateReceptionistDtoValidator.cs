using FluentValidation;
using ProfilesApi.Application.DTOs.Requests.UpdateRequests;

namespace ProfilesApi.Application.Validators.Receptionist;

public sealed class UpdateReceptionistDtoValidator : AbstractValidator<UpdateReceptionistDto>
{
    public UpdateReceptionistDtoValidator()
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
    }
}
