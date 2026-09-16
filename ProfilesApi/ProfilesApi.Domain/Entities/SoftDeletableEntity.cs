namespace ProfilesApi.Domain.Entities;

public class SoftDeletableEntity : BaseEntity
{
    public bool IsDeleted { get; set; } = false;
    public DateTime DeletedOnUtc { get; set; } = DateTime.UtcNow;
}
