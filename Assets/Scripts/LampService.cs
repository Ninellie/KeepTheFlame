using UnityEngine;

public class LampService : ILamp
{
    public float Current { get; private set; }
    public float Max { get; }
    
    public event System.Action<float> OnChanged;

    private readonly float _decayPerSecond;
    
    public LampService(LampConfig config)
    {
        Max = Mathf.Max(0, config.maxValue);
        Current = Mathf.Clamp(config.startValue, 0, Max);
        _decayPerSecond = Mathf.Max(0, config.decayPerSecond);
    }

    public void Tick(float deltaTime)
    {
        if (Current <= 0f || _decayPerSecond <= 0f) return;
        SetValue(Current - _decayPerSecond * deltaTime);
    }

    public void Add(float amount) => SetValue(Current + amount);

    private void SetValue(float v)
    {
        var clamped = Mathf.Clamp(v, 0f, Max);
        if (Mathf.Approximately(clamped, Current)) return;
        Current = clamped;
        OnChanged?.Invoke(Current);
    }
}