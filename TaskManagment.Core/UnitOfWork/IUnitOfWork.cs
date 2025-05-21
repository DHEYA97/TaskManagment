using TaskManagment.Core.Entities;
using TaskManagment.Core.Interfaces.Repositories;

namespace TaskManagment.Core.UnitOfWork
{
    public interface IUnitOfWork : IAsyncDisposable
    {
        IGenericRepositories<TEntity> Repositories<TEntity>() where TEntity : BaseEntity;
        Task<int> CompleteAsync();
        Task BeginTransactionAsync();
        Task CommitTransactionAsync();
        Task RollbackTransactionAsync();
    }
}
