using UnityEngine;

[CreateAssetMenu(menuName = "Config/LampConfig")]
public class LampConfig : ScriptableObject
{
    [Range(0, 20)] public float maxValue = 10f;
    [Range(0, 20)] public float startValue = 10f;
    [Range(0, 1)] public float decayPerSecond = 0.25f;
}