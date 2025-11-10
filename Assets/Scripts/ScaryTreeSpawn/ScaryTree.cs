using PlayerHealth;
using Spawning;
using UnityEngine;
using VContainer;

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

        private PlayerHealthCounter _playerHealthCounter;

        [Inject]
        public void InjectDependencies(PlayerHealthCounter playerHealthCounter)
        {
            _playerHealthCounter = playerHealthCounter;
        }
        
        private void OnCollisionEnter2D(Collision2D other)
        {
            if (!other.collider.CompareTag("PlayerFeet")) return;
            
            _playerHealthCounter.Decrease(damage);
        }
    }
}