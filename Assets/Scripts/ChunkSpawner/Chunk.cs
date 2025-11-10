using System.Collections.Generic;
using UnityEngine;

namespace ChunkSpawner
{
    public class Chunk : MonoBehaviour
    {
        [field:SerializeField] public Vector2Int Position { get; set; }
        [field:SerializeField] public float SpawnChance { get; set; }
        [field:SerializeField] public float DestroyCooldown { get; set; }
        [field:SerializeField] public float BaseDestroyCooldown { get; set; }
        public List<Vector2Int> Tiles { get; set; } = new();
    }
}