using ProfilesApi.Domain.Enum;
using System.Text.Json.Serialization;

namespace ProfilesApi.Application.DTOs.Requests.Registration;

[JsonPolymorphic(TypeDiscriminatorPropertyName = "role")]
[JsonDerivedType(typeof(RegisterPatientDto), typeDiscriminator: nameof(AccountRole.Patient))]
[JsonDerivedType(typeof(RegisterDoctorDto), typeDiscriminator: nameof(AccountRole.Doctor))]
[JsonDerivedType(typeof(RegisterAdminDto), typeDiscriminator: nameof(AccountRole.Admin))]
[JsonDerivedType(typeof(RegisterReceptionistDto), typeDiscriminator: nameof(AccountRole.Receptionist))]
public abstract record RegisterUserDto
{
    public required string Email { get; init; }
    public required string Password { get; init; }
    public required string FirstName { get; init; }
    public required string LastName { get; init; }

    public string? MiddleName { get; init; }
    public required string PhoneNumber { get; init; }
    public required DateTime BirthDate { get; init; }
    public Guid? PhotoId { get; init; }
}

public sealed record RegisterPatientDto : RegisterUserDto
{
    public required string InsuranceNumber { get; init; }
}

public sealed record RegisterDoctorDto : RegisterUserDto
{
    public required DateTime WorkStartDate { get; init; }
    public required Guid OfficeId { get; init; }
    public required Guid SpecializationId { get; init; }
}

public sealed record RegisterAdminDto : RegisterUserDto
{
    public required DateTime WorkStartDate { get; init; }
    public required Guid OfficeId { get; init; }
}

public sealed record RegisterReceptionistDto : RegisterUserDto
{
    public required DateTime WorkStartDate { get; init; }
    public required Guid OfficeId { get; init; }
}
