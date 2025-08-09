using Domain.Abstractions.Contracts;
using Domain.Entities;
using Infrastructure.Database;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Data
{
    public class GenericRepository<TEntity> : IGenericRepository<TEntity> where TEntity : BaseEntity
    {
        private readonly ApplicationDbContext _appDbContext;
        private readonly DbSet<TEntity> _dbSet;

        public GenericRepository(ApplicationDbContext appDbContext)
        {
            _appDbContext = appDbContext;
            _dbSet = appDbContext.Set<TEntity>();
        }

        public async Task<TEntity> AddAsync(TEntity entity)
        {

            await _dbSet.AddAsync(entity);
            return entity;
        }

        public async Task AddRangeAsync(IEnumerable<TEntity> entities)
        {
         
            await _dbSet.AddRangeAsync(entities);
        }



        public void Delete(TEntity entity)
        {
            entity.IsDeleted = true;
            entity.DeletedAt = DateTime.UtcNow;
            //UpdateInclude(entity,);
        }

        public async Task<bool> DoesEntityExistAsync(Guid id) => await _dbSet.AnyAsync(e => e.Id == id);

        public async Task<bool> AnyAsync(Expression<Func<TEntity, bool>> expression)
        {
            return await _dbSet.AnyAsync(expression);
        }
        public IQueryable<TEntity> GetAll() => _dbSet.Where(e => !e.IsDeleted);

        public IQueryable<TEntity> GetAll(Expression<Func<TEntity, bool>> expression) => GetAll().Where(expression);
        public async Task<TEntity?> GetByIdAsync(Guid id) => await GetAll().FirstOrDefaultAsync(x => x.Id == id);

        public void UpdateInclude(TEntity entity, params string[] modifiedProperties)
        {
            if (!_dbSet.Any(x => x.Id == entity.Id && !x.IsDeleted))
                return;

            var local = _dbSet.Local.FirstOrDefault(x => x.Id == entity.Id);
            EntityEntry entityEntry;

            if (local is null)
                entityEntry = _appDbContext.Entry(entity);
            else
                entityEntry = _appDbContext.ChangeTracker.Entries<TEntity>().FirstOrDefault(x => x.Entity.Id == entity.Id)!;


            foreach (var prop in entityEntry.Properties)
            {
                if (modifiedProperties.Contains(prop.Metadata.Name))
                {
                    prop.CurrentValue = entity.GetType().GetProperty(prop.Metadata.Name)?.GetValue(entity);
                    prop.IsModified = true;
                }
            }

        }

        public async Task<int> SaveChangesAsync(CancellationToken cancellationToken)
        {
            var count = await _appDbContext.SaveChangesAsync(cancellationToken);
            return count;
        }
    }
}
