using PlayerMovement;
using UnityEngine;
using VContainer.Unity;

namespace FirefliesSpawn
{
    public class FireflySpawner : IFixedTickable
    {
        private readonly FireflyPool _fireflyPool;
        private readonly SpawnerConfig _spawnerConfig;
        private readonly PlayerTransform _playerTransform;
        private readonly Camera _mainCamera;
        private float _timeToNextSpawn;   

        public FireflySpawner(FireflyPool fireflyPool, SpawnerConfig spawnerConfig, PlayerTransform playerTransform, Camera mainCamera)
        {
            _fireflyPool = fireflyPool;
            _spawnerConfig = spawnerConfig;
            _playerTransform = playerTransform;
            _mainCamera = mainCamera;
            
            InitializeStartingFireflies();
        }
        
        private void InitializeStartingFireflies()
        {
            // Спавним светлячков внутри видимой области камеры
            var cameraHeight = _mainCamera.orthographicSize * 2f;
            var cameraWidth = cameraHeight * _mainCamera.aspect;
            var cameraRadius = Mathf.Sqrt(cameraWidth * cameraWidth + cameraHeight * cameraHeight) * 0.5f;
            
            // Минимальное расстояние от игрока (паддинг)
            var minRadius = _spawnerConfig.spawnCircleOffset;
            
            // Спавним пока есть место в пуле и не заполнили экран
            int attempts = 0;
            int maxAttempts = _fireflyPool.Size * 2; // избегаем бесконечного цикла
            
            while (_fireflyPool.Active < _fireflyPool.Size / 2 && attempts < maxAttempts)
            {
                attempts++;
                
                // Генерируем случайную точку внутри камеры, но вне минимального радиуса
                var angle = Random.Range(0f, 2f * Mathf.PI);
                var radius = Random.Range(minRadius, cameraRadius);
                
                var x = _playerTransform.Value.position.x + radius * Mathf.Cos(angle);
                var y = _playerTransform.Value.position.y + radius * Mathf.Sin(angle);
                var point = new Vector2(x, y);
                
                var sector = GetSector(point);
                
                // Проверяем заполненность сектора
                if (_fireflyPool.IsSectorFull(sector))
                    continue;
                    
                // Получаем светлячка из пула
                var firefly = _fireflyPool.GetFromPool();
                if (firefly == null)
                    break;
                    
                // Устанавливаем сектор и позицию
                firefly.Sector = sector;
                firefly.transform.position = point;
                firefly.gameObject.SetActive(true);
            }
        }

        public void FixedTick()
        {
            _timeToNextSpawn -= Time.deltaTime;
            
            if (_timeToNextSpawn > 0) return;
            
            _timeToNextSpawn = GetSpawnInterval();
            SpawnFirefly();
        }

        private float GetSpawnInterval()
        {
            var interval = _spawnerConfig.interval;
            var balanceFactor = _spawnerConfig.balanceFactor;
            var activeFireflies = _fireflyPool.Active;
            var poolSize = _fireflyPool.Size;

            return interval * (activeFireflies / poolSize) * balanceFactor;
        }

        private Vector2 GetRandomPoint()
        {
            // Получаем размеры камеры
            var cameraHeight = _mainCamera.orthographicSize * 2f;
            var cameraWidth = cameraHeight * _mainCamera.aspect;
            
            // Радиус окружности, описанной вокруг прямоугольника камеры
            var cameraRadius = Mathf.Sqrt(cameraWidth * cameraWidth + cameraHeight * cameraHeight) * 0.5f;
            
            // Внутренний и внешний радиусы для спавна
            var innerRadius = cameraRadius + _spawnerConfig.spawnCircleOffset;
            var outerRadius = innerRadius + _spawnerConfig.spawnCircleRadius;
            
            // Генерируем случайную точку в кольце
            var angle = Random.Range(0f, 2f * Mathf.PI);
            var radius = Mathf.Sqrt(Random.Range(innerRadius * innerRadius, outerRadius * outerRadius));
            
            // Конвертируем в декартовы координаты относительно игрока
            var x = _playerTransform.Value.position.x + radius * Mathf.Cos(angle);
            var y = _playerTransform.Value.position.y + radius * Mathf.Sin(angle);
            
            return new Vector2(x, y);
        }

        private void SpawnFirefly()
        {
            var point = GetRandomPoint();
            var sector = GetSector(point);
            
            // Проверяем заполненность сектора
            if (_fireflyPool.IsSectorFull(sector))
                return;
                
            // Получаем светлячка из пула
            var firefly = _fireflyPool.GetFromPool();
            if (firefly == null)
                return;
                
            // Устанавливаем сектор и позицию
            firefly.Sector = sector;
            firefly.transform.position = point;
            firefly.gameObject.SetActive(true);
        }

        private Vector2Int GetSector(Vector2 position)
        {
            var x = Mathf.FloorToInt(position.x / _spawnerConfig.sectorSize);
            var y = Mathf.FloorToInt(position.y / _spawnerConfig.sectorSize);
            return new Vector2Int(x, y);
        }
    }
}