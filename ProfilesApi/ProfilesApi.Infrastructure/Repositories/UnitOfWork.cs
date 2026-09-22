using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using ProfilesApi.Infrastructure.Context;
using ProfilesApi.Infrastructure.Interfaces;
using System.Data;

namespace ProfilesApi.Infrastructure.Repositories;

public class UnitOfWork : IUnitOfWork
{
    private readonly ProfilesDbContext _context;
    private IDbContextTransaction? _currentTransaction;

    public IAccountRepository Accounts { get; }
    public IDoctorRepository Doctors { get; }
    public IPatientRepository Patients { get; }
    public IReceptionistRepository Receptionists { get; }
    public IAdminRepository Admins { get; }

    public UnitOfWork(
        ProfilesDbContext context,
        IAccountRepository accounts,
        IAdminRepository admins,
        IDoctorRepository doctors,
        IReceptionistRepository receptionists,
        IPatientRepository patients)
    {
        _context = context;
        Accounts = accounts;
        Admins = admins;
        Doctors = doctors;
        Patients = patients;
        Receptionists = receptionists;
    }

    public async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        return await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task BeginTransactionAsync(IsolationLevel isolationLevel = IsolationLevel.ReadCommitted, CancellationToken cancellationToken = default)
    {
        if (_currentTransaction is not null)
        {
            return;
        }

        _currentTransaction = await _context.Database.BeginTransactionAsync(isolationLevel, cancellationToken);
    }

    public async Task CommitTransactionAsync(CancellationToken cancellationToken = default)
    {
        try
        {
            await SaveChangesAsync(cancellationToken);

            if (_currentTransaction is not null)
            {
                await _currentTransaction.CommitAsync(cancellationToken);
            }
        }
        catch
        {
            await RollbackTransactionAsync(cancellationToken);
            throw;
        }
        finally
        {
            _currentTransaction?.Dispose();
            _currentTransaction = null;
        }
    }

    public async Task RollbackTransactionAsync(CancellationToken cancellationToken = default)
    {
        try
        {
            if (_currentTransaction is not null)
            {
                await _currentTransaction.RollbackAsync(cancellationToken);
            }
        }
        finally
        {
            _currentTransaction?.Dispose();
            _currentTransaction = null;
        }
    }
}
