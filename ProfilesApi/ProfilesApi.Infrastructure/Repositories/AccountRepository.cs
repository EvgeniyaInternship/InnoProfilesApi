using ProfilesApi.Domain.Entities;
using ProfilesApi.Infrastructure.Context;
using ProfilesApi.Infrastructure.Interfaces;

namespace ProfilesApi.Infrastructure.Repositories;

public class AccountRepository : GenericRepository<AccountEntity>, IAccountRepository
{
    public AccountRepository(ProfilesDbContext context) : base(context) { }
}