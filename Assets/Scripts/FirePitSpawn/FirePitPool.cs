using System.Collections.Generic;
using System.Linq;
using Spawning;
using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace FirePitSpawn
{
    public class FirePitPool : IStartable, IEntityPool
    {
        public int Size => _active.Count + _inactive.Count;
        public int Active => _active.Count;

        private readonly List<FirePit> _inactive = new();
        private readonly List<FirePit> _active = new();
        
        private readonly SpawnerConfig _config;
        private readonly Transform _playerTransform;
        private readonly FirePitFactory _factory;
        
        public FirePitPool(
            [Key(nameof(FirePit))] SpawnerConfig config,
            [Key("Player")] Transform playerTransform, 
            FirePitFactory factory)
        {
            _config = config;
            _playerTransform = playerTransform;
            _factory = factory;
        }

        public void Start()
        {
            Init();
        }

        public void Init()
        {
            Cleanup();
            
            // Создаем пул костров
            for (var i = 0; i < _config.poolSize; i++)
            {
                var firePit = _factory.CreateInstance();
                _inactive.Add(firePit);
            }
        }
        
        public void ReturnAllToPool()
        {
            foreach (var activeFirefly in _active.ToList())
            {
                if (activeFirefly != null) ReturnToPool(activeFirefly);
            }
        }
        
        public IPooledEntity GetFromPool()
        {
            FirePit firefly;
            
            if (_inactive.Count > 0)
            {
                // Берем из пула неактивных
                firefly = _inactive[0];
                _inactive.RemoveAt(0);
            }
            else
            {
                // Находим самого дальнего активного светлячка
                firefly = FindFarthest();
                if (firefly == null) return null;
                
                _active.Remove(firefly);
                firefly.gameObject.SetActive(false);
            }
            
            _active.Add(firefly);
            
            return firefly;
        }
        
        public bool IsSectorFull(Vector2Int sector)
        {
            var firefliesInSector = _active.Count(firefly => firefly.Sector == sector);
            return firefliesInSector >= _config.maxSpawnsPerSector;
        }
        
        public void ReturnToPool(FirePit firepit)
        {
            _active.Remove(firepit);
            firepit.gameObject.SetActive(false);
            _inactive.Add(firepit);
        }
        
        private FirePit FindFarthest()
        {
            if (_active.Count == 0) return null;
            
            var playerPosition = _playerTransform.position;
            FirePit farthest = null;
            var maxDistance = 0f;
            
            foreach (var firefly in _active)
            {
                var distance = Vector2.Distance(playerPosition, firefly.transform.position);
                if (distance > maxDistance)
                {
                    maxDistance = distance;
                    farthest = firefly;
                }
            }
            
            return farthest;
        }
        
        private void Cleanup()
        {
            if (_inactive != null)
            {
                foreach (var firefly in _inactive.ToList())
                {
                    _inactive.Remove(firefly);
                    Object.Destroy(firefly.gameObject);
                }
            }

            if (_active != null)
            {
                foreach (var firefly in _active.ToList())
                {
                    _active.Remove(firefly);
                    Object.Destroy(firefly.gameObject);
                }
            }
        }
    }
}