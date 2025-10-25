using Spawning;
using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace FirePitSpawn
{
    
    public class FirePitSpawner : IFixedTickable, IStartable
    {
        public float TimeToNextSpawn { get; private set; }
        
        private readonly FirePitPool _pool;
        
        private readonly SpawnerConfig _config;
        private readonly Transform _playerTransform;
        private readonly Camera _mainCamera;

        public FirePitSpawner(FirePitPool pool,
            [Key(nameof(FirePit))] SpawnerConfig config, 
            [Key("Player")] Transform playerTransform,
            Camera mainCamera)
        {
            _pool = pool;
            _config = config;
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
            
            var spawnInterval = SpawnerTools.GetSpawnInterval(_config, _pool.Size, _pool.Active);
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
            var minRadius = _config.spawnCircleOffset;
            
            // Спавним пока есть место в пуле и не заполнили экран
            var attempts = 0;
            var maxAttempts = _pool.Size * 2; // избегаем бесконечного цикла
            
            while (_pool.Active < _pool.Size / 2 && attempts < maxAttempts)
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
                if (_pool.IsSectorFull(sector))
                    continue;
                    
                // Получаем объект из пула
                var firePit = _pool.GetFromPool();
                if (firePit == null)
                    break;
                    
                // Устанавливаем сектор и позицию
                firePit.Sector = sector;
                firePit.Transform.position = point;
                firePit.GameObject.SetActive(true);
            }
        }
        
        private void SpawnFirePit()
        {
            var point = SpawnerTools.GetRandomPoint(_mainCamera, _playerTransform, _config);
            var sector = SpawnerTools.GetSector(point, _config);
            
            // Проверяем заполненность сектора
            if (_pool.IsSectorFull(sector))
                return;
                
            // Получаем костёр из пула
            var firePit = _pool.GetFromPool();
            if (firePit == null)
                return;
                
            // Устанавливаем сектор и позицию
            firePit.Sector = sector;
            firePit.Transform.position = point;
            firePit.GameObject.SetActive(true);
        }
    }
}