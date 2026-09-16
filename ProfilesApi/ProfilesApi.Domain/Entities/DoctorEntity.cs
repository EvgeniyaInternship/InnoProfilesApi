namespace ProfilesApi.Domain.Entities;

public sealed class DoctorEntity : PersonEntity
{
    public int WorkExperience { get; set; }
    public Guid OfficeId { get; set; }
    public Guid SpecializationId { get; set; }
    public Guid ServiceId { get; set; }
}