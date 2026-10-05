using ProfilesApi.Application.DTOs.Requests.CreateRequests;
using ProfilesApi.Application.DTOs.Requests.UpdateRequests;
using ProfilesApi.Application.DTOs.Responses;
using ProfilesApi.Domain.Entities;

namespace ProfilesApi.Application.Mappings;

public static class ReceptionistMappingExtensions
{
    public static ReceptionistDto ToDto(this ReceptionistEntity entity)
    {
        return new ReceptionistDto(
            entity.Id,
            entity.CreatedAt,
            entity.UpdatedAt,
            entity.AccountId,
            entity.BirthDate,
            entity.FirstName,
            entity.LastName,
            entity.MiddleName,
            entity.WorkStartDate,
            entity.OfficeId
        );
    }

    public static ReceptionistEntity ToEntity(this CreateReceptionistDto dto)
    {
        return new ReceptionistEntity
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

    public static ReceptionistEntity UpdateEntity(this UpdateReceptionistDto dto)
    {
        return new ReceptionistEntity
        {
            Id = dto.Id,
            BirthDate = dto.BirthDate,
            FirstName = dto.FirstName,
            LastName = dto.LastName,
            MiddleName = dto.MiddleName,
            WorkStartDate = dto.WorkStartDate,
            OfficeId = dto.OfficeId,
        };
    }
}
