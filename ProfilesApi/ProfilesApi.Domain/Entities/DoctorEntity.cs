namespace ProfilesApi.Domain.Entities
{
    public class DoctorEntity : PersonEntity
    {
        public int WorkExperience { get; set; }
        public string? JobTitle { get; set; }

        public Guid OfficeId { get; set; }
        public Guid SpecializationId { get; set; }
        public Guid ServiceId { get; set; }
    }
}
