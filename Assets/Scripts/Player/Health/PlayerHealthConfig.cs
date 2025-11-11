using UnityEngine;

namespace Player.Health
{
    [CreateAssetMenu(menuName = "Config/Player Health")]
    public class PlayerHealthConfig : ScriptableObject
    {
        [Range(0, 20)] public int maxValue = 3;
        [Range(0, 20)] public int minValue = 0;
        [Range(0, 20)] public int startValue = 3;
    }
}