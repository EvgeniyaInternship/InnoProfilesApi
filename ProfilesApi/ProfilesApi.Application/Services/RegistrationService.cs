using Microsoft.Extensions.Logging;
using ProfilesApi.Application.DTOs.Requests.Registration;
using ProfilesApi.Application.DTOs.Responses.Registration;
using ProfilesApi.Application.Interfaces;
using ProfilesApi.Application.Mappings;
using ProfilesApi.Domain.Entities;
using ProfilesApi.Domain.Enum;
using ProfilesApi.Domain.Exceptions;
using ProfilesApi.Domain.Interfaces;

namespace ProfilesApi.Application.Services;

public sealed class RegistrationService(
    IUnitOfWork unitOfWork,
    IIdentityClient identityClient,
    ILogger<RegistrationService> logger) : IRegistrationService
{
    public async Task<RegisteredUserDto> RegisterAsync(RegisterUserDto dto, CancellationToken cancellationToken = default)
    {
        var role = ResolveRole(dto);
        await EnsureAccountIsAvailableAsync(dto.Email, dto.PhoneNumber, cancellationToken);
        if (dto is RegisterPatientDto patient)
            await EnsureInsuranceNumberIsAvailableAsync(patient.InsuranceNumber, cancellationToken);

        var userId = await identityClient.RegisterAsync(dto.ToIdentityRequest(role), cancellationToken);

        using var transaction = await unitOfWork.BeginTransactionAsync(cancellationToken: cancellationToken);
        try
        {
            var account = dto.ToAccount(userId, role);
            var profile = dto.ToProfile(userId);

            unitOfWork.Accounts.Add(account);
            AddProfile(profile);
            await unitOfWork.SaveChangesAsync(cancellationToken);
            await transaction.CommitAsync(cancellationToken);
            logger.LogInformation("Registration committed for account {AccountId} with role {Role}.", account.Id, role);

            return profile.ToRegisteredDto(account);
        }
        catch (Exception ex) when (ex is not AppException)
        {
            logger.LogError(ex, "Registration failed for account {AccountId}.", userId);
            await transaction.RollbackAsync(cancellationToken);
            await TryDeleteIdentityAsync(userId);
            throw new ServerException("An error occurred while registering the user.", ex);
        }
        catch
        {
            await transaction.RollbackAsync(cancellationToken);
            await TryDeleteIdentityAsync(userId);
            throw;
        }
    }

    private void AddProfile(PersonEntity profile)
    {
        switch (profile)
        {
            case PatientEntity patient:
                unitOfWork.Patients.Add(patient);
                break;
            case DoctorEntity doctor:
                unitOfWork.Doctors.Add(doctor);
                break;
            case AdminEntity admin:
                unitOfWork.Admins.Add(admin);
                break;
            case ReceptionistEntity receptionist:
                unitOfWork.Receptionists.Add(receptionist);
                break;
            default:
                logger.LogWarning("Unsupported registration role.");
                throw new ArgumentException("Unsupported registration role.");
        }
    }

    private async Task EnsureAccountIsAvailableAsync(string email, string phoneNumber, CancellationToken cancellationToken)
    {
        var account = await unitOfWork.Accounts.GetAsync(
            x => x.Email == email || x.PhoneNumber == phoneNumber,
            cancellationToken: cancellationToken);

        if (account is not null)
        {
            logger.LogWarning("Account with email {Email} or phone {PhoneNumber} already exists.", email, phoneNumber);
            throw new ConflictException($"Account with Email {email} or Phone Number {phoneNumber} already exists.");
        }
    }

    private async Task EnsureInsuranceNumberIsAvailableAsync(string insuranceNumber, CancellationToken cancellationToken)
    {
        var patient = await unitOfWork.Patients.GetAsync(
            x => x.InsuranceNumber == insuranceNumber,
            cancellationToken: cancellationToken);

        if (patient is not null)
        {
            logger.LogWarning("Insurance number {InsuranceNumber} is already registered.", insuranceNumber);
            throw new ConflictException($"Insurance number {insuranceNumber} is already registered.");
        }
    }

    private async Task TryDeleteIdentityAsync(Guid userId)
    {
        try
        {
            await identityClient.DeleteAsync(userId, CancellationToken.None);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Failed to remove identity user {UserId} after registration failed.", userId);
        }
    }

    private AccountRole LogUnsupportedRole()
    {
        logger.LogWarning("Unsupported registration role.");
        throw new ArgumentException("Unsupported registration role.");
    }

    private AccountRole ResolveRole(RegisterUserDto dto)
    {
        return dto switch
        {
            RegisterPatientDto => AccountRole.Patient,
            RegisterDoctorDto => AccountRole.Doctor,
            RegisterAdminDto => AccountRole.Admin,
            RegisterReceptionistDto => AccountRole.Receptionist,
            _ => LogUnsupportedRole()
        };
    }
}
