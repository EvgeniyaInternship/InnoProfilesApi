using ProfilesApi.Domain.Interfaces;

namespace ProfilesApi.Domain.Entities;

public class PersonEntity : BaseEntity, ISoftDeletable
{
    public Guid AccountId { get; set; }
    public DateTime BirthDate { get; set; } = DateTime.UtcNow;
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string? MiddleName { get; set; }
    public bool IsDeleted { get; set; }
    public DateTime? DeletedOnUtc { get; set; }
}