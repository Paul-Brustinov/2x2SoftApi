using A2v10.Data.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _2x2SoftApi.Area.Common.Store
{
    public class Store<T> where T : class, IEntity
    {
        protected IDbContext _dbContext;
        protected Cache<T> _cache;

        protected readonly string _entityName;
        protected readonly string _entitySchema;

        public Store(IDbContext dbContext, string name, string schema)
        {
            _entityName = name;
            _entitySchema = schema;
            _dbContext = dbContext;
            _cache = new Cache<T>();
        }

        public async Task CreateAsync(T entity)
        {
            await _dbContext.ExecuteAsync(null, _entitySchema + "." + _entityName + "Create", entity);
            _cache.CacheEntity(entity);
        }

        public async Task AddOrUpdateAsync(T entity)
        {
            await _dbContext.ExecuteAsync(null, _entitySchema + "." + _entityName + "AddOrUpdate", entity);
            _cache.CacheEntity(entity);
        }


        public async Task DeleteAsync(Int64 entityId)
        {
            await _dbContext.ExecuteAsync(null, _entitySchema + "." + _entityName + "Delete", new { Id = entityId });
        }

        public async Task<T> FindByIdAsync(Int64 entityId)
        {
            T entity = _cache.GetById(entityId);
            if (entity != null)
                return entity;
            entity = await _dbContext.LoadAsync<T>(null, _entitySchema + "." + _entityName + "FindById", new { Id = entityId });
            _cache.CacheEntity(entity);
            return entity;
        }

        public async Task UpdateAsync(T entity)
        {
            await _dbContext.ExecuteAsync<T>(null, _entitySchema + "." + _entityName + "Update", entity);
        }

        public async Task<IList<T>> GetPage(int pageNo, int pageCount)
        {
            IList<T> list = await _dbContext.LoadListAsync<T>(null, _entitySchema + "." + _entityName + "GetPage", new { PageNo = pageNo, PageCount = pageCount });
            return list;
        }

        public async Task<IList<T>> GetAll()
        {
            IList<T> list = await _dbContext.LoadListAsync<T>(null, _entitySchema + "." + _entityName + "GetAll", new { });
            return list;
        }
    }
}
