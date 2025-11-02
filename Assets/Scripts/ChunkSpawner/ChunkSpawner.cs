using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor.EditorTools;
using UnityEngine;
using UnityEngine.Tilemaps;
using VContainer.Unity;
using Object = UnityEngine.Object;
using Random = UnityEngine.Random;

namespace ChunkSpawner
{
    /// <summary>
    /// ChunkSpawner слушает события пересечения игроком чанков и решает
    /// в каких чанках нужно произвести спавн пределами камеры.
    /// </summary>
    public class ChunkSpawner : IStartable, IDisposable
    {
        private readonly ChunkSpawnerConfig _config;
        private readonly Camera _camera;
        private readonly Transform _cameraTransform;
        private readonly Tilemap _tilemap;
        private readonly ChunkBoundaryWatcher _watcher;
        private readonly ChunksCooldownsCounter _cooldowns;
        private int ChunkSize => _config.ChunkSize; // Размер чанка в тайлах
        
        private List<Entity> _entities = new();
        private Vector2Int _currentChunk;
        
        public ChunkSpawner(ChunkSpawnerConfig config, Camera camera, Tilemap tilemap,
            ChunkBoundaryWatcher watcher, ChunksCooldownsCounter cooldowns)
        {
            _config = config;
            _camera = camera;
            _cameraTransform = _camera.transform;
            _tilemap = tilemap;
            _watcher = watcher;
            _cooldowns = cooldowns;
        }
        
        public void Start()
        {
            _currentChunk = ChunkUtils.GetChunkFromWorldPosition(_tilemap, _cameraTransform.position, ChunkSize);
            _watcher.OnChunkBoundaryCrossed += OnChunkCrossed;
        }
        
        public void Dispose()
        {
            _watcher.OnChunkBoundaryCrossed -= OnChunkCrossed;
        }
        
        private void OnChunkCrossed(Vector2Int direction)
        {
            _currentChunk = ChunkUtils.GetChunkFromWorldPosition(_tilemap, _cameraTransform.position, ChunkSize);
            
            var cameraChunks = ChunkUtils.GetCameraSizeInChunks(_camera, _tilemap, ChunkSize);
            
            var chunksToSpawn = GetSpawnChunks(_currentChunk, cameraChunks, direction);
            
            foreach (var chunk in chunksToSpawn)
            {
                if (!_cooldowns.IsOnCooldown(chunk))
                {
                    Spawn(chunk);
                }
                _cooldowns.SetCooldown(chunk, _config.ChunkSpawnCooldown);
            }
        }

        /// <summary>
        /// Возвращает список координат чанков, которые нужно заспаунить за пределами камеры
        /// в зависимости от направления движения.
        /// </summary>
        private static List<Vector2Int> GetSpawnChunks(Vector2Int playerChunk,
            Vector2Int cameraChunks, Vector2Int direction)
        {
            var spawnPositions = new List<Vector2Int>();
            
            var halfWidth = cameraChunks.x / 2;
            var halfHeight = cameraChunks.y / 2;
            
            var offsetX = (halfWidth + 1) * direction.x;
            var offsetY = (halfHeight + 1) * direction.y;
            
            if (direction == Vector2Int.right || direction == Vector2Int.left)
            {
                var spawnX = playerChunk.x + offsetX;

                for (var y = -halfHeight; y <= halfHeight; y++)
                {
                    spawnPositions.Add(new Vector2Int(spawnX, playerChunk.y + y));
                }
            }
            
            else if (direction == Vector2Int.up || direction == Vector2Int.down)
            {
                var spawnY = playerChunk.y + offsetY;

                for (var x = -halfWidth; x <= halfWidth; x++)
                {
                    spawnPositions.Add(new Vector2Int(playerChunk.x + x, spawnY));
                }
            }

            return spawnPositions;
        }

        private void ClearChunk(Vector2Int chunk)
        {
            var entitiesToRemove= _entities.Where(e => e.Chunk == chunk);
            
            foreach (var entity in entitiesToRemove)
            {
                _entities.Remove(entity);
                entity.gameObject.SetActive(false);
                Object.Destroy(entity.gameObject);
            }
        }
        
        private void Spawn(Vector2Int chunk)
        {
            Debug.Log($"[SPAWN] Чанк: {chunk}");

            ClearChunk(chunk);
            
            var chunkTiles = ChunkUtils.GetAllTilesInChunk(chunk, _config.ChunkSize);

            var spawnChance = _config.GetSpawnChance();

            var chunkObject = new GameObject();
            
            foreach (var chunkTile in chunkTiles)
            {
                if (Random.Range(0, 1) > spawnChance)
                {
                    continue;
                }

                var entityPrefab = _config.GetEntityPrefab();
                
                var position = _tilemap.CellToWorld((Vector3Int)chunkTile);
                
                var entity = Object.Instantiate(entityPrefab, position, Quaternion.identity, chunkObject.transform);
                entity.Chunk = chunk;
                
                _entities.Add(entity);
            }
        }
    }
}