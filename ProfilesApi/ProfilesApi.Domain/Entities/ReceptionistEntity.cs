namespace ProfilesApi.Domain.Entities
{
    public class ReceptionistEntity : PersonEntity
    {
        public int WorkExperience { get; set; }

        public Guid OfficeId { get; set; }
    }
}
