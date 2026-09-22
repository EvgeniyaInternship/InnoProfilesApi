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

    public int SaveChanges()
    {
        return _context.SaveChanges();
    }

    public async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        return await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task<IDbContextTransaction> BeginTransactionAsync(
        IsolationLevel isolationLevel = IsolationLevel.ReadCommitted,
        CancellationToken cancellationToken = default)
    {
        return await _context.Database.BeginTransactionAsync(isolationLevel, cancellationToken);
    }
}
