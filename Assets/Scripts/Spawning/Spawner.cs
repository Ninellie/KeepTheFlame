using UnityEngine;
using VContainer.Unity;

namespace Spawning
{
    public class Spawner : IStartable, IFixedTickable
    {
        public SpawnTimer Timer { get; }
        public EntityPool Pool { get; }

        private readonly SpawnerConfig _config;
        private readonly Transform _playerTransform;
        private readonly Camera _mainCamera;

        public Spawner(SpawnerConfig config, IEntityFactory factory, Transform playerTransform, Camera mainCamera)
        {
            _config = config;
            Pool = new EntityPool(config, playerTransform, factory);
            _playerTransform = playerTransform;
            _mainCamera = mainCamera;
            Timer = new SpawnTimer();
        }

        public void Start()
        {
            Pool.Init();
            InitializeStartingEntities();
        }

        public void FixedTick()
        {
            Timer.Tick(Time.deltaTime);
            if (Timer.TimeToNextSpawn > 0) return;
            
            var spawnInterval = SpawnerTools.GetSpawnInterval(_config, Pool.Size, Pool.Active);
            Timer.Set(spawnInterval);

            for (int i = 0; i < _config.spawnsPerInterval; i++)
            {
                Spawn();
            }
        }

        private void Spawn()
        {
            var point = SpawnerTools.GetRandomPoint(_mainCamera, _playerTransform, _config);
            var sector = SpawnerTools.GetSector(point, _config);
            
            // Проверяем заполненность сектора
            if (Pool.IsSectorFull(sector))
                return;
                
            // Получаем сущность из пула
            var entity = Pool.GetFromPool();
            if (entity == null)
                return;
                
            // Устанавливаем сектор и позицию
            entity.Sector = sector;
            entity.Transform.position = point;
            entity.GameObject.SetActive(true);
        }

        private void InitializeStartingEntities()
        {
            // Спавним внутри видимой области камеры
            var cameraHeight = _mainCamera.orthographicSize * 2f;
            var cameraWidth = cameraHeight * _mainCamera.aspect;
            var cameraRadius = Mathf.Sqrt(cameraWidth * cameraWidth + cameraHeight * cameraHeight) * 0.5f;
            
            // Минимальное расстояние от игрока (паддинг)
            var minRadius = _config.spawnCircleOffset;
            
            // Спавним пока есть место в пуле и не заполнили экран
            var attempts = 0;
            var maxAttempts = Pool.Size * 2; // избегаем бесконечного цикла
            
            while (Pool.Active < Pool.Size / 2 && attempts < maxAttempts)
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
                if (Pool.IsSectorFull(sector))
                    continue;
                    
                // Получаем объект из пула
                var entity = Pool.GetFromPool();
                if (entity == null)
                    break;
                    
                // Устанавливаем сектор и позицию
                entity.Sector = sector;
                entity.Transform.position = point;
                entity.GameObject.SetActive(true);
            }
        }
    }
}