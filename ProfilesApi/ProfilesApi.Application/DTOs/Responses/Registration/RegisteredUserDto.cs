using ProfilesApi.Domain.Enum;
using System.Text.Json.Serialization;

namespace ProfilesApi.Application.DTOs.Responses.Registration;

[JsonPolymorphic(TypeDiscriminatorPropertyName = "role")]
[JsonDerivedType(typeof(RegisteredPatientDto), typeDiscriminator: nameof(AccountRole.Patient))]
[JsonDerivedType(typeof(RegisteredDoctorDto), typeDiscriminator: nameof(AccountRole.Doctor))]
[JsonDerivedType(typeof(RegisteredAdminDto), typeDiscriminator: nameof(AccountRole.Admin))]
[JsonDerivedType(typeof(RegisteredReceptionistDto), typeDiscriminator: nameof(AccountRole.Receptionist))]
public abstract record RegisteredUserDto
{
    public required AccountDto Account { get; init; }
}

public sealed record RegisteredPatientDto : RegisteredUserDto
{
    public required PatientDto Patient { get; init; }
}

public sealed record RegisteredDoctorDto : RegisteredUserDto
{
    public required DoctorDto Doctor { get; init; }
}

public sealed record RegisteredAdminDto : RegisteredUserDto
{
    public required AdminDto Admin { get; init; }
}

public sealed record RegisteredReceptionistDto : RegisteredUserDto
{
    public required ReceptionistDto Receptionist { get; init; }
}
