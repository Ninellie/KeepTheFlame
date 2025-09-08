using Darkness;
using Interacting;
using LampFuel;
using TriInspector;
using UnityEngine;
using UnityEngine.Rendering.Universal;

namespace FirePitSpawn
{
    public class FirePit : MonoBehaviour
    {
        [SerializeField] private FirePitConfig config;
        [SerializeField] private Light2D ember;
        [SerializeField] private Light2D fire;
        
        [ReadOnly] [SerializeField] private bool isBurned;
        [ReadOnly] [SerializeField] private bool isBurning;
        public Vector2Int Sector { get; set; }
        
        private DarknessPower _darknessPower;
        private LampFuelTank _fuelTank;
        private Interactable _interactable;
        
        private float _burningDuration;
        private float _darknessResistancePerSecond;
        private float _fuelCost;

        private float _burningTimeLeft;

        public void InjectDependencies(DarknessPower darknessPower, LampFuelTank fuelTank)
        {
            _darknessPower = darknessPower;
            _fuelTank = fuelTank;
        }
        
        public void Configure()
        {
            _burningDuration = config.burningDuration;
            _darknessResistancePerSecond = config.darknessResistancePerSecond;
            _fuelCost = config.fuelCost;
        }

        private void Awake()
        {
            _interactable = GetComponent<Interactable>();
        }

        private void Start()
        {
            isBurning = false;
            isBurned = false;
            Configure();
        }

        private void OnEnable()
        {
            _interactable.OnInteractAction += Burn;
        }

        private void OnDisable()
        {
            _interactable.OnInteractAction -= Burn;
        }

        private void Burn()
        {
            if (isBurning) return;
            if (isBurned) return;
            
            // Если не хватает топлива
            if (_fuelTank.Value < _fuelCost) return;
            
            ember.gameObject.SetActive(false);
            fire.gameObject.SetActive(true);
            
            _fuelTank.Subtract(_fuelCost);
            _burningTimeLeft = _burningDuration;
            isBurning = true;
        }

        private void FixedUpdate()
        {
            if (!isBurning) return; // Выйти если не горит
            if (isBurned) return; // Выйти если уже сгорел
            
            var deltaTime = Time.deltaTime;
            _burningTimeLeft -= deltaTime;
            _darknessPower.Decrease(_darknessResistancePerSecond * deltaTime);

            if (_burningTimeLeft > 0) return;
            isBurned = true;
            isBurning = false;
            ember.gameObject.SetActive(true);
            fire.gameObject.SetActive(false);
        }
    }
}