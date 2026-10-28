using FluentValidation;
using ProfilesApi.Application.DTOs.Requests.Registration;
using ProfilesApi.Application.Validators;

namespace ProfilesApi.Application.Validators.Registration;

public sealed class RegisterUserDtoValidator : AbstractValidator<RegisterUserDto>
{
    public RegisterUserDtoValidator()
    {
        RuleFor(x => x.Email)
            .NotEmpty()
            .EmailAddress()
            .WithMessage("Invalid email format.")
            .MaximumLength(150)
            .WithMessage("Email cannot exceed 150 characters.");

        RuleFor(x => x.Password)
            .NotEmpty()
            .MinimumLength(8)
            .WithMessage("Password must be at least 8 characters long.");

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

        RuleFor(x => x.PhoneNumber)
            .NotEmpty()
            .ValidPhoneNumber()
            .WithMessage("Invalid phone number format.");

        RuleFor(x => x.BirthDate)
            .NotEmpty()
            .LessThan(DateTime.UtcNow);

        RuleFor(x => x.PhotoId)
            .Must(id => id != Guid.Empty)
            .When(x => x.PhotoId.HasValue)
            .WithMessage("Photo id cannot be empty.");

        RuleFor(x => x).SetInheritanceValidator(validator =>
        {
            validator.Add(new RegisterPatientDtoValidator());
            validator.Add(new RegisterDoctorDtoValidator());
            validator.Add(new RegisterAdminDtoValidator());
            validator.Add(new RegisterReceptionistDtoValidator());
        });
    }
}

internal sealed class RegisterPatientDtoValidator : AbstractValidator<RegisterPatientDto>
{
    public RegisterPatientDtoValidator()
    {
        RuleFor(x => x.InsuranceNumber)
            .ValidNationalInsuranceNumber();
    }
}

internal sealed class RegisterDoctorDtoValidator : AbstractValidator<RegisterDoctorDto>
{
    public RegisterDoctorDtoValidator()
    {
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

internal sealed class RegisterAdminDtoValidator : AbstractValidator<RegisterAdminDto>
{
    public RegisterAdminDtoValidator()
    {
        RuleFor(x => x.WorkStartDate)
            .NotEmpty()
            .LessThan(DateTime.UtcNow)
            .WithMessage("Work start date must be in the past.");

        RuleFor(x => x.OfficeId)
            .NotEmpty();
    }
}

internal sealed class RegisterReceptionistDtoValidator : AbstractValidator<RegisterReceptionistDto>
{
    public RegisterReceptionistDtoValidator()
    {
        RuleFor(x => x.WorkStartDate)
            .NotEmpty()
            .LessThan(DateTime.UtcNow)
            .WithMessage("Work start date must be in the past.");

        RuleFor(x => x.OfficeId)
            .NotEmpty();
    }
}
