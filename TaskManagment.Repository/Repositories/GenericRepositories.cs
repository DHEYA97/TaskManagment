using Microsoft.EntityFrameworkCore;
using TaskManagment.Core.Entities;
using TaskManagment.Core.Interfaces;
using TaskManagment.Core.Interfaces.Repositories;
using TaskManagment.Core.Specification;
using TaskManagment.Repository.Data;
using TaskManagment.Repository.Specification;

namespace TaskManagment.Repository.Repositories
{
    public class GenericRepositories<T>(ApplicationDbContext context, IDateTimeService dateTimeService) : IGenericRepositories<T> where T : BaseEntity
    {
        private readonly ApplicationDbContext _context = context;
        private readonly IDateTimeService _dateTimeService = dateTimeService;

        public IQueryable<T> ApplaySpecification(ISpecification<T> specification)
        {
            return SpecificationEvaluator<T>.GetQuery(_context.Set<T>(), specification);
        }
        public async Task<IEnumerable<T?>> GetAllAsync()
        {
            return await _context.Set<T>().ToListAsync();
        }
        public async Task<T?> GetByIdAsync(int Id)
        {
            return await _context.Set<T>().FindAsync(Id);
        }
        public async Task<IEnumerable<T?>> GetAllWithSpecificationAsync(ISpecification<T> specification)
        {
            return await ApplaySpecification(specification).ToListAsync();
        }
        public async Task<T?> GetByIdWithSpecificationAsync(ISpecification<T> specification)
        {
            return await ApplaySpecification(specification).FirstOrDefaultAsync();
        }
        public async Task<int> GetCountAsync(ISpecification<T> specification)
        {
            return await ApplaySpecification(specification).CountAsync();
        }

        public async Task<T> AddAsync(T entity, bool saveChanges = false)
        {
            if (entity == null)
                throw new ArgumentNullException(nameof(entity));

            entity.CreatedOn = _dateTimeService.GetNowKsa();

            var entry = await _context.Set<T>().AddAsync(entity);
            if (saveChanges)
                await _context.SaveChangesAsync();

            return entry.Entity;
        }
        public async Task AddRangeAsync(IList<T> entities)
        {
            if (entities == null || entities?.Count <= 0)
                throw new ArgumentNullException(nameof(entities));
            foreach (var entity in entities)
            {
                entity.CreatedOn = _dateTimeService.GetNowKsa();
            }
            await _context.Set<T>().AddRangeAsync(entities);
        }

        public void DeleteRange(IList<T> entities)
        {
            if (entities == null || entities?.Count <= 0)
                throw new ArgumentNullException(nameof(entities));
            foreach (var entity in entities)
            {
                Delete(entity);
            }
        }

        public void Delete(T entity)
        {
            switch (entity)
            {
                case null:
                    throw new ArgumentNullException(nameof(entity));

                case ISoftDelete softDeletedEntity:
                    softDeletedEntity.IsDeleted = true;
                    Update(entity);
                    break;

                default:
                    _context.Set<T>().Remove(entity);
                    break;
            }
        }
        public void ToggleStatus(T entity)
        {
            switch (entity)
            {
                case null:
                    throw new ArgumentNullException(nameof(entity));
                case ISoftDelete softDeletedEntity:
                    softDeletedEntity.IsDeleted = !softDeletedEntity.IsDeleted;
                    Update(entity);
                    break;
                default:
                    break;
            }
        }

        public T Update(T entity)
        {
            if (entity == null)
                throw new ArgumentNullException(nameof(entity));
            entity.UpdatedOn = _dateTimeService.GetNowKsa();
            _context.Set<T>().Update(entity);
            return entity;
        }
        public virtual IQueryable<T> Table => _context.Set<T>();
    }
}
