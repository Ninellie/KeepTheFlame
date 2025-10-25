using FirefliesPicking;
using Spawning;
using UnityEngine;

namespace FirefliesSpawn
{
    public class Firefly : MonoBehaviour, IPooledEntity
    {
        public Vector2Int Sector { get; set; }
        public Transform Transform => transform;
        public GameObject GameObject => gameObject;
        
        public float FuelAmount { get; private set; }
        
        private FireflyPool _pool;
        private FireflyPicker _picker;
    
        public void Init(FireflyPool pool, FireflyPicker picker, float fuelAmount)
        {
            _pool = pool;
            _picker = picker;
            FuelAmount = fuelAmount;
        }
    
        private void OnTriggerEnter2D(Collider2D other)
        {
            if (!other.CompareTag("Player")) return;
            
            _picker.PickUp(this);
            _pool.ReturnToPool(this);
        }
    }
}