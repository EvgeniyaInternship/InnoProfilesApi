using ProfilesApi.Domain.Enum;

namespace ProfilesApi.Domain.Entities
{
    public class AccountEntity : BaseEntity
    {
        public string? PhoneNumber { get; set; }
        public string? Email { get; set; }
        public string? HashedPassword { get; set; }
        public AccountRole Role { get; set; }

        public Guid PhotoId { get; set; }
    }
}
