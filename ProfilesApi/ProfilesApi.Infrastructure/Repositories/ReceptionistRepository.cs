using ProfilesApi.Domain.Entities;
using ProfilesApi.Domain.Interfaces;
using ProfilesApi.Infrastructure.Context;

namespace ProfilesApi.Infrastructure.Repositories;

public sealed class ReceptionistRepository : GenericRepository<ReceptionistEntity>, IReceptionistRepository
{
    public ReceptionistRepository(ProfilesDbContext context) : base(context) { }
}