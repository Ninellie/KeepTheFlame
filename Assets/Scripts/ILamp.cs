public interface ILamp
{
    float Current { get; }
    float Max { get; }
    event System.Action<float> OnChanged;
    void Tick(float deltaTime);
    void Add(float amount);
}