namespace ProfilesApi.Domain.Entities;

public sealed class ReceptionistEntity : PersonEntity
{
    public int WorkExperience { get; set; }
    public Guid OfficeId { get; set; }
}