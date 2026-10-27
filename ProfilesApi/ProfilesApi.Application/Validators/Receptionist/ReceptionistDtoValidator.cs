using FluentValidation;
using ProfilesApi.Application.DTOs.Responses;

namespace ProfilesApi.Application.Validators.Receptionist;

public sealed class ReceptionistDtoValidator : AbstractValidator<ReceptionistDto>
{
    public ReceptionistDtoValidator()
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
        RuleFor(x => x.WorkStartDate)
            .NotEmpty()
            .LessThan(DateTime.UtcNow)
            .WithMessage("Work start date must be in the past.");
        RuleFor(x => x.OfficeId)
            .NotEmpty();
    }
}
