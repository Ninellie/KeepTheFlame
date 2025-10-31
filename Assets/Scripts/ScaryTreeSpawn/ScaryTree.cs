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
        
        private void OnTriggerEnter2D(Collider2D other)
        {
            if (!other.CompareTag("Player")) return;
            
            PlayerHealthCounter.Decrease(damage);
            Pool.ReturnToPool(this);
        }
    }
}