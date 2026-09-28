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
            Id = Guid.NewGuid(),
            PhoneNumber = dto.PhoneNumber,
            Email = dto.Email,
            Role = dto.Role,
            PhotoId = dto.PhotoId,
            CreatedAt = DateTime.UtcNow,
            IsDeleted = false
        };
    }

    public static void UpdateEntity(this UpdateAccountDto dto, AccountEntity entity)
    {
        entity.PhoneNumber = dto.PhoneNumber;
        entity.Email = dto.Email;
        entity.Role = dto.Role;
        entity.PhotoId = dto.PhotoId;
        entity.UpdatedAt = DateTime.UtcNow;
    }

    public static IEnumerable<AccountDto> ToDtos(this IEnumerable<AccountEntity> entities)
    {
        return entities?.Select(e => e.ToDto()) ?? [];
    }
}
