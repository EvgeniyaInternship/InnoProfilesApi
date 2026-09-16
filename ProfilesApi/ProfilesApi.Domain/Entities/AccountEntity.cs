using ProfilesApi.Domain.Enum;
using ProfilesApi.Domain.Interfaces;

namespace ProfilesApi.Domain.Entities;

public sealed class AccountEntity : BaseEntity, ISoftDeletable
{
    public string PhoneNumber { get; set; }
    public string Email { get; set; }
    public string HashedPassword { get; set; }
    public AccountRole Role { get; set; }
    public Guid PhotoId { get; set; }
    public bool IsDeleted { get; set; }
    public DateTime? DeletedOnUtc { get; set; }
}