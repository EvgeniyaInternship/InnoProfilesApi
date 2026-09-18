using ProfilesApi.Domain.Entities;
using ProfilesApi.Domain.Interfaces;

namespace ProfilesApi.Infrastructure.Interfaces;

public interface IAdminRepository : IGenericRepository<AdminEntity> { }
