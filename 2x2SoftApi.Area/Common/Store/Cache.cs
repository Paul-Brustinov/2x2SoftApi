using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _2x2SoftApi.Area.Common.Store
{
    public class Cache<T> where T : class, IEntity
    {
        Dictionary<Int64, T> _mapIds = new Dictionary<Int64, T>();

        public Dictionary<long, T> MapIds => _mapIds;

        public T GetById(Int64 id)
        {
            T entity = null;
            if (_mapIds.TryGetValue(id, out entity))
                return entity;
            return null;
        }

        public void Remove(Int64 entityId)
        {
            if (_mapIds.ContainsKey(entityId))
                _mapIds.Remove(entityId);
        }


        public void CacheEntity(T entity)
        {
            if (entity == null)
                return;
            if (!_mapIds.ContainsKey(entity.Id))
            {
                _mapIds.Add(entity.Id, entity);
            }
            else
            {
                var existing = _mapIds[entity.Id];
                if (!Comparer<T>.Equals(entity, existing))
                    throw new InvalidProgramException("Invalid user cache");
            }
        }
    }
}
