using ProfilesApi.Application.DTOs.Filters;
using ProfilesApi.Application.DTOs.Requests.CreateRequests;
using ProfilesApi.Application.DTOs.Requests.UpdateRequests;
using ProfilesApi.Application.DTOs.Responses;
using ProfilesApi.Application.Interfaces;
using ProfilesApi.Application.Mappings;
using ProfilesApi.Domain.Entities;
using ProfilesApi.Domain.Interfaces;
using System.Linq.Expressions;

namespace ProfilesApi.Application.Services;

public class AdminService(IUnitOfWork unitOfWork) : IAdminService
{
    private static Expression<Func<AdminEntity, bool>> BuildFilterExpression(AdminFilterDto? filter)
    {
        if (filter is null)
            return x => true;

        var searchTerm = filter.SearchTerm?.Trim();

        return x => (filter.OfficeId == null || x.OfficeId == filter.OfficeId)
                 && (filter.WorkStartDateFrom == null || x.WorkStartDate >= filter.WorkStartDateFrom)
                 && (string.IsNullOrWhiteSpace(filter.FirstName) || x.FirstName.Contains(filter.FirstName))
                 && (string.IsNullOrWhiteSpace(filter.LastName) || x.LastName.Contains(filter.LastName))
                 && (string.IsNullOrWhiteSpace(filter.MiddleName) || (x.MiddleName != null && x.MiddleName.Contains(filter.MiddleName)))
                 && (string.IsNullOrWhiteSpace(searchTerm) ||
                     x.FirstName.Contains(searchTerm) ||
                     x.LastName.Contains(searchTerm) ||
                     (x.MiddleName != null && x.MiddleName.Contains(searchTerm)));
    }

    public async Task<AdminDto> GetAdminByIdAsync(Guid id, CancellationToken ct = default)
    {
        var admin = await unitOfWork.Admins.GetAsync(x => x.Id == id, cancellationToken: ct);
        if (admin is null)
            throw new KeyNotFoundException($"Admin with ID {id} was not found.");

        return admin.ToDto();
    }

    public async Task<IEnumerable<AdminDto>> GetAdminsAsync(AdminFilterDto? filter = null, CancellationToken ct = default)
    {
        var filterExpression = BuildFilterExpression(filter);

        var admins = await unitOfWork.Admins.GetAllAsync(filterExpression, cancellationToken: ct);

        return admins.Select(admin => admin.ToDto());
    }

    public async Task<AdminDto> CreateAdminAsync(CreateAdminDto dto, CancellationToken ct = default)
    {
        var admin = dto.ToEntity();

        unitOfWork.Admins.Add(admin);
        await unitOfWork.SaveChangesAsync(ct);

        return admin.ToDto();
    }

    public async Task UpdateAdminAsync(UpdateAdminDto dto, CancellationToken ct = default)
    {
        using var transaction = await unitOfWork.BeginTransactionAsync(cancellationToken: ct);
        try
        {
            var admin = await unitOfWork.Admins.GetAsync(x => x.Id == dto.Id, cancellationToken: ct);
            if (admin is null)
                throw new KeyNotFoundException($"Admin with ID {dto.Id} was not found.");

            unitOfWork.Admins.Update(dto.UpdateEntity());
            await unitOfWork.SaveChangesAsync(ct);
            await transaction.CommitAsync(ct);
        }
        catch
        {
            await transaction.RollbackAsync(cancellationToken: ct);
            throw new Exception("An error occurred while updating admins.");
        }
    }

    public async Task RemoveAdminAsync(Guid id, CancellationToken ct = default)
    {
        using var transaction = await unitOfWork.BeginTransactionAsync(cancellationToken: ct);
        try
        {
            var admin = await unitOfWork.Admins.GetAsync(x => x.Id == id, true, cancellationToken: ct);
            if (admin is null)
                throw new KeyNotFoundException($"Admin with ID {id} was not found.");

            unitOfWork.Admins.Remove(admin);
            await unitOfWork.SaveChangesAsync(ct);
            await transaction.CommitAsync(ct);
        }
        catch
        {
            await transaction.RollbackAsync(cancellationToken: ct);
            throw new Exception("An error occurred while removing admins.");
        }
    }

    public async Task<IEnumerable<AdminDto>> CreateRangeAsync(IEnumerable<CreateAdminDto> dtos, CancellationToken ct = default)
    {
        if (!dtos.Any()) return [];

        var entities = dtos.Select(dto => dto.ToEntity());

        unitOfWork.Admins.AddRange(entities);
        await unitOfWork.SaveChangesAsync(ct);

        return entities.Select(admin => admin.ToDto());
    }

    public async Task UpdateRangeAsync(IEnumerable<UpdateAdminDto> dtos, CancellationToken ct = default)
    {
        var uniqueDtos = dtos.DistinctBy(d => d.Id).ToList();
        if (uniqueDtos.Count == 0) return;

        using var transaction = await unitOfWork.BeginTransactionAsync(cancellationToken: ct);
        try
        {
            var ids = uniqueDtos.Select(d => d.Id).ToList();

            var existingAdmins = (await unitOfWork.Admins.GetAllAsync(x => ids.Contains(x.Id), cancellationToken: ct)).ToList();
            if (existingAdmins.Count != uniqueDtos.Count)
                throw new KeyNotFoundException("One or more admins were not found.");

            var entities = uniqueDtos.Select(d => d.UpdateEntity());

            unitOfWork.Admins.UpdateRange(entities);
            await unitOfWork.SaveChangesAsync(ct);
            await transaction.CommitAsync(ct);
        }
        catch
        {
            await transaction.RollbackAsync(ct);
            throw new Exception("An error occurred while updating admins.");
        }
    }

    public async Task RemoveRangeAsync(IEnumerable<Guid> ids, CancellationToken ct = default)
    {
        var distinctIds = ids.Distinct().ToList();
        if (distinctIds.Count == 0) return;

        using var transaction = await unitOfWork.BeginTransactionAsync(cancellationToken: ct);
        try
        {
            var existingAdmins = (await unitOfWork.Admins.GetAllAsync(x => distinctIds.Contains(x.Id), cancellationToken: ct)).ToList();

            if (existingAdmins.Count != distinctIds.Count)
                throw new KeyNotFoundException("One or more admins were not found.");

            unitOfWork.Admins.RemoveRange(existingAdmins);
            await unitOfWork.SaveChangesAsync(ct);
            await transaction.CommitAsync(ct);
        }
        catch
        {
            await transaction.RollbackAsync(ct);
            throw new Exception("An error occurred while removing admins.");
        }
    }
}
