using UnityEngine;

namespace Darkness
{
    [CreateAssetMenu(menuName = "Config/Darkness Power")]
    public class DarknessConfig : ScriptableObject
    {
        [Range(0, 20)] public float maxValue = 10f;
        [Range(0, 0)] public float startValue;
        
        public AnimationCurve PowerIncreaseRateCurve;
    }
}