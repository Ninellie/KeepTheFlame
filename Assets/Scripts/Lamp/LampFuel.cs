using UnityEngine;

namespace Lamp
{
    public class LampFuel
    {
        public float Value { get; private set; }
        public float Min { get; private set; }
        public float Max { get; private set; }

        public event System.Action<float> OnChanged;

        private readonly LampFuelConfig _config;
    
        public LampFuel(LampFuelConfig fuelConfig)
        {
            _config = fuelConfig;
            Init();
        }

        public void Init()
        {
            Max = _config.maxValue;
            Min  = _config.minValue;
            Value = Mathf.Clamp(_config.startValue, Min, Max);
        }
    
        public void Add(float amount) => SetValue(Value + amount);

        public void Subtract(float amount) => SetValue(Value - amount);
    
        private void SetValue(float v)
        {
            var clamped = Mathf.Clamp(v, Min, Max);
            if (Mathf.Approximately(clamped, Value)) return;
            Value = clamped;
            OnChanged?.Invoke(Value);
        }
    }
}