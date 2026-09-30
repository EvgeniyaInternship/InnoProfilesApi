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
            AccountId = dto.AccountId,
            BirthDate = dto.BirthDate,
            FirstName = dto.FirstName,
            LastName = dto.LastName,
            MiddleName = dto.MiddleName,
            WorkStartDate = dto.WorkStartDate,
            OfficeId = dto.OfficeId,
            SpecializationId = dto.SpecializationId,
        };
    }

    public static DoctorEntity UpdateEntity(this UpdateDoctorDto dto)
    {
        return new DoctorEntity
        {
            Id = dto.Id,
            BirthDate = dto.BirthDate,
            FirstName = dto.FirstName,
            LastName = dto.LastName,
            MiddleName = dto.MiddleName,
            WorkStartDate = dto.WorkStartDate,
            OfficeId = dto.OfficeId,
            SpecializationId = dto.SpecializationId
        };          
    }
}
