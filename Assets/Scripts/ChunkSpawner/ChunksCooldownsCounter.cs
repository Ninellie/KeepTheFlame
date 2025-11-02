using System.Collections.Generic;
using UnityEngine;
using VContainer.Unity;

namespace ChunkSpawner
{
    /// <summary>
    /// Класс ведёт отсчёт кулдаунов спавна для отдельных чанков
    /// и позволяет установить кулдаун для чанка.
    /// </summary>
    public class ChunksCooldownsCounter : ITickable
    {
        private readonly Dictionary<Vector2Int, float> _chunks = new();
        
        public void Tick()
        {
            var delta = Time.deltaTime;
            var chunksToDelete = new List<Vector2Int>();
            
            foreach (var chunk in _chunks)
            { 
                _chunks[chunk.Key] -= delta;
                if (_chunks[chunk.Key] <= 0)
                {
                    chunksToDelete.Add(chunk.Key);
                }
            }

            foreach (var chunk in chunksToDelete)
            {
                _chunks.Remove(chunk);
            }
        }

        public bool IsOnCooldown(Vector2Int chunk)
        {
            return _chunks.ContainsKey(chunk);
        }
        
        // Необходимо вызывать каждый раз когда камера видит чанк
        public void SetCooldown(Vector2Int chunk, float cooldown)
        {
            _chunks[chunk] = cooldown;
        }
    }
}