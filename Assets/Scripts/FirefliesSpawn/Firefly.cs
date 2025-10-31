using FirefliesPicking;
using Spawning;
using UnityEngine;

namespace FirefliesSpawn
{
    public class Firefly : MonoBehaviour, IPooledEntity
    {
        [SerializeField] private float fuelAmount;

        public float FuelAmount
        {
            get => fuelAmount;
            set => fuelAmount = value;
        }
        
        public Vector2Int Sector { get; set; }
        public Transform Transform => transform;
        public GameObject GameObject => gameObject;
        public EntityPool Pool { get; set; }

        private FireflyPicker _picker;

        public void SetPicker(FireflyPicker picker)
        {
            _picker = picker;
        }
        
        private void OnTriggerEnter2D(Collider2D other)
        {
            if (!other.CompareTag("Player")) return;
            
            _picker.PickUp(this);
            Pool.ReturnToPool(this);
        }

        private void OnEnable()
        {
            transform.rotation = Quaternion.Euler(0, 0, Random.Range(0f, 360f));
        }
    }
}