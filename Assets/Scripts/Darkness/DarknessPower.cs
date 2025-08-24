using UnityEngine;

namespace Darkness
{
    public class DarknessPower
    {
        public float Value { get; private set; }
        public float Min { get; private set; }
        public float Max { get; private set; }
        
        public event System.Action<float> OnChanged;
        
        private readonly DarknessConfig _config;

        public DarknessPower(DarknessConfig config)
        {
            _config = config;
            Init();
        }

        public void Init()
        {
            Max = _config.maxValue;
            Min  = _config.minValue;
            Value = Mathf.Clamp(_config.startValue, Min, Max);
        }
        
        public void Increase(float amount) => SetValue(Value + amount);

        public void Decrease(float amount) => SetValue(Value - amount);
    
        private void SetValue(float v)
        {
            var clamped = Mathf.Clamp(v, Min, Max);
            if (Mathf.Approximately(clamped, Value)) return;
            Value = clamped;
            OnChanged?.Invoke(Value);
        }
    }
}