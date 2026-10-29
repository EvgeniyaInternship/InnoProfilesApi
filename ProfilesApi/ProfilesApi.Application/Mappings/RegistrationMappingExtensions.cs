using ProfilesApi.Application.DTOs.Requests.Registration;
using ProfilesApi.Application.DTOs.Responses.Registration;
using ProfilesApi.Application.Security;
using ProfilesApi.Domain.Entities;
using ProfilesApi.Domain.Enum;

namespace ProfilesApi.Application.Mappings;

public static class RegistrationMappingExtensions
{
    public static IdentityRegistrationRequest ToIdentityRequest(this RegisterUserDto dto, AccountRole role)
    {
        return new IdentityRegistrationRequest(
            dto.Email,
            dto.Password,
            dto.FirstName,
            dto.LastName,
            role.ToString());
    }

    public static AccountEntity ToAccount(this RegisterUserDto dto, Guid id, AccountRole role)
    {
        return new AccountEntity
        {
            Id = id,
            PhoneNumber = dto.PhoneNumber,
            Email = dto.Email,
            HashedPassword = PasswordHasher.Hash(dto.Password),
            Role = role,
            PhotoId = dto.PhotoId,
            FirstName = dto.FirstName,
            LastName = dto.LastName,
            MiddleName = dto.MiddleName
        };
    }

    public static PersonEntity ToProfile(this RegisterUserDto dto, Guid accountId)
    {
        return dto switch
        {
            RegisterPatientDto patient => new PatientEntity
            {
                AccountId = accountId,
                BirthDate = patient.BirthDate,
                InsuranceNumber = patient.InsuranceNumber
            },
            RegisterDoctorDto doctor => new DoctorEntity
            {
                AccountId = accountId,
                BirthDate = doctor.BirthDate,
                WorkStartDate = doctor.WorkStartDate,
                OfficeId = doctor.OfficeId,
                SpecializationId = doctor.SpecializationId
            },
            RegisterAdminDto admin => new AdminEntity
            {
                AccountId = accountId,
                BirthDate = admin.BirthDate,
                WorkStartDate = admin.WorkStartDate,
                OfficeId = admin.OfficeId
            },
            RegisterReceptionistDto receptionist => new ReceptionistEntity
            {
                AccountId = accountId,
                BirthDate = receptionist.BirthDate,
                WorkStartDate = receptionist.WorkStartDate,
                OfficeId = receptionist.OfficeId
            },
            _ => throw new ArgumentException("Unsupported registration role.")
        };
    }

    public static RegisteredUserDto ToRegisteredDto(this PersonEntity profile, AccountEntity account)
    {
        return profile switch
        {
            PatientEntity patient => new RegisteredPatientDto
            {
                Account = account.ToDto(),
                Patient = patient.ToDto()
            },
            DoctorEntity doctor => new RegisteredDoctorDto
            {
                Account = account.ToDto(),
                Doctor = doctor.ToDto()
            },
            AdminEntity admin => new RegisteredAdminDto
            {
                Account = account.ToDto(),
                Admin = admin.ToDto()
            },
            ReceptionistEntity receptionist => new RegisteredReceptionistDto
            {
                Account = account.ToDto(),
                Receptionist = receptionist.ToDto()
            },
            _ => throw new ArgumentException("Unsupported registration role.")
        };
    }
}
