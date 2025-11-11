using System.Collections.Generic;
using UnityEngine;
using VContainer.Unity;

namespace ChunkSpawner
{
    /// <summary>
    /// Класс ведёт отсчёт кулдаунов спавна для отдельных чанков
    /// и позволяет установить кулдаун для чанка.
    /// Хранит все ссылки на существующие в данный момент чанки.
    /// </summary>
    public class ChunksDestroyCooldownsCounter : ITickable
    {
        private readonly List<Chunk> _chunks = new();
        
        public void Tick()
        {
            _chunks.RemoveAll(chunk => chunk == null);
            
            var delta = Time.deltaTime;
            var chunksToDestroy = new List<Chunk>();

            foreach (var chunk in _chunks)
            {
                if (chunk == null) continue;
                
                chunk.DestroyCooldown -= delta;
                if (chunk.DestroyCooldown <= 0)
                {
                    chunksToDestroy.Add(chunk);
                }
            }

            foreach (var chunk in chunksToDestroy)
            {
                _chunks.Remove(chunk);
                if (chunk != null)
                {
                    Object.Destroy(chunk.gameObject);
                }
            }
        }

        public bool IsOnCooldown(Vector2Int chunk)
        {
            return _chunks.Exists(x => x != null && x.Position == chunk);
        }

        public void SetCooldown(Chunk chunk)
        {
            _chunks.Add(chunk);
            chunk.DestroyCooldown = chunk.BaseDestroyCooldown;
        }
        
        // Необходимо вызывать каждый раз когда камера видит чанк
        public void UpdateCooldown(Vector2Int chunkPos)
        {
            var chunk = _chunks.Find(x => x != null && x.Position == chunkPos);
            if (chunk != null)
            {
                chunk.DestroyCooldown = chunk.BaseDestroyCooldown;
                return;
            }
            Debug.Log($"Chunk with pos {chunkPos} could not be found");
        }
    }
}