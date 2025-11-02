using System;
using UnityEngine;
using UnityEngine.Tilemaps;
using VContainer.Unity;

namespace ChunkSpawner
{
    /// <summary>
    /// Отправляет ивент OnChunkBoundaryCrossed когда камера пересекает границу какого либо чанка.
    /// </summary>
    public class ChunkBoundaryWatcher : ITickable, IStartable
    {
        /// <summary>
        /// Событие вызывается, когда игрок пересекает границу чанка.
        /// Передаётся направление (Vector2Int.up, down, left, right).
        /// </summary>
        public event Action<Vector2Int> OnChunkBoundaryCrossed;
        
        private readonly ChunkSpawnerConfig _config;
        private readonly Camera _camera;
        private readonly Transform _cameraTransform;
        private readonly Tilemap _tilemap;
        private readonly ChunksCooldownsCounter _cooldowns;
        private int ChunkSize => _config.ChunkSize;
        
        private Vector2Int _currentChunk;

        public ChunkBoundaryWatcher(ChunkSpawnerConfig config, Camera camera, Tilemap tilemap,
            ChunksCooldownsCounter cooldowns)
        {
            _config = config;
            _camera = camera;
            _cameraTransform = camera.transform;
            _tilemap = tilemap;
            _cooldowns = cooldowns;
        }
        
        public void Start()
        {
            _currentChunk = ChunkUtils.GetChunkFromWorldPosition(_tilemap, _cameraTransform.position, ChunkSize);
            
            SetVisibleChunksOnCooldown();
        }

        private void SetVisibleChunksOnCooldown()
        {
            var visibleChunks = ChunkUtils.GetVisibleChunks(_camera, _tilemap, ChunkSize);
            
            foreach (var chunk in visibleChunks)
            {
                _cooldowns.SetCooldown(chunk, _config.ChunkSpawnCooldown);
            }
        }
        
        public void Tick()
        {
            var previousChunk = _currentChunk;
            
            _currentChunk = ChunkUtils.GetChunkFromWorldPosition(_tilemap, _cameraTransform.position, ChunkSize);

            if (_currentChunk == previousChunk) return;
            
            var diff = _currentChunk - previousChunk;

            switch (diff.x)
            {
                case > 0:
                    OnChunkBoundaryCrossed?.Invoke(Vector2Int.right);
                    break;
                case < 0:
                    OnChunkBoundaryCrossed?.Invoke(Vector2Int.left);
                    break;
            }

            switch (diff.y)
            {
                case > 0:
                    OnChunkBoundaryCrossed?.Invoke(Vector2Int.up);
                    break;
                case < 0:
                    OnChunkBoundaryCrossed?.Invoke(Vector2Int.down);
                    break;
            }
        }
    }
}