using ProfilesApi.Application.DTOs.Requests.CreateRequests;
using ProfilesApi.Application.DTOs.Requests.UpdateRequests;
using ProfilesApi.Application.DTOs.Responses;
using ProfilesApi.Domain.Entities;

namespace ProfilesApi.Application.Mappings;

public static class AccountMappingExtensions
{
    public static AccountDto ToDto(this AccountEntity entity)
    {
        return new AccountDto(
            entity.Id,
            entity.PhoneNumber,
            entity.Email,
            entity.Role,
            entity.PhotoId,
            entity.CreatedAt,
            entity.UpdatedAt
        );
    }

    public static AccountEntity ToEntity(this CreateAccountDto dto)
    {
        return new AccountEntity
        {
            PhoneNumber = dto.PhoneNumber,
            Email = dto.Email,
            Role = dto.Role,
            PhotoId = dto.PhotoId,
        };
    }

    public static void UpdateEntity(this UpdateAccountDto dto, AccountEntity entity)
    {
        entity.PhoneNumber = dto.PhoneNumber;
        entity.Email = dto.Email;
        entity.Role = dto.Role;
        entity.PhotoId = dto.PhotoId;
    }
}
