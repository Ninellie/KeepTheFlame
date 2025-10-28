using System;
using LampFlame;
using UnityEngine;
using VContainer;

namespace LampFlameUi
{
    public class LampFlameUiView : MonoBehaviour
    {
        [SerializeField] private float _maxStartSpeed;
        [SerializeField] private float _maxStartSize;
        [SerializeField] private float _maxRateOverTime;
        
        private LampFlamePower _flame;
        
        private ParticleSystem _particleSystem;

        private void Awake()
        {
            _particleSystem = GetComponent<ParticleSystem>();
        }
        
        [Inject]
        public void Inject(LampFlamePower flame)
        {
            _flame = flame;
            _flame.OnChanged += Set;
        }

        private void OnDisable()
        {
            _flame.OnChanged -= Set;
        }

        private void Set(float flamePower)
        {
            var flamePowerPercent = (flamePower - _flame.Min) / (_flame.Max - _flame.Min);
            var main = _particleSystem.main;
            main.startSpeedMultiplier = _maxStartSpeed * flamePowerPercent;
            main.startSizeMultiplier = _maxStartSize * flamePowerPercent;
            var emission = _particleSystem.emission;
            emission.rateOverTimeMultiplier = _maxRateOverTime * flamePowerPercent;
        }
    }
}