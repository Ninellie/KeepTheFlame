using FirefliesPicking;
using UnityEngine;
using VContainer;

namespace FirefliesSpawn
{
    public class Firefly : MonoBehaviour
    {
        [SerializeField] private float fuelAmount;

        public float FuelAmount
        {
            get => fuelAmount;
            set => fuelAmount = value;
        }
        
        private FireflyPicker _picker;

        [Inject]
        public void SetPicker(FireflyPicker picker)
        {
            _picker = picker;
        }
        
        private void OnTriggerEnter2D(Collider2D other)
        {
            if (!other.CompareTag("Player")) return;
            
            _picker.PickUp(this);
            Destroy(gameObject);
        }

        private void OnEnable()
        {
            transform.rotation = Quaternion.Euler(0, 0, Random.Range(0f, 360f));
        }
    }
}