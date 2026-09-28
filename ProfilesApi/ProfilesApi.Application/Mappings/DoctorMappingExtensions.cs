using ProfilesApi.Application.DTOs.Requests.CreateRequests;
using ProfilesApi.Application.DTOs.Requests.UpdateRequests;
using ProfilesApi.Application.DTOs.Responses;
using ProfilesApi.Domain.Entities;

namespace ProfilesApi.Application.Mappings;

public static class DoctorMappingExtensions
{
    public static DoctorDto ToDto(this DoctorEntity entity)
    {
        return new DoctorDto(
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
            entity.OfficeId,
            entity.SpecializationId
        );
    }

    public static DoctorEntity ToEntity(this CreateDoctorDto dto)
    {
        return new DoctorEntity
        {
            Id = Guid.NewGuid(),
            AccountId = dto.AccountId,
            BirthDate = dto.BirthDate,
            FirstName = dto.FirstName,
            LastName = dto.LastName,
            MiddleName = dto.MiddleName,
            WorkStartDate = dto.WorkStartDate,
            OfficeId = dto.OfficeId,
            SpecializationId = dto.SpecializationId,
            CreatedAt = DateTime.UtcNow,
            IsDeleted = false
        };
    }

    public static void UpdateEntity(this UpdateDoctorDto dto, DoctorEntity entity)
    {
        entity.BirthDate = dto.BirthDate;
        entity.FirstName = dto.FirstName;
        entity.LastName = dto.LastName;
        entity.MiddleName = dto.MiddleName;
        entity.WorkStartDate = dto.WorkStartDate;
        entity.OfficeId = dto.OfficeId;
        entity.SpecializationId = dto.SpecializationId;
        entity.UpdatedAt = DateTime.UtcNow;
    }

    public static IEnumerable<DoctorDto> ToDtos(this IEnumerable<DoctorEntity> entities)
    {
        return entities?.Select(e => e.ToDto()) ?? [];
    }
}
