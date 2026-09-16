namespace ProfilesApi.Domain.Entities;

public sealed class AdminEntity : PersonEntity
{
    public int WorkExperience { get; set; }
    public Guid OfficeId { get; set; }
}