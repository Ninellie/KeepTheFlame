using UnityEngine;

namespace Spawning
{
    [CreateAssetMenu(menuName = "Config/Spawner")]
    public class SpawnerConfig : ScriptableObject
    {
        [Header("Pool")]
        public int poolSize;
        
        [Header("Sector")]
        public float sectorSize;
        [Tooltip("Максимальное количество сущностей которые могут появится в одном секторе")]
        public int sectorCapacity;
        
        [Header("Spawn interval")]
        public float interval;

        // Чем меньше активных сущностей, тем быстрее появляются новые,
        // так как интервал спавна умножается на отношение активных сущностей к размеру пула.
        public bool dynamicInterval;
        public int spawnsPerInterval;
        
        [Header("Area")]
        public float spawnCircleOffset;
        public float spawnCircleRadius;
        
        [Header("Prefab")]
        public GameObject prefab;
    }
}