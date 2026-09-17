using ProfilesApi.Domain.Entities;
using ProfilesApi.Infrastructure.Context;

namespace ProfilesApi.Infrastructure.Repositories;

public class ReceptionistRepository(ProfilesDbContext context) : GenericRepository<ReceptionistEntity>(context) { }