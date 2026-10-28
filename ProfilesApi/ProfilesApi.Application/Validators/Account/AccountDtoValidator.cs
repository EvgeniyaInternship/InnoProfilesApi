using FluentValidation;
using ProfilesApi.Application.DTOs.Responses;

namespace ProfilesApi.Application.Validators.Account;

public sealed class AccountDtoValidator : AbstractValidator<AccountDto>
{
    public AccountDtoValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty();

        RuleFor(x => x.CreatedAt)
            .NotEmpty()
            .LessThan(DateTime.UtcNow);

        RuleFor(x => x.UpdatedAt)
            .NotEmpty()
            .LessThan(DateTime.UtcNow);

        RuleFor(x => x.PhoneNumber)
            .NotEmpty()
            .ValidPhoneNumber()
            .WithMessage("Invalid phone number format.");

        RuleFor(x => x.Email)
            .NotEmpty()
            .EmailAddress();

        RuleFor(x => x.Role)
            .IsInEnum();

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
