using UnityEngine;

namespace ChunkSpawner
{
    public class ChunkSpawnerConfig : ScriptableObject
    {
        [Tooltip("Размер чанка в тайлах")]
        public int ChunkSize;

        public float ChunkSpawnCooldown;
    }
}