using Microsoft.EntityFrameworkCore.Storage;
using TaskManagment.Core.Entities;
using TaskManagment.Core.Interfaces;
using TaskManagment.Core.Interfaces.Repositories;
using TaskManagment.Core.UnitOfWork;
using TaskManagment.Repository.Data;
using TaskManagment.Repository.Repositories;

namespace TaskManagment.Repository.UnitOfWork
{
    public class UnitOfWork : IUnitOfWork, IAsyncDisposable
    {
        private readonly ApplicationDbContext _applicationDbContext;
        private IDbContextTransaction _transaction;
        private readonly Dictionary<Type, object> _repositoriesHashSet = new();
        private readonly IDateTimeService _dateTimeService;
        public UnitOfWork(ApplicationDbContext applicationDbContext, IDateTimeService dateTimeService)
        {
            _applicationDbContext = applicationDbContext;
            _dateTimeService = dateTimeService;
        }

        public async Task<int> CompleteAsync()
        {
            return await _applicationDbContext.SaveChangesAsync();
        }

        public async ValueTask DisposeAsync()
        {
            if (_transaction is not null)
            {
                await _transaction.DisposeAsync();
                _transaction = null;
            }
            await _applicationDbContext.DisposeAsync();
        }

        public IGenericRepositories<TEntity> Repositories<TEntity>() where TEntity : BaseEntity
        {
            var type = typeof(TEntity);
            if (!_repositoriesHashSet.TryGetValue(type, out var repo))
            {
                repo = new GenericRepositories<TEntity>(_applicationDbContext, _dateTimeService);
                _repositoriesHashSet[type] = repo;
            }
            return (IGenericRepositories<TEntity>)repo;
        }

        public async Task BeginTransactionAsync()
        {
            if (_transaction is null)
            {
                _transaction = await _applicationDbContext.Database.BeginTransactionAsync();
            }
        }

        public async Task CommitTransactionAsync()
        {
            if (_transaction is not null)
            {
                await _transaction.CommitAsync();
                await _transaction.DisposeAsync();
                _transaction = null;
            }
        }

        public async Task RollbackTransactionAsync()
        {
            if (_transaction is not null)
            {
                await _transaction.RollbackAsync();
                await _transaction.DisposeAsync();
                _transaction = null;
            }
        }
    }
}
