using UnityEngine;

namespace Entity.Movement
{
    [CreateAssetMenu(menuName = "Config/Movement")]
    public class MovementConfig : ScriptableObject
    {
        [Range(0, 20)] public float MovementSpeed = 5f;
        [Range(0, 20)] public float RotationSpeed = 5f;
    }
}
