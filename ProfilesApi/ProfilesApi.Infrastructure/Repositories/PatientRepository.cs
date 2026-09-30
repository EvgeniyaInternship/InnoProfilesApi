using ProfilesApi.Domain.Entities;
using ProfilesApi.Domain.Interfaces;
using ProfilesApi.Infrastructure.Context;

namespace ProfilesApi.Infrastructure.Repositories;

public class PatientRepository : GenericRepository<PatientEntity>, IPatientRepository
{
    public PatientRepository(ProfilesDbContext context) : base(context) { }
}