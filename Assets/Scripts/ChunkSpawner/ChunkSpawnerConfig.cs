using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

namespace ChunkSpawner
{
    [CreateAssetMenu(menuName = "Config/ChunkSpawner")]
    public class ChunkSpawnerConfig : ScriptableObject
    {
        [Tooltip("Размер чанка в тайлах")]
        public int ChunkSize;
        public float ChunkSpawnCooldown;
        
        [Header("Шанс спавна сущности на каждом тайле.")]
        [Tooltip(" Выбирается случайное значение между минимумом и максимумом для чанка.")]
        [Range(0, 1)]
        [SerializeField]
        private float minSpawnChance;
        
        [Range(0, 1)]
        [SerializeField]
        private float maxSpawnChance;
        
        [SerializeField]
        private List<EntitySpawnData> entities;
        
        public Entity GetEntityPrefab()
        {
            if (entities == null || entities.Count == 0)
                return null;
            
            var totalWeight = entities.Sum(e => e.Weight);
            
            if (totalWeight <= 0f)
                return null;
            
            var randomPoint = Random.Range(0f, totalWeight);
            
            var cumulative = 0f;
            foreach (var e in entities)
            {
                cumulative += e.Weight;
                if (randomPoint <= cumulative)
                {
                    return e.Prefab;
                }
            }

            return entities[^1].Prefab;
        }

        public float GetSpawnChance()
        {
            if (minSpawnChance > maxSpawnChance)
            {
                return minSpawnChance;
            }
            
            return Random.Range(minSpawnChance, maxSpawnChance);
        }
    }
}