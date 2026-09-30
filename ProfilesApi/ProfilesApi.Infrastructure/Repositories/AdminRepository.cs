using ProfilesApi.Domain.Entities;
using ProfilesApi.Domain.Interfaces;
using ProfilesApi.Infrastructure.Context;

namespace ProfilesApi.Infrastructure.Repositories;

public class AdminRepository : GenericRepository<AdminEntity>, IAdminRepository
{
    public AdminRepository(ProfilesDbContext context) : base(context) { }
}