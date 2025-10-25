using FirefliesPicking;
using UnityEngine;

namespace FirefliesSpawn
{
    public class Firefly : MonoBehaviour
    {
        public Vector2Int Sector { get; set; }
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