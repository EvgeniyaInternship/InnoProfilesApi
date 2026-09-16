using ProfilesApi.Domain.Enum;

namespace ProfilesApi.Domain.Entities;

public sealed class AccountEntity : SoftDeletableEntity
{
    public string PhoneNumber { get; set; }
    public string Email { get; set; }
    public string HashedPassword { get; set; }
    public AccountRole Role { get; set; } = AccountRole.Patient;
    public Guid? PhotoId { get; set; }
}