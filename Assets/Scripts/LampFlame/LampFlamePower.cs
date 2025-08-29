﻿using UnityEngine;

namespace LampFlame
{
    public class LampFlamePower
    {
        public float Value { get; private set; }
        public float Max { get; private set; }
        public float Min { get; private set; }
        public bool IsLit => !Mathf.Approximately(Value, Min);
        
        public event System.Action<float> OnChanged;
        public event System.Action OnExtinguished;
        public event System.Action OnLit;
        
        private readonly LampFlameConfig _config;
        
        public LampFlamePower(LampFlameConfig config)
        {
            _config = config;
            
            Init();
        }
        
        public void Init()
        {
            Max = _config.maxValue;
            Min = _config.minValue;
        }
        
        public void SetValue(float v)
        {
            var clamped = Mathf.Clamp(v, Min, Max);
            if (Mathf.Approximately(clamped, Value)) return;
            
            var oldValue = Value;
            Value = clamped;
            OnChanged?.Invoke(Value);
            
            // Если было пусто
            if (Mathf.Approximately(oldValue, Min))
            {
                OnLit?.Invoke();
                return;
            }
            
            if (!Mathf.Approximately(Value, Min)) return;
            OnExtinguished?.Invoke();
        }
    }
}