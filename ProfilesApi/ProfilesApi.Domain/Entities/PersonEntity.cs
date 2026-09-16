namespace ProfilesApi.Domain.Entities;

public class PersonEntity : SoftDeletableEntity
{
    public Guid AccountId { get; init; }
    public DateTime BirthDate { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string? MiddleName { get; set; }
}