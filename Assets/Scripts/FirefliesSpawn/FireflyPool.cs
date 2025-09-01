using System.Collections.Generic;
using System.Linq;
using FirefliesPicking;
using Spawn;
using UnityEngine;
using VContainer;

namespace FirefliesSpawn
{
    public class FireflyPool
    {
        private const float FuelAmount = 1;
        public int Size => _activeFireflies.Count + _inactiveFireflies.Count;
        public int Active => _activeFireflies.Count;

        private readonly List<Firefly> _inactiveFireflies = new();
        private readonly List<Firefly> _activeFireflies = new();
        
        private readonly Transform _playerTransform;
        private readonly SpawnerConfig _config;
        private readonly FireflyPicker _picker;
        
        public FireflyPool([Key(nameof(Firefly))] SpawnerConfig config, [Key("Player")] Transform playerTransform, FireflyPicker picker)
        {
            _config = config;
            _playerTransform = playerTransform;
            _picker = picker;

            Init();
        }

        public void Init()
        {
            Cleanup();
            
            // Создаем пул светлячков
            for (var i = 0; i < _config.poolSize; i++)
            {
                var firefly = Object.Instantiate(_config.prefab).GetComponent<Firefly>();
                firefly.Init(this, _picker, FuelAmount);
                firefly.gameObject.SetActive(false);
                _inactiveFireflies.Add(firefly);
            }
        }

        public void ReturnAllToPool()
        {
            foreach (var activeFirefly in _activeFireflies.ToList())
            {
                if (activeFirefly != null) ReturnToPool(activeFirefly);
            }
        }
        
        public Firefly GetFromPool()
        {
            Firefly firefly;
            
            if (_inactiveFireflies.Count > 0)
            {
                // Берем из пула неактивных
                firefly = _inactiveFireflies[0];
                _inactiveFireflies.RemoveAt(0);
            }
            else
            {
                // Находим самого дальнего активного светлячка
                firefly = FindFarthestFirefly();
                if (firefly == null) return null;
                
                _activeFireflies.Remove(firefly);
                firefly.gameObject.SetActive(false);
            }
            
            _activeFireflies.Add(firefly);
            
            return firefly;
        }
        
        public bool IsSectorFull(Vector2Int sector)
        {
            var firefliesInSector = _activeFireflies.Count(firefly => firefly.Sector == sector);
            return firefliesInSector >= _config.maxSpawnsPerSector;
        }
        
        public void ReturnToPool(Firefly firefly)
        {
            _activeFireflies.Remove(firefly);
            firefly.gameObject.SetActive(false);
            _inactiveFireflies.Add(firefly);
        }
        
        private Firefly FindFarthestFirefly()
        {
            if (_activeFireflies.Count == 0) return null;
            
            var playerPosition = _playerTransform.position;
            Firefly farthestFirefly = null;
            var maxDistance = 0f;
            
            foreach (var firefly in _activeFireflies)
            {
                var distance = Vector2.Distance(playerPosition, firefly.transform.position);
                if (distance > maxDistance)
                {
                    maxDistance = distance;
                    farthestFirefly = firefly;
                }
            }
            
            return farthestFirefly;
        }
        
        private void Cleanup()
        {
            if (_inactiveFireflies != null)
            {
                foreach (var firefly in _inactiveFireflies.ToList())
                {
                    _inactiveFireflies.Remove(firefly);
                    Object.Destroy(firefly.gameObject);
                }
            }

            if (_activeFireflies != null)
            {
                foreach (var firefly in _activeFireflies.ToList())
                {
                    _activeFireflies.Remove(firefly);
                    Object.Destroy(firefly.gameObject);
                }
            }
        }
    }
}