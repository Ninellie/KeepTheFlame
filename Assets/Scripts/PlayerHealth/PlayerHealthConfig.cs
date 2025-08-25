using UnityEngine;

namespace PlayerHealth
{
    [CreateAssetMenu(menuName = "Config/PlayerHealthConfig")]
    public class PlayerHealthConfig : ScriptableObject
    {
        [Range(0, 20)] public int maxValue = 3;
        [Range(0, 20)] public int minValue = 0;
        [Range(0, 20)] public int startValue = 3;
    }
}