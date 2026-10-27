using FluentValidation;
using ProfilesApi.Application.DTOs.Requests.CreateRequests;

namespace ProfilesApi.Application.Validators.Receptionist;

public sealed class CreateReceptionistDtoValidator : AbstractValidator<CreateReceptionistDto>
{
    public CreateReceptionistDtoValidator()
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
    }
}
