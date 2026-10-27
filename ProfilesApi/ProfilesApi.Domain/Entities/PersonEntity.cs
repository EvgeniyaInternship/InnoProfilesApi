namespace ProfilesApi.Domain.Entities;

public class PersonEntity : SoftDeletableEntity
{
    public Guid AccountId { get; init; }
    public DateTime BirthDate { get; set; }
}