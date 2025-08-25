using UnityEngine;

namespace Lamp
{
    [CreateAssetMenu(menuName = "Config/LampConfig")]
    public class LampFuelConfig : ScriptableObject
    {
        [Range(0, 20)] public float maxValue = 10f;
        [Range(-20, 20)] public float minValue = 0f;
        [Range(0, 20)] public float startValue = 5f;
        [Range(0, 1)] public float decayPerSecond = 0.25f;
    }
}