using UnityEngine;

namespace FireflyMovement
{
    [CreateAssetMenu(menuName = "Config/Firefly Movement")]
    public class FireflyMovementConfig : ScriptableObject
    {
        [Range(0, 20)] public float maxMovementSpeed = 10f;
        [Range(0, 20)] public float minMovementSpeed = 0f;
        [Range(0, 20)] public float startMovementSpeed = 5f;
        
        [Range(0, 20)] public float maxRotationSpeed = 10f;
        [Range(0, 20)] public float minRotationSpeed = 0f;
        [Range(0, 20)] public float startRotationSpeed = 5f;
    }
}
