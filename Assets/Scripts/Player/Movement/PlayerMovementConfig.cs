using UnityEngine;

namespace Player.Movement
{
    [CreateAssetMenu(menuName = "Config/Player Movement")]
    public class PlayerMovementConfig : ScriptableObject
    {
        [Range(0, 20)] public float maxValue = 10f;
        [Range(0, 20)] public float minValue = 0f;
        [Range(0, 20)] public float startValue = 5f;
    }
}