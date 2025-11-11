using PlayerHealth;
using UnityEngine;
using VContainer;

namespace ScaryTreeSpawn
{
    public class ScaryTree : MonoBehaviour
    {
        [SerializeField] private int damage;

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