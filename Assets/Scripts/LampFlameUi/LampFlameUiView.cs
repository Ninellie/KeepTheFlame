using LampFlame;
using UnityEngine;
using VContainer;

namespace LampFlameUi
{
    public class LampFlameUiView : MonoBehaviour
    {
        [SerializeField] private float maxStartSpeed = 7;
        [SerializeField] private float maxStartSize = 5;
        [SerializeField] private float maxRateOverTime = 20;
        
        private LampFlamePower _flame;
        private ParticleSystem _particleSystem;

        private void Awake()
        {
            _particleSystem = GetComponent<ParticleSystem>();
        }

        private void OnDisable()
        {
            if (_flame != null)
            {
                _flame.OnChanged -= Set;
            }
        }
        
        [Inject]
        public void Inject(LampFlamePower flame)
        {
            _flame = flame;
            _flame.OnChanged += Set;
        }

        private void Set(float flamePower)
        {
            var flamePowerPercent = (flamePower - _flame.Min) / (_flame.Max - _flame.Min);
            var main = _particleSystem.main;
            main.startSpeedMultiplier = maxStartSpeed * flamePowerPercent;
            main.startSizeMultiplier = maxStartSize * flamePowerPercent;
            var emission = _particleSystem.emission;
            emission.rateOverTimeMultiplier = maxRateOverTime * flamePowerPercent;
        }
    }
}