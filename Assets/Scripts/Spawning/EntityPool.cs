using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Spawning
{
    public class EntityPool
    {
        public int Size => _active.Count + _inactive.Count;
        public int Active => _active.Count;

        private readonly List<IPooledEntity> _inactive = new();
        private readonly List<IPooledEntity> _active = new();
        
        private readonly SpawnerConfig _config;
        private readonly IEntityFactory _factory;
        private readonly Transform _playerTransform;
        
        public EntityPool(SpawnerConfig config, Transform playerTransform, IEntityFactory factory)
        {
            _config = config;
            _playerTransform = playerTransform;
            _factory = factory;
        }

        public void Init()
        {
            Cleanup();
            
            for (var i = 0; i < _config.poolSize; i++)
            {
                var entity = _factory.CreateInstance();
                entity.Pool = this;
                _inactive.Add(entity);
            }
        }
        
        public void ReturnAllToPool()
        {
            foreach (var activeEntity in _active.ToList())
            {
                if (activeEntity != null) ReturnToPool(activeEntity);
            }
        }
        
        public IPooledEntity GetFromPool()
        {
            IPooledEntity entity;
            
            if (_inactive.Count > 0)
            {
                // Берем из пула неактивных
                entity = _inactive[0];
                _inactive.RemoveAt(0);
            }
            else
            {
                // Находим самую дальнюю активную сущность
                entity = FindFarthest();
                if (entity == null) return null;
                
                _active.Remove(entity);
                entity.GameObject.SetActive(false);
            }
            
            _active.Add(entity);
            
            return entity;
        }
        
        public bool IsSectorFull(Vector2Int sector)
        {
            var entitiesInSector = _active.Count(entity => entity.Sector == sector);
            return entitiesInSector >= _config.sectorCapacity;
        }
        
        public void ReturnToPool(IPooledEntity entity)
        {
            _active.Remove(entity);
            entity.GameObject.SetActive(false);
            _inactive.Add(entity);
        }
        
        private IPooledEntity FindFarthest()
        {
            if (_active.Count == 0) return null;
            
            var playerPosition = _playerTransform.position;
            
            IPooledEntity farthest = null;
            var maxDistance = 0f;
            
            foreach (var entity in _active)
            {
                var distance = Vector2.Distance(playerPosition, entity.Transform.position);
                if (distance > maxDistance)
                {
                    maxDistance = distance;
                    farthest = entity;
                }
            }
            
            return farthest;
        }
        
        private void Cleanup()
        {
            ReturnAllToPool();
            
            foreach (var entity in _inactive.ToList())
            {
                _inactive.Remove(entity);
                Object.Destroy(entity.GameObject);
            }
        }
    }
}