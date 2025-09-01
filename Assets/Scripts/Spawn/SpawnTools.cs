using FirefliesSpawn;
using UnityEngine;

namespace Spawn
{
    public static class SpawnTools
    {
        /// <summary>
        /// Получает точку между двумя окружностями за пределами камеры
        /// </summary>
        /// <param name="config">ScriptableObject config</param>
        /// <param name="mainCamera">Ортографическая камера</param>
        /// <param name="playerTransform">Позиция игрока</param>
        /// <returns></returns>
        public static Vector2 GetRandomPoint(SpawnerConfig config, Camera mainCamera, Transform playerTransform)
        {
            // Получаем размеры камеры
            var cameraHeight = mainCamera.orthographicSize * 2f;
            var cameraWidth = cameraHeight * mainCamera.aspect;
            
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
    }
}