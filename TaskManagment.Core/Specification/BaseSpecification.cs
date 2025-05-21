using Microsoft.EntityFrameworkCore.Query;
using System.Linq.Expressions;

namespace TaskManagment.Core.Specification
{
    public class BaseSpecification<T> : ISpecification<T> where T : class
    {
        public Expression<Func<T, bool>> Criteria { get; set; } = null;
        public List<Expression<Func<T, object>>> Includes { get; set; } = new List<Expression<Func<T, object>>>();
        public List<Func<IQueryable<T>, IIncludableQueryable<T, object>>> ThenIncludes { get; set; } = new List<Func<IQueryable<T>, IIncludableQueryable<T, object>>>();

        public Expression<Func<T, object>> OrderByAsc { get; set; } = null;
        public Expression<Func<T, object>> OrderByDes { get; set; } = null;
        public int? Skip { get; set; }
        public int? Take { get; set; }
        public bool IsEnablePageSize { get; set; }

        public BaseSpecification()
        {

        }
        public BaseSpecification(Expression<Func<T, bool>> newCriteria)
        {
            Criteria = newCriteria;
        }
        public void AddOrderByAsc(Expression<Func<T, object>> orderByAsc)
        {
            OrderByAsc = orderByAsc;
        }
        public void AddOrderByDes(Expression<Func<T, object>> orderByDes)
        {
            OrderByDes = orderByDes;
        }
        public void ApplayPagination(int? skip, int? take)
        {
            Skip = skip;
            Take = take;
            IsEnablePageSize = true;
        }
    }
}
