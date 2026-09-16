namespace ProfilesApi.Domain.Entities;

public sealed class PatientEntity : PersonEntity
{
    public string InsuranceNumber { get; set; }
}