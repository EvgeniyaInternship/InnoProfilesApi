using Microsoft.EntityFrameworkCore.Storage;
using System.Data;

namespace ProfilesApi.Infrastructure.Interfaces;

public interface IUnitOfWork
{
    IAccountRepository Accounts { get; }
    IDoctorRepository Doctors { get; }
    IPatientRepository Patients { get; }
    IReceptionistRepository Receptionists { get; }
    IAdminRepository Admins { get; }

    int SaveChanges();
    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);

    Task<IDbContextTransaction> BeginTransactionAsync(
        IsolationLevel isolationLevel = IsolationLevel.ReadCommitted,
        CancellationToken cancellationToken = default);
}