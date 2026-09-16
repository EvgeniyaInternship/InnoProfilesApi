namespace ProfilesApi.Domain.Entities;

public class BaseEntity : SoftDeletableEntity
{
    public Guid Id { get; init; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
    public string CreatedBy { get; set; }
    public string UpdatedBy { get; set; }
}