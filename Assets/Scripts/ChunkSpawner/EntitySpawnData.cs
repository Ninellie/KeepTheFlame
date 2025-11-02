using System;
using UnityEngine;

namespace ChunkSpawner
{
    [Serializable]
    public class EntitySpawnData
    {
        public Entity Prefab;
        [Min(0)] public int Weight;
        public int TileSize;
    }
}