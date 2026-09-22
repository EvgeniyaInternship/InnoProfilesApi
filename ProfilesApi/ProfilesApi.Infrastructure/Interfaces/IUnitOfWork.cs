using System.Data;

namespace ProfilesApi.Infrastructure.Interfaces;

public interface IUnitOfWork
{
    IAccountRepository Accounts { get; }
    IDoctorRepository Doctors { get; }
    IPatientRepository Patients { get; }
    IReceptionistRepository Receptionists { get; }
    IAdminRepository Admins { get; }

    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);

    Task BeginTransactionAsync(IsolationLevel isolationLevel = IsolationLevel.ReadCommitted, CancellationToken cancellationToken = default);
    Task CommitTransactionAsync(CancellationToken cancellationToken = default);
    Task RollbackTransactionAsync(CancellationToken cancellationToken = default);
}