namespace ProfilesApi.Domain.Entities;

public sealed class AdminEntity : PersonEntity
{
    public DateTime WorkStartDate { get; set; }
    public Guid OfficeId { get; set; }
}