using PlayerHealth;
using Spawning;
using UnityEngine;

namespace ScaryTreeSpawn
{
    public class ScaryTree : MonoBehaviour, IPooledEntity
    {
        [SerializeField] private int damage;
        
        public int Damage
        {
            set => damage = value;
        }
        
        public Vector2Int Sector { get; set; }
        public Transform Transform => transform;
        public GameObject GameObject => gameObject;
        public EntityPool Pool { get; set; }

        public PlayerHealthCounter PlayerHealthCounter;

        private void OnCollisionEnter2D(Collision2D other)
        {
            if (!other.collider.CompareTag("PlayerFeet")) return;
            
            PlayerHealthCounter.Decrease(damage);
        }
    }
}