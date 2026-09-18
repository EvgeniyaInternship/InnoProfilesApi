using ProfilesApi.Domain.Entities;
using ProfilesApi.Infrastructure.Context;
using ProfilesApi.Infrastructure.Interfaces;

namespace ProfilesApi.Infrastructure.Repositories;

public class PatientRepository : GenericRepository<PatientEntity>, IPatientRepository
{
    public PatientRepository(ProfilesDbContext context) : base(context) { }
}