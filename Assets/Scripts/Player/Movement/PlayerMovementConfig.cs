using UnityEngine;

namespace Player.Movement
{
    [CreateAssetMenu(menuName = "Config/Player Movement")]
    public class PlayerMovementConfig : ScriptableObject
    {
        [Range(0, 5)] public float startValue = 0.5f;
    }
}