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
            AccountId = dto.AccountId,
            BirthDate = dto.BirthDate,
            FirstName = dto.FirstName,
            LastName = dto.LastName,
            MiddleName = dto.MiddleName,
            InsuranceNumber = dto.InsuranceNumber,
        };
    }

    public static PatientEntity UpdateEntity(this UpdatePatientDto dto)
    {
        return new PatientEntity
        {
            Id = dto.Id,
            BirthDate = dto.BirthDate,
            FirstName = dto.FirstName,
            LastName = dto.LastName,
            MiddleName = dto.MiddleName,
            InsuranceNumber = dto.InsuranceNumber,
        };
    }
}
