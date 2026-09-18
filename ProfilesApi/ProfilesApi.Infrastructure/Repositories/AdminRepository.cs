using ProfilesApi.Domain.Entities;
using ProfilesApi.Infrastructure.Context;
using ProfilesApi.Infrastructure.Interfaces;

namespace ProfilesApi.Infrastructure.Repositories;

public class AdminRepository : GenericRepository<AdminEntity>, IAdminRepository
{
    public AdminRepository(ProfilesDbContext context) : base(context) { }
}