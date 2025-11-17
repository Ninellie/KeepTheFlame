using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using VContainer;
using VContainer.Unity;
using Object = UnityEngine.Object;
using Random = UnityEngine.Random;

namespace ChunkSpawner
{
    /// <summary>
    /// ChunkSpawner слушает события пересечения игроком чанков и решает
    /// в каких чанках нужно произвести спавн пределами камеры.
    /// </summary>
    public class Spawner : IStartable, IDisposable
    {
        private int ChunkSize => _config.ChunkSize; // Размер чанка в тайлах
        
        private readonly ChunkSpawnerConfig _config;
        private readonly Camera _camera;
        private readonly Transform _cameraTransform;
        private readonly Tilemap _tilemap;
        private readonly ChunkBoundaryWatcher _watcher;
        private readonly ChunksDestroyCooldownsCounter _destroyCooldowns;
        
        private Vector2Int _currentChunkPosition;
        private IObjectResolver _resolver;
        
        public Spawner(ChunkSpawnerConfig config, Camera camera, Tilemap tilemap,
            ChunkBoundaryWatcher watcher, ChunksDestroyCooldownsCounter destroyCooldowns, IObjectResolver resolver)
        {
            _config = config;
            _camera = camera;
            _cameraTransform = _camera.transform;
            _tilemap = tilemap;
            _watcher = watcher;
            _destroyCooldowns = destroyCooldowns;
            _resolver = resolver;
        }
        
        public void Start()
        {
            _currentChunkPosition = ChunkUtils.GetChunkFromWorldPosition(_tilemap, _cameraTransform.position, ChunkSize);
            _watcher.OnChunkBoundaryCrossed += OnChunkCrossed;
            
            SpawnInitialChunks();
        }
        
        private void SpawnInitialChunks()
        {
            var visibleChunks = ChunkUtils.GetVisibleChunks(_camera, _tilemap, ChunkSize, 1);
            
            foreach (var chunkPos in visibleChunks)
            {
                if (_destroyCooldowns.IsOnCooldown(chunkPos))
                {
                    _destroyCooldowns.UpdateCooldown(chunkPos);
                    continue;
                }
                var chunk = CreateChunk(chunkPos);
                Spawn(chunk);
            }
        }
        
        public void Dispose()
        {
            _watcher.OnChunkBoundaryCrossed -= OnChunkCrossed;
        }
        
        private void OnChunkCrossed(Vector2Int direction)
        {
            _currentChunkPosition = ChunkUtils.GetChunkFromWorldPosition(_tilemap, _cameraTransform.position, ChunkSize);
            
            var cameraChunks = ChunkUtils.GetCameraSizeInChunks(_camera, _tilemap, ChunkSize);
            
            var chunksToSpawn = GetSpawnChunks(_currentChunkPosition, cameraChunks, direction);
            
            foreach (var chunkPos in chunksToSpawn)
            {
                if (_destroyCooldowns.IsOnCooldown(chunkPos))
                {
                    _destroyCooldowns.UpdateCooldown(chunkPos);
                    continue;
                }
                var chunk = CreateChunk(chunkPos);
                Spawn(chunk);
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
        
        private Chunk CreateChunk(Vector2Int chunkPos)
        {
            var chunkObject = new GameObject();
            chunkObject.name = $"Chunk {chunkPos}";
            var gizmoSquare = chunkObject.AddComponent<GizmoSquare>();
            
            // Узнать точку чанка
            var bounds = ChunkUtils.GetChunkWorldBounds(chunkPos, _config.ChunkSize, _tilemap);
            chunkObject.transform.position = bounds.min;
            gizmoSquare.Set(bounds.min, bounds.max);
            
            var chunk = chunkObject.AddComponent<Chunk>();
            chunk.BaseDestroyCooldown = _config.ChunkSpawnCooldown;
            chunk.Position = chunkPos;
            chunk.Tiles = ChunkUtils.GetAllTilesInChunk(chunkPos, _config.ChunkSize);
            chunk.SpawnChance = _config.GetSpawnChance();
            
            _destroyCooldowns.SetCooldown(chunk);
            
            return chunk;
        }
        
        private void Spawn(Chunk chunk)
        {
            Debug.Log($"[SPAWN] Чанк: {chunk.Position}");
            
            foreach (var chunkTile in chunk.Tiles)
            {
                var random = Random.Range(0f, 1f);
                
                if (random > chunk.SpawnChance) continue;
                
                var entityPrefab = _config.GetEntityPrefab();
                var position = _tilemap.CellToWorld((Vector3Int)chunkTile);
                var go = Object.Instantiate(entityPrefab, position, Quaternion.identity, chunk.transform);
                _resolver.InjectGameObject(go.gameObject);
            }
        }
    }
}