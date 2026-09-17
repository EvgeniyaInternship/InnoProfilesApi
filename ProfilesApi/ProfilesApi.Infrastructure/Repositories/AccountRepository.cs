using ProfilesApi.Domain.Entities;
using ProfilesApi.Infrastructure.Context;

namespace ProfilesApi.Infrastructure.Repositories;

public class AccountRepository(ProfilesDbContext context) : GenericRepository<AccountEntity>(context) { }