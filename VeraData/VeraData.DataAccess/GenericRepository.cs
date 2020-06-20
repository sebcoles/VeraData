using DataAccess;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace VeraData.DataAccess
{
    public class GenericRepository<TEntity> : IGenericRepository<TEntity> where TEntity : class
    {
        protected readonly ApplicationDbContext _dbContext;
        public GenericRepository(ApplicationDbContext context)
        {
            _dbContext = context;
        }
        public IQueryable<TEntity> GetAll()
        {
            return _dbContext
                .Set<TEntity>()
                .AsNoTracking();
        }

        public TEntity Create(TEntity entity)
        {
            _dbContext.Set<TEntity>().Add(entity);
            _dbContext.SaveChanges();
            return entity;
        }

        public void Create(List<TEntity> entities)
        {
            _dbContext.Set<TEntity>().AddRange(entities);
            _dbContext.SaveChanges();
        }

        public void Update(TEntity entity)
        {
            _dbContext.Set<TEntity>().Update(entity);
            _dbContext.SaveChanges();
        }
    }
}
