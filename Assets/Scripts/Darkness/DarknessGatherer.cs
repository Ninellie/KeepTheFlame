using UnityEngine;
using VContainer.Unity;

namespace Darkness
{
    public class DarknessGatherer : IFixedTickable
    {
        public float PowerIncreaseRate => _powerIncreaseRateCurve.Evaluate(Time.timeSinceLevelLoad);
        
        private readonly DarknessPower _darkness;
        private readonly DarknessConfig _config;
        
        private AnimationCurve _powerIncreaseRateCurve { get; set; }
        
        public DarknessGatherer(DarknessPower darkness, DarknessConfig config)
        {
            _darkness = darkness;
            _config = config;
            
            Init();
        }

        private void Init()
        {
            _powerIncreaseRateCurve = _config.PowerIncreaseRateCurve;
        }

        public void FixedTick()
        {
            if (_darkness.Value >= _darkness.Max) return;
            if (PowerIncreaseRate <= 0f) return;
            _darkness.Increase(PowerIncreaseRate * Time.deltaTime);
        }
    }
}