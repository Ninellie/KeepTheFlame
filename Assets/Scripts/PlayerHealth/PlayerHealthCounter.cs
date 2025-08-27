using UnityEngine;

namespace PlayerHealth
{
    public class PlayerHealthCounter
    {
        public int Value { get; private set; }
        public int Min { get; private set; }
        public int Max { get; private set; }
        
        public event System.Action<int> OnChanged;
        public event System.Action<int> OnEmpty;
        
        private readonly PlayerHealthConfig _config;

        public PlayerHealthCounter(PlayerHealthConfig config)
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
        
        public void Increase(int amount) => SetValue(Value + amount);

        public void Decrease(int amount) => SetValue(Value - amount);
    
        private void SetValue(int v)
        {
            var clamped = Mathf.Clamp(v, Min, Max);
            if (Mathf.Approximately(clamped, Value)) return;
            Value = clamped;
            OnChanged?.Invoke(Value);

            if (Mathf.Approximately(Value, Min)) return;
            OnEmpty?.Invoke(Value);
        }
    }
}