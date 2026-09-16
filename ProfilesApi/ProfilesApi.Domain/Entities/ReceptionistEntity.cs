namespace ProfilesApi.Domain.Entities;

public sealed class ReceptionistEntity : PersonEntity
{
    public DateTime WorkStartDate { get; set; }
    public Guid OfficeId { get; set; }
}