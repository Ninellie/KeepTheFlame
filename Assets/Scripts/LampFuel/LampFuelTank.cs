using UnityEngine;

namespace LampFuel
{
    public class LampFuelTank
    {
        public float Value { get; private set; }
        public float Min { get; private set; }
        public float Max { get; private set; }
        
        public event System.Action<float> OnChanged;
        public event System.Action OnReplenish;
        public event System.Action OnEmpty;
        public event System.Action OnNonEmpty;
        
        private readonly LampFuelConfig _config;
    
        public LampFuelTank(LampFuelConfig fuelConfig)
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
            
            var oldValue = Value;
            Value = clamped;
            
            OnChanged?.Invoke(Value);
            
            if (Value > oldValue)
            {
                OnReplenish?.Invoke();
            }
            
            // Если было пусто
            if (Mathf.Approximately(oldValue, Min))
            {
                OnNonEmpty?.Invoke();
                return;
            }
            
            if (!Mathf.Approximately(Value, Min)) return;
            OnEmpty?.Invoke();
        }
    }
}