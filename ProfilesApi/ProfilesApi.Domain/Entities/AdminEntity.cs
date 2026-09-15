namespace ProfilesApi.Domain.Entities
{
    public class AdminEntity : PersonEntity
    {
        public int WorkExperience { get; set; }

        public Guid OfficeId { get; set; }
    }
}
