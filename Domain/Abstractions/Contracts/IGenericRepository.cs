using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Abstractions.Contracts
{
    public interface IGenericRepository<TEntity> where TEntity : BaseEntity
    {
        Task AddAsync(TEntity entity);
        Task AddRangeAsync(IEnumerable<TEntity> entities);
        void UpdateInclude(TEntity entity, params Expression<Func<TEntity, object>>[] propertyExpressions);
        IQueryable<TEntity> GetAll();
        IQueryable<TEntity> GetAll(Expression<Func<TEntity, bool>> expression);
        Task<TEntity?> GetByIdAsync(Guid id);
        void Delete(TEntity entity);
        Task<bool> DoesEntityExistAsync(Guid id);
        Task<bool> AnyAsync(Expression<Func<TEntity, bool>> expression);
        Task<int> SaveChangesAsync(CancellationToken cancellationToken);

    }
}
