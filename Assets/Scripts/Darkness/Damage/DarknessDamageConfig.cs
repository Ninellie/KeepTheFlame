using UnityEngine;

namespace Darkness.Damage
{
    [CreateAssetMenu(menuName = "Config/Darkness Damage")]
    public class DarknessDamageConfig : ScriptableObject
    {
        [Range(0, 10)] public int damage = 1;
        [Range(0, 5)] public float damageDealInterval = 5;
    }
}