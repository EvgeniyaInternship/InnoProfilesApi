using ProfilesApi.Application.DTOs.Requests.CreateRequests;
using ProfilesApi.Application.DTOs.Requests.UpdateRequests;
using ProfilesApi.Application.DTOs.Responses;
using ProfilesApi.Domain.Entities;

namespace ProfilesApi.Application.Mappings;

public static class AdminMappingExtensions
{
    public static AdminDto ToDto(this AdminEntity entity)
    {
        return new AdminDto(
            entity.Id,
            entity.CreatedAt,
            entity.UpdatedAt,
            entity.AccountId,
            entity.BirthDate,
            entity.WorkStartDate,
            entity.OfficeId
        );
    }

    public static AdminEntity ToEntity(this CreateAdminDto dto)
    {
        return new AdminEntity
        {
            AccountId = dto.AccountId,
            BirthDate = dto.BirthDate,
            WorkStartDate = dto.WorkStartDate,
            OfficeId = dto.OfficeId,
        };
    }

    public static AdminEntity UpdateEntity(this UpdateAdminDto dto)
    {
        return new AdminEntity
        {
            Id = dto.Id,
            BirthDate = dto.BirthDate,
            WorkStartDate = dto.WorkStartDate,
            OfficeId = dto.OfficeId,
        };
    }
}
