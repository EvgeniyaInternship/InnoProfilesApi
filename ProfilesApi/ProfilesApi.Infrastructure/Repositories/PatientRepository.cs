using ProfilesApi.Domain.Entities;
using ProfilesApi.Infrastructure.Context;

namespace ProfilesApi.Infrastructure.Repositories;

public class PatientRepository(ProfilesDbContext context) : GenericRepository<PatientEntity>(context) { }