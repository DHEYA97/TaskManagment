using TaskManagment.Core.Entities;
using TaskManagment.Core.Specification;

namespace TaskManagment.Core.Interfaces.Repositories
{
    public interface IGenericRepositories<T> where T : BaseEntity
    {
        Task<T?> GetByIdAsync(int Id);
        Task<IEnumerable<T?>> GetAllAsync();

        Task<T?> GetByIdWithSpecificationAsync(ISpecification<T> specification);
        Task<IEnumerable<T?>> GetAllWithSpecificationAsync(ISpecification<T> specification);
        Task<int> GetCountAsync(ISpecification<T> specification);
        Task<T> AddAsync(T entity, bool saveChanges = false);
        Task AddRangeAsync(IList<T> entities);
        void Delete(T entity);
        void DeleteRange(IList<T> entities);
        void ToggleStatus(T entity);
        T Update(T entity);
        IQueryable<T> ApplaySpecification(ISpecification<T> specification);
        IQueryable<T> Table { get; }

    }
}
