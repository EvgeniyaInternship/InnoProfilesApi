using FluentValidation;
using ProfilesApi.Application.DTOs.Requests.UpdateRequests;

namespace ProfilesApi.Application.Validators.Account;

public sealed class UpdateAccountDtoValidator : AbstractValidator<UpdateAccountDto>
{
    public UpdateAccountDtoValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty();

        RuleFor(x => x.PhoneNumber)
            .NotEmpty()
            .ValidPhoneNumber()
            .WithMessage("Invalid phone number format.");

        RuleFor(x => x.Email)
            .NotEmpty()
            .EmailAddress()
            .WithMessage("Invalid email format.")
            .MaximumLength(150)
            .WithMessage("Email cannot exceed 150 characters.");

        RuleFor(x => x.Role)
            .IsInEnum()
            .WithMessage("Invalid account role.");

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
    }
}
