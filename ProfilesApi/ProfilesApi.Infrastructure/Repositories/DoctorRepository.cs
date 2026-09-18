using ProfilesApi.Domain.Entities;
using ProfilesApi.Infrastructure.Context;
using ProfilesApi.Infrastructure.Interfaces;

namespace ProfilesApi.Infrastructure.Repositories;

public class DoctorRepository : GenericRepository<DoctorEntity>, IDoctorRepository
{
    public DoctorRepository(ProfilesDbContext context) : base(context) { }
}