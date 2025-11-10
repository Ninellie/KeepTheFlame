using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace ChunkSpawner
{
    public static class ChunkUtils
    {
        /// <summary>
        /// Возвращает координаты чанка, в котором находится игрок.
        /// </summary>
        /// <param name="tilemap">Tilemap, к которой привязаны тайлы.</param>
        /// <param name="worldPosition">Позиция игрока в мировых координатах.</param>
        /// <param name="chunkSize">Размер чанка в тайлах.</param>
        public static Vector2Int GetChunkFromWorldPosition(Tilemap tilemap, Vector3 worldPosition, int chunkSize)
        {
            var tilePos = tilemap.WorldToCell(worldPosition);

            var chunkX = Mathf.FloorToInt((float)tilePos.x / chunkSize);
            var chunkY = Mathf.FloorToInt((float)tilePos.y / chunkSize);

            return new Vector2Int(chunkX, chunkY);
        }
        
        /// <summary>
        /// Возвращает список координат тайлов, которые принадлежат указанному чанку.
        /// </summary>
        /// <param name="chunk">Координаты чанка (в чанках).</param>
        /// <param name="chunkSize">Размер чанка в тайлах.</param>
        public static List<Vector2Int> GetAllTilesInChunk(Vector2Int chunk, int chunkSize)
        {
            var tiles = new List<Vector2Int>(chunkSize * chunkSize);

            var startX = chunk.x * chunkSize;
            var startY = chunk.y * chunkSize;

            for (var x = 0; x < chunkSize; x++)
            {
                for (var y = 0; y < chunkSize; y++)
                {
                    tiles.Add(new Vector2Int(startX + x, startY + y));
                }
            }

            return tiles;
        }
        
        /// <summary>
        /// Вычисляет размер ортографической камеры в чанках, округляя в большую.
        /// </summary>
        /// <param name="camera">Ортографическая камера.</param>
        /// <param name="tilemap">Tilemap, из которой берётся размер тайла (Grid -> Cell Size).</param>
        /// <param name="chunkSize">Размер чанка в тайлах.</param>
        /// <returns>Размер камеры в чанках (по X и Y).</returns>
        public static Vector2Int GetCameraSizeInChunks(Camera camera, Tilemap tilemap, int chunkSize)
        {
            if (!camera.orthographic)
            {
                Debug.LogWarning("Камера должна быть ортографической");
            }

            // Размер тайла из Grid
            var tileSize = tilemap.layoutGrid.cellSize.x;

            // Размер камеры в мировых единицах
            var cameraHeight = camera.orthographicSize * 2f;
            var cameraWidth = cameraHeight * camera.aspect;

            // Размер камеры в тайлах
            var tilesX = cameraWidth / tileSize;
            var tilesY = cameraHeight / tileSize;

            // Размер камеры в чанках (округляем вверх)
            var chunksX = Mathf.CeilToInt(tilesX / chunkSize);
            var chunksY = Mathf.CeilToInt(tilesY / chunkSize);

            return new Vector2Int(chunksX, chunksY);
        }
        
        /// <summary>
        /// Возвращает список чанков, которые видит камера хотя бы частично.
        /// Можно расширить область на дополнительные чанки с каждой стороны.
        /// </summary>
        /// <param name="camera">Ортографическая камера</param>
        /// <param name="tilemap">Tilemap для перевода мировых координат в тайловые</param>
        /// <param name="chunkSize">Размер чанка в тайлах</param>
        /// <param name="extraChunkPadding">Дополнительные полоски чанков с каждой стороны (по умолчанию 0)</param>
        public static List<Vector2Int> GetVisibleChunks(Camera camera, Tilemap tilemap, int chunkSize, int extraChunkPadding = 0)
        {
            if (!camera.orthographic)
            {
                Debug.LogWarning("Камера должна быть ортографической");
            }
            
            var camPos = camera.transform.position;
            var camHeight = camera.orthographicSize * 2f;
            var camWidth = camHeight * camera.aspect;
            
            var worldBottomLeft = new Vector3(camPos.x - camWidth / 2f, camPos.y - camHeight / 2f, 0);
            var worldTopRight   = new Vector3(camPos.x + camWidth / 2f, camPos.y + camHeight / 2f, 0);
            
            var bottomLeftTile = tilemap.WorldToCell(worldBottomLeft);
            var topRightTile   = tilemap.WorldToCell(worldTopRight);
            
            var visibleChunks = GetChunksInTileRange(
                (Vector2Int)bottomLeftTile,
                (Vector2Int)topRightTile,
                chunkSize
            );

            // Если padding больше 0 — расширяем область
            if (extraChunkPadding <= 0) return visibleChunks;
            
            var minChunkX = int.MaxValue;
            var maxChunkX = int.MinValue;
            var minChunkY = int.MaxValue;
            var maxChunkY = int.MinValue;

            foreach (var chunk in visibleChunks)
            {
                if (chunk.x < minChunkX) minChunkX = chunk.x;
                if (chunk.x > maxChunkX) maxChunkX = chunk.x;
                if (chunk.y < minChunkY) minChunkY = chunk.y;
                if (chunk.y > maxChunkY) maxChunkY = chunk.y;
            }

            // Добавляем дополнительный padding
            for (var x = minChunkX - extraChunkPadding; x <= maxChunkX + extraChunkPadding; x++)
            {
                for (var y = minChunkY - extraChunkPadding; y <= maxChunkY + extraChunkPadding; y++)
                {
                    var paddedChunk = new Vector2Int(x, y);
                    if (!visibleChunks.Contains(paddedChunk))
                        visibleChunks.Add(paddedChunk);
                }
            }

            return visibleChunks;
        }
        
        /// <summary>
        /// Возвращает все чанки, которые попадают в область между нижним левым и верхним правым тайлом.
        /// </summary>
        /// <param name="bottomLeftTile">Нижний левый тайл (в координатах тайлов)</param>
        /// <param name="topRightTile">Верхний правый тайл (в координатах тайлов)</param>
        /// <param name="chunkSize">Размер чанка в тайлах</param>
        private static List<Vector2Int> GetChunksInTileRange(Vector2Int bottomLeftTile, Vector2Int topRightTile, int chunkSize)
        {
            var chunks = new List<Vector2Int>();

            var startChunkX = Mathf.FloorToInt((float)bottomLeftTile.x / chunkSize);
            var endChunkX   = Mathf.FloorToInt((float)topRightTile.x   / chunkSize);
            var startChunkY = Mathf.FloorToInt((float)bottomLeftTile.y / chunkSize);
            var endChunkY   = Mathf.FloorToInt((float)topRightTile.y   / chunkSize);

            for (var cx = startChunkX; cx <= endChunkX; cx++)
            {
                for (var cy = startChunkY; cy <= endChunkY; cy++)
                {
                    chunks.Add(new Vector2Int(cx, cy));
                }
            }

            return chunks;
        }

        public static Bounds GetChunkWorldBounds(Vector2Int chunkPos, int chunkSize, Tilemap tilemap)
        {
            var bottomLeftCell = (Vector3Int)(chunkPos * chunkSize);
            var bottomLeftPos = tilemap.CellToWorld(bottomLeftCell);
    
            var topRightCell = (Vector3Int)(new Vector2Int(chunkPos.x + 1, chunkPos.y + 1) * chunkSize);
            var topRightPos = tilemap.CellToWorld(topRightCell);
    
            var size = topRightPos - bottomLeftPos;
            size.x = Mathf.Abs(size.x);
            size.y = Mathf.Abs(size.y);
    
            var center = bottomLeftPos + size / 2f;
            return new Bounds(center, size);
        }
    }
}