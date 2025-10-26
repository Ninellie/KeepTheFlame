using UnityEngine;

namespace Spawning
{
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
            if (!config.dynamicInterval) return interval;
            var poolSizeIntervalScale = size == 0 ? 1f : (float)active / size;
            return interval * poolSizeIntervalScale;
        }
    } 
}