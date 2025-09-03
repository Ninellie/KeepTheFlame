using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace TileFloorGeneration
{
    public static class TilePositionCalculator
    {
        /// <summary>
        /// Вычисляет позиции для размещения тайлов на основе размера камеры и тайла
        /// </summary>
        /// <param name="tile">Тайл для размещения</param>
        /// <param name="camera">Камера для определения области размещения</param>
        /// <param name="offset">Дополнительный отступ с каждой стороны</param>
        /// <returns>Позиций тайлов</returns>
        public static IEnumerable<Vector3Int> GetTilePositionsInCameraBounds(Tile tile, Camera camera, int offset)
        {
            var tileSize = tile.sprite.bounds.size;
            var cameraHeight = 2f * camera.orthographicSize;
            var cameraWidth = cameraHeight * camera.aspect;
            var cameraSize = new Vector2(cameraWidth, cameraHeight);
            
            var tilesX = Mathf.CeilToInt(cameraSize.x / tileSize.x);
            var tilesY = Mathf.CeilToInt(cameraSize.y / tileSize.y);
            
            var startX = -tilesX / 2 - offset;
            var endX = tilesX / 2 + offset;
            var startY = -tilesY / 2 - offset;
            var endY = tilesY / 2 + offset;
            
            for (var x = startX; x <= endX; x++)
            {
                for (var y = startY; y <= endY; y++)
                {
                    yield return new Vector3Int(x, y, 0);
                }
            }
        }
    }
}