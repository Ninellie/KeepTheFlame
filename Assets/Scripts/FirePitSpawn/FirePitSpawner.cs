using FirefliesSpawn;
using Spawn;
using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace FirePitSpawn
{
    public class FirePitSpawner : IFixedTickable, IStartable
    {
        public float TimeToNextSpawn { get; private set; }
        
        private readonly FirePitPool _firePitPool;
        private readonly SpawnerConfig _spawnerConfig;
        private readonly Transform _playerTransform;
        private readonly Camera _mainCamera;

        public FirePitSpawner(FirePitPool firePitPool,
            [Key(nameof(FirePit))] SpawnerConfig spawnerConfig, 
            [Key("Player")] Transform playerTransform,
            Camera mainCamera)
        {
            _firePitPool = firePitPool;
            _spawnerConfig = spawnerConfig;
            _playerTransform = playerTransform;
            _mainCamera = mainCamera;
        }

        public void Start()
        {
            InitializeStartingFirePits();
        }
        
        public void FixedTick()
        {
            TimeToNextSpawn -= Time.deltaTime;
            
            if (TimeToNextSpawn > 0) return;
            
            var spawnInterval = GetSpawnInterval(_spawnerConfig);
            
            TimeToNextSpawn = spawnInterval;
            
            SpawnFirePit();
        }
        
        private void InitializeStartingFirePits()
        {
            // Спавним внутри видимой области камеры
            var cameraHeight = _mainCamera.orthographicSize * 2f;
            var cameraWidth = cameraHeight * _mainCamera.aspect;
            var cameraRadius = Mathf.Sqrt(cameraWidth * cameraWidth + cameraHeight * cameraHeight) * 0.5f;
            
            // Минимальное расстояние от игрока (паддинг)
            var minRadius = _spawnerConfig.spawnCircleOffset;
            
            // Спавним пока есть место в пуле и не заполнили экран
            var attempts = 0;
            var maxAttempts = _firePitPool.Size * 2; // избегаем бесконечного цикла
            
            while (_firePitPool.Active < _firePitPool.Size / 2 && attempts < maxAttempts)
            {
                attempts++;
                
                // Генерируем случайную точку внутри камеры, но вне минимального радиуса
                var angle = Random.Range(0f, 2f * Mathf.PI);
                var radius = Random.Range(minRadius, cameraRadius);
                
                var x = _playerTransform.position.x + radius * Mathf.Cos(angle);
                var y = _playerTransform.position.y + radius * Mathf.Sin(angle);
                var point = new Vector2(x, y);
                
                var sector = GetSector(point);
                
                // Проверяем заполненность сектора
                if (_firePitPool.IsSectorFull(sector))
                    continue;
                    
                // Получаем объект из пула
                var firePit = _firePitPool.GetFromPool();
                if (firePit == null)
                    break;
                    
                // Устанавливаем сектор и позицию
                firePit.Sector = sector;
                firePit.transform.position = point;
                firePit.gameObject.SetActive(true);
            }
        }

        private float GetSpawnInterval(SpawnerConfig config)
        {
            var interval = config.interval;
            var balanceFactor = config.balanceFactor;
            var poolSizeScale = GetPoolSizeIntervalScale();
            
            return interval * poolSizeScale * balanceFactor;
        }

        private float GetPoolSizeIntervalScale()
        {
            float activeFireflies = _firePitPool.Active;
            float poolSize = _firePitPool.Size;

            float poolSizeScale;
            
            if (poolSize == 0)
            {
                poolSizeScale = 1f;
            }
            else
            {
                poolSizeScale = activeFireflies / poolSize;
            }

            return poolSizeScale;
        }
        
        private void SpawnFirePit()
        {
            var point = SpawnTools.GetRandomPoint(_spawnerConfig, _mainCamera, _playerTransform);
            var sector = GetSector(point);
            
            // Проверяем заполненность сектора
            if (_firePitPool.IsSectorFull(sector))
                return;
                
            // Получаем костёр из пула
            var firePit = _firePitPool.GetFromPool();
            if (firePit == null)
                return;
                
            // Устанавливаем сектор и позицию
            firePit.Sector = sector;
            firePit.transform.position = point;
            firePit.gameObject.SetActive(true);
        }

        private Vector2Int GetSector(Vector2 position)
        {
            var x = Mathf.FloorToInt(position.x / _spawnerConfig.sectorSize);
            var y = Mathf.FloorToInt(position.y / _spawnerConfig.sectorSize);
            return new Vector2Int(x, y); 
        }
    }
}