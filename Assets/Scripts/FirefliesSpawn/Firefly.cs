using FirefliesPicking;
using FireflyMovement;
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
        private FireflyMover _mover;
        private EntityPool _pool;

        public void InjectDependencies(FireflyPicker picker, FireflyMover mover, float fuelAmount)
        {
            _picker = picker;
            _mover = mover;
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
            _mover?.RegisterFirefly(this);
            transform.rotation = Quaternion.Euler(0, 0, Random.Range(0f, 360f));
        }

        private void OnDisable()
        {
            _mover?.UnregisterFirefly(this);
        }
    }
}