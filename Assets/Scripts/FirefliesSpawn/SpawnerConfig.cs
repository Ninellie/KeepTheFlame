using UnityEngine;

namespace FirefliesSpawn
{
    [CreateAssetMenu(menuName = "Config/Spawner")]
    public class SpawnerConfig : ScriptableObject
    {
        [Header("Pool")]
        public int poolSize;
        
        [Header("Sector")]
        public float sectorSize;
        public int maxSpawnsPerSector;
        
        [Header("Spawn delay")]
        public float interval;
        
        // Множитель интервала спавна. Чем он больше,
        // тем меньше значение активных светлячков по отношению
        // к общему количеству нужно, чтобы интервал был неизменённым.
        // Так как интервал будет тем больше, чем ближе значение активных светлячков к максимальному.
        // И соответственно в обратную сторону.
        public float balanceFactor; 
        
        [Header("Area")]
        public float spawnCircleOffset;
        public float spawnCircleRadius;
        
        [Header("Prefab")]
        public GameObject prefab;
    }
}