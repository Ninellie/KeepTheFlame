using FirefliesSpawn;
using UnityEngine;
using VContainer.Unity;

namespace Spawning
{
    public interface IPooledEntity
    {
        Vector2Int Sector { get; set; }
        Transform Transform { get; }
        GameObject GameObject { get; }
    }
    
    public interface IEntityPool
    {
        int Size { get; }
        int Active { get; }
        bool IsSectorFull(Vector2Int sector);
        IPooledEntity GetFromPool();
    }
    
    public class Spawner : IStartable, IFixedTickable
    {
        private readonly SpawnerConfig _config;
        private readonly Transform _playerTransform;
        private readonly Camera _mainCamera;
        private readonly SpawnTimer _timer;
        private readonly IEntityPool _pool;
        
        public void Start()
        {
            InitializeStartingEntities();
        }

        public void FixedTick()
        {
            _timer.Tick(Time.deltaTime);
            if (_timer.TimeToNextSpawn > 0) return;
            
            var spawnInterval = SpawnerTools.GetSpawnInterval(_config, _pool.Size, _pool.Active);
            _timer.Set(spawnInterval);
            
            Spawn();
        }

        private void Spawn()
        {
            var point = SpawnerTools.GetRandomPoint(_mainCamera, _playerTransform, _config);
            var sector = SpawnerTools.GetSector(point, _config);
            
            // Проверяем заполненность сектора
            if (_pool.IsSectorFull(sector))
                return;
                
            // Получаем сущность из пула
            var entity = _pool.GetFromPool();
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
                var entity = _pool.GetFromPool();
                if (entity == null)
                    break;
                    
                // Устанавливаем сектор и позицию
                entity.Sector = sector;
                entity.Transform.position = point;
                entity.GameObject.SetActive(true);
            }
        }
    }
    
    public static class SpawnerTools
    {
        /// <summary>
        /// Получает сектор на игровом поле для переданной позиции
        /// </summary>
        /// <param name="position">Позиция, для которой требуется определить сектор</param>
        /// <param name="config">Конфигурация</param>
        /// <returns></returns>
        public static Vector2Int GetSector(Vector2 position, SpawnerConfig config)
        {
            var x = Mathf.FloorToInt(position.x / config.sectorSize);
            var y = Mathf.FloorToInt(position.y / config.sectorSize);
            return new Vector2Int(x, y); 
        }
        
        /// <summary>
        /// Получает точку между двумя окружностями за пределами камеры
        /// </summary>
        /// <param name="config">ScriptableObject config</param>
        /// <param name="camera">Ортографическая камера</param>
        /// <param name="playerTransform">Позиция игрока</param>
        /// <returns></returns>
        public static Vector2 GetRandomPoint(Camera camera, Transform playerTransform, SpawnerConfig config)
        {
            // Получаем размеры камеры
            var cameraHeight = camera.orthographicSize * 2f;
            var cameraWidth = cameraHeight * camera.aspect;
            
            // Радиус окружности, описанной вокруг прямоугольника камеры
            var cameraRadius = Mathf.Sqrt(cameraWidth * cameraWidth + cameraHeight * cameraHeight) * 0.5f;
            
            // Внутренний и внешний радиусы для спавна
            var innerRadius = cameraRadius + config.spawnCircleOffset;
            var outerRadius = innerRadius + config.spawnCircleRadius;
            
            // Генерируем случайную точку в кольце
            var angle = Random.Range(0f, 2f * Mathf.PI);
            var radius = Mathf.Sqrt(Random.Range(innerRadius * innerRadius, outerRadius * outerRadius));
            
            // Конвертируем в декартовы координаты относительно игрока
            var x = playerTransform.position.x + radius * Mathf.Cos(angle);
            var y = playerTransform.position.y + radius * Mathf.Sin(angle);
            
            return new Vector2(x, y);
        }
        
        public static float GetSpawnInterval(SpawnerConfig config, int size, int active)
        {
            var interval = config.interval;
            var balanceFactor = config.balanceFactor;
            var poolSizeIntervalScale = size == 0 ? 1f : (float)active / size;
            
            return interval * poolSizeIntervalScale * balanceFactor;
        }
    } 
}