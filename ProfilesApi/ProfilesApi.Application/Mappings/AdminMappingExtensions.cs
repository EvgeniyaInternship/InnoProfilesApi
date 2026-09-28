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
            entity.IsDeleted,
            entity.AccountId,
            entity.BirthDate,
            entity.FirstName,
            entity.LastName,
            entity.MiddleName,
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
            FirstName = dto.FirstName,
            LastName = dto.LastName,
            MiddleName = dto.MiddleName,
            WorkStartDate = dto.WorkStartDate,
            OfficeId = dto.OfficeId,
        };
    }

    public static void UpdateEntity(this UpdateAdminDto dto, AdminEntity entity)
    {
        entity.BirthDate = dto.BirthDate;
        entity.FirstName = dto.FirstName;
        entity.LastName = dto.LastName;
        entity.MiddleName = dto.MiddleName;
        entity.WorkStartDate = dto.WorkStartDate;
    }
}
