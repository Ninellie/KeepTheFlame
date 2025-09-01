using TriInspector;
using UnityEngine;

namespace FirePitSpawn
{
    [CreateAssetMenu(menuName = "Config/FirePit")]
    public class FirePitConfig : ScriptableObject
    {
        [Range(0, 10)] public float fuelCost;
        [Range(0, 10)] public float burningDuration;
        [Range(0, 3)] public float darknessResistancePerSecond;
        [ReadOnly] public float totalDarknessResistance => burningDuration * darknessResistancePerSecond;
    }
}