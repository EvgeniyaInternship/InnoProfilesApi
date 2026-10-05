using ProfilesApi.Domain.Entities;
using ProfilesApi.Domain.Interfaces;
using ProfilesApi.Infrastructure.Context;

namespace ProfilesApi.Infrastructure.Repositories;

public sealed class AccountRepository : GenericRepository<AccountEntity>, IAccountRepository
{
    public AccountRepository(ProfilesDbContext context) : base(context) { }
}