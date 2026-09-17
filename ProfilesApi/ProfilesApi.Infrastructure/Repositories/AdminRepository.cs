using ProfilesApi.Domain.Entities;
using ProfilesApi.Infrastructure.Context;

namespace ProfilesApi.Infrastructure.Repositories;

public class AdminRepository(ProfilesDbContext context) : GenericRepository<AdminEntity>(context) {}