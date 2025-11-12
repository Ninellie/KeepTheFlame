using LampFuel;
using UnityEngine;
using VContainer;

namespace Entity
{
    public class Firefly : MonoBehaviour
    {
        [SerializeField] private float fuelAmount;

        private LampFuelTank _fuel;

        private void OnEnable()
        {
            transform.rotation = Quaternion.Euler(0, 0, Random.Range(0f, 360f));
        }
        
        [Inject]
        public void InjectDependencies(LampFuelTank fuel)
        {
            _fuel = fuel;
        }
        
        private void OnTriggerEnter2D(Collider2D other)
        {
            if (!other.CompareTag("Player")) return;
            _fuel.Add(fuelAmount);
            Destroy(gameObject);
        }
    }
}