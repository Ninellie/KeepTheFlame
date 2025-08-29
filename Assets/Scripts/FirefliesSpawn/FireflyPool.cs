using System.Collections.Generic;
using System.Linq;
using PlayerMovement;
using UnityEngine;

namespace FirefliesSpawn
{
    public class FireflyPool
    {
        public int Size { get; private set; }
        public int Active { get; private set; }

        public static event System.Action<Firefly> OnFireflyCollected;

        private readonly List<Firefly> _inactiveFireflies = new();
        private readonly List<Firefly> _activeFireflies = new();
        
        private readonly PlayerTransform _playerTransform;
        private readonly SpawnerConfig _config;

        public FireflyPool(SpawnerConfig config, PlayerTransform playerTransform)
        {
            _config = config;
            _playerTransform = playerTransform;
            Init();
        }

        public void Init()
        {
            Cleanup();

            Size = _config.poolSize;
            Active = 0;

            // Создаем пул светлячков
            for (var i = 0; i < _config.poolSize; i++)
            {
                var firefly = Object.Instantiate(_config.fireflyPrefab).GetComponent<Firefly>();
                firefly.Init(this);
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
            Active = _activeFireflies.Count;
            
            return firefly;
        }
        
        public bool IsSectorFull(Vector2Int sector)
        {
            var firefliesInSector = _activeFireflies.Count(firefly => firefly.Sector == sector);
            return firefliesInSector >= _config.maxFirefliesPerSector;
        }
        
        public void ReturnToPool(Firefly firefly)
        {
            OnFireflyCollected?.Invoke(firefly);
            
            _activeFireflies.Remove(firefly);
            firefly.gameObject.SetActive(false);
            _inactiveFireflies.Add(firefly);
            Active = _activeFireflies.Count;
        }
        
        private Firefly FindFarthestFirefly()
        {
            if (_activeFireflies.Count == 0) return null;
            
            var playerPosition = _playerTransform.Value.position;
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
    }
}