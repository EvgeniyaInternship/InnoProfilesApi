namespace ProfilesApi.Domain.Entities
{
    public abstract class PersonEntity : BaseEntity
    {
        public Guid AccountId { get; set; }

        public DateTime BirthDate { get; set; } = DateTime.UtcNow;
        public string? Name { get; set; }
        public string? LastName { get; set; }
        public string? MiddleName { get; set; }
    }
}
