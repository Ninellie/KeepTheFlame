using Spawning;
using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace FirefliesSpawn
{

    
    
    
    public class FireflySpawner : IFixedTickable, IStartable
    {
        private readonly FireflyPool _fireflyPool;
        private readonly SpawnerConfig _config;
        private readonly Transform _playerTransform;
        private readonly Camera _mainCamera;
        private readonly SpawnTimer _timer;

        public FireflySpawner(
            [Key(nameof(Firefly))] FireflyPool fireflyPool,
            [Key(nameof(Firefly))] SpawnerConfig config,
            [Key("Player")] Transform playerTransform,
            Camera mainCamera)
        {
            _fireflyPool = fireflyPool;
            _config = config;
            _playerTransform = playerTransform;
            _mainCamera = mainCamera;
            _timer = new SpawnTimer();
        }
        
        public void Start()
        {
            InitializeStartingFireflies();
        }
        
        public void FixedTick()
        {
            _timer.Tick(Time.deltaTime);
            
            if (_timer.TimeToNextSpawn > 0) return;
            
            var spawnInterval = GetSpawnInterval();
            
            _timer.Set(spawnInterval);
            
            SpawnFirefly();
        }
        
        private void InitializeStartingFireflies()
        {
            // Спавним светлячков внутри видимой области камеры
            var cameraHeight = _mainCamera.orthographicSize * 2f;
            var cameraWidth = cameraHeight * _mainCamera.aspect;
            var cameraRadius = Mathf.Sqrt(cameraWidth * cameraWidth + cameraHeight * cameraHeight) * 0.5f;
            
            // Минимальное расстояние от игрока (паддинг)
            var minRadius = _config.spawnCircleOffset;
            
            // Спавним пока есть место в пуле и не заполнили экран
            var attempts = 0;
            var maxAttempts = _fireflyPool.Size * 2; // избегаем бесконечного цикла
            
            while (_fireflyPool.Active < _fireflyPool.Size / 2 && attempts < maxAttempts)
            {
                attempts++;
                
                // Генерируем случайную точку внутри камеры, но вне минимального радиуса
                var angle = Random.Range(0f, 2f * Mathf.PI);
                var radius = Random.Range(minRadius, cameraRadius);
                
                var x = _playerTransform.position.x + radius * Mathf.Cos(angle);
                var y = _playerTransform.position.y + radius * Mathf.Sin(angle);
                var point = new Vector2(x, y);
                
                var sector = SpawnerTools.GetSector(point, _config);
                
                // Проверяем заполненность сектора
                if (_fireflyPool.IsSectorFull(sector))
                    continue;
                    
                // Получаем светлячка из пула
                var firefly = _fireflyPool.GetFromPool();
                if (firefly == null)
                    break;
                    
                // Устанавливаем сектор, позицию и случайный поворот
                firefly.Sector = sector;
                firefly.Transform.position = point;
                firefly.GameObject.SetActive(true);
            }
        }

        private float GetSpawnInterval()
        {
            var interval = _config.interval;
            var balanceFactor = _config.balanceFactor;
            var poolSizeScale = GetPoolSizeIntervalScale();
            
            return interval * poolSizeScale * balanceFactor;
        }

        private float GetPoolSizeIntervalScale()
        {
            float activeFireflies = _fireflyPool.Active;
            float poolSize = _fireflyPool.Size;

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
        
        private void SpawnFirefly()
        {
            var point = SpawnerTools.GetRandomPoint(_mainCamera, _playerTransform, _config);
            var sector = SpawnerTools.GetSector(point, _config);
            
            // Проверяем заполненность сектора
            if (_fireflyPool.IsSectorFull(sector))
                return;
                
            // Получаем светлячка из пула
            var firefly = _fireflyPool.GetFromPool();
            if (firefly == null)
                return;
                
            // Устанавливаем сектор, позицию и случайный поворот
            firefly.Sector = sector;
            firefly.Transform.position = point;
            firefly.GameObject.SetActive(true);
        }
    }
 }