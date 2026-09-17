using ProfilesApi.Domain.Entities;
using ProfilesApi.Infrastructure.Context;

namespace ProfilesApi.Infrastructure.Repositories;

public class DoctorRepository(ProfilesDbContext context) : GenericRepository<DoctorEntity>(context) { }