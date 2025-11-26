using UnityEngine;

namespace Darkness
{
    public class DarknessPower
    {
        public float Value { get; private set; }
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
            Value = Mathf.Clamp(_config.startValue, 0, Max);
        }
        
        public void Increase(float amount) => SetValue(Value + amount);

        public void Decrease(float amount) => SetValue(Value - amount);
    
        private void SetValue(float v)
        {
            var clamped = Mathf.Clamp(v, 0, Max);
            if (Mathf.Approximately(clamped, Value)) return;
            Value = clamped;
            OnChanged?.Invoke(Value);
        }
    }
}