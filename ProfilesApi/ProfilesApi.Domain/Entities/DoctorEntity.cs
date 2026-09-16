namespace ProfilesApi.Domain.Entities;

public sealed class DoctorEntity : PersonEntity
{
    public DateTime WorkStartDate { get; set; }
    public Guid OfficeId { get; set; }
    public Guid SpecializationId { get; set; }
}