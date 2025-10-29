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
        
        private FireflyPicker _picker;
        private EntityPool _pool;

        public void InjectDependencies(FireflyPicker picker, float fuelAmount)
        {
            _picker = picker;
            FuelAmount = fuelAmount;
        }
    
        public void SetPool(EntityPool pool)
        {
            _pool = pool;
        }
        
        private void OnTriggerEnter2D(Collider2D other)
        {
            if (!other.CompareTag("Player")) return;
            
            _picker.PickUp(this);
            _pool.ReturnToPool(this);
        }

        private void OnEnable()
        {
            transform.rotation = Quaternion.Euler(0, 0, Random.Range(0f, 360f));
        }
    }
}