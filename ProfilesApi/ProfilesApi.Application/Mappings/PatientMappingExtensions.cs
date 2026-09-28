using ProfilesApi.Application.DTOs.Requests.CreateRequests;
using ProfilesApi.Application.DTOs.Requests.UpdateRequests;
using ProfilesApi.Application.DTOs.Responses;
using ProfilesApi.Domain.Entities;

namespace ProfilesApi.Application.Mappings;

public static class PatientMappingExtensions
{
    public static PatientDto ToDto(this PatientEntity entity)
    {
        return new PatientDto(
            entity.Id,
            entity.CreatedAt,
            entity.UpdatedAt,
            entity.IsDeleted,
            entity.AccountId,
            entity.BirthDate,
            entity.FirstName,
            entity.LastName,
            entity.MiddleName,
            entity.InsuranceNumber
        );
    }

    public static PatientEntity ToEntity(this CreatePatientDto dto)
    {
        return new PatientEntity
        {
            Id = Guid.NewGuid(),
            AccountId = dto.AccountId,
            BirthDate = dto.BirthDate,
            FirstName = dto.FirstName,
            LastName = dto.LastName,
            MiddleName = dto.MiddleName,
            InsuranceNumber = dto.InsuranceNumber,
            CreatedAt = DateTime.UtcNow,
            IsDeleted = false
        };
    }

    public static void UpdateEntity(this UpdatePatientDto dto, PatientEntity entity)
    {
        entity.BirthDate = dto.BirthDate;
        entity.FirstName = dto.FirstName;
        entity.LastName = dto.LastName;
        entity.MiddleName = dto.MiddleName;
        entity.InsuranceNumber = dto.InsuranceNumber;
        entity.UpdatedAt = DateTime.UtcNow; 
    }

    public static IEnumerable<PatientDto> ToDtos(this IEnumerable<PatientEntity> entities)
    {
        return entities?.Select(e => e.ToDto()) ?? [];
    }
}
