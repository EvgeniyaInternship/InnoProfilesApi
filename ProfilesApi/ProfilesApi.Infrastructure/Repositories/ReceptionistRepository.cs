using ProfilesApi.Domain.Entities;
using ProfilesApi.Infrastructure.Context;
using ProfilesApi.Infrastructure.Interfaces;

namespace ProfilesApi.Infrastructure.Repositories;

public class ReceptionistRepository : GenericRepository<ReceptionistEntity>, IReceptionistRepository
{
    public ReceptionistRepository(ProfilesDbContext context) : base(context) { }
}