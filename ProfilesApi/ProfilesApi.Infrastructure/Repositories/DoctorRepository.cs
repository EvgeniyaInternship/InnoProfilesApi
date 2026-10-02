using ProfilesApi.Domain.Entities;
using ProfilesApi.Domain.Interfaces;
using ProfilesApi.Infrastructure.Context;

namespace ProfilesApi.Infrastructure.Repositories;

public class DoctorRepository : GenericRepository<DoctorEntity>, IDoctorRepository
{
    public DoctorRepository(ProfilesDbContext context) : base(context) { }
}