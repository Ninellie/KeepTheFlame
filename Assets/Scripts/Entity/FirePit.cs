using Darkness;
using Entity;
using LampFuel;
using Player.Interacting;
using TriInspector;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using VContainer;

namespace FirePitSpawn
{
    public class FirePit : MonoBehaviour
    {
        [SerializeField] private FirePitConfig config;
        [SerializeField] private Light2D ember;
        [SerializeField] private Light2D fire;
        [SerializeField] private float fireIntensity = 5;
        
        [ReadOnly] [SerializeField] private bool isBurned;
        [ReadOnly] [SerializeField] private bool isBurning;

        private DarknessPower _darknessPower;
        private LampFuelTank _fuelTank;
        private Interactable _interactable;

        private float _burningTimeLeft;
        
        private void Awake()
        {
            _interactable = GetComponent<Interactable>();
        }

        private void Start()
        {
            isBurning = false;
            isBurned = false;
        }
        
        private void FixedUpdate()
        {
            if (!isBurning) return; // Выйти если не горит
            if (isBurned) return; // Выйти если уже сгорел
            
            _burningTimeLeft -= Time.deltaTime;
            _darknessPower.Decrease(config.darknessResistancePerSecond * Time.deltaTime);
            var intensityScale = ParabolicNormalized(0, config.burningDuration, _burningTimeLeft);
            fire.intensity = intensityScale * fireIntensity;
            if (_burningTimeLeft > 0) return;
            isBurned = true;
            isBurning = false;
            fire.gameObject.SetActive(false);
        }
        
        private void Burn()
        {
            if (isBurning) return;
            if (isBurned) return;
            
            // Если не хватает топлива
            if (_fuelTank.Value < config.fuelCost) return;
            
            ember.gameObject.SetActive(false);
            
            fire.intensity = 0;
            fire.gameObject.SetActive(true);
            
            _fuelTank.Subtract(config.fuelCost);
            _burningTimeLeft = config.burningDuration;
            isBurning = true;
        }
        
        private void OnEnable()
        {
            _interactable.OnInteractAction += Burn;
        }

        private void OnDisable()
        {
            if (_interactable != null)
            {
                _interactable.OnInteractAction -= Burn;
            }
        }
        
        private static float ParabolicNormalized(float min, float max, float current)
        {
            if (Mathf.Approximately(max, min))
                return 0f; // защита от деления на 0

            var t = Mathf.Clamp01((current - min) / (max - min));
            return 4f * t * (1f - t);
        }
        
        [Inject]
        public void InjectDependencies(DarknessPower darknessPower, LampFuelTank fuelTank)
        {
            _darknessPower = darknessPower;
            _fuelTank = fuelTank;
        }
    }
}