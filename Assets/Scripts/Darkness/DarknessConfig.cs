using UnityEngine;

namespace Darkness
{
    [CreateAssetMenu(menuName = "Config/DarknessConfig")]
    public class DarknessConfig : ScriptableObject
    {
        [Range(0, 1000)] public float maxValue = 100f;
        [Range(-200, 200)] public float minValue = 0f;
        [Range(0, 1000)] public float startValue = 5f;
        
        public AnimationCurve PowerIncreaseRateCurve;
    }
}