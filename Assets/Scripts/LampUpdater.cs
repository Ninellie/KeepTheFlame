using VContainer.Unity;

public class LampUpdater : ITickable
{
    private readonly LampFuelService _lamp;
    public LampUpdater(LampFuelService lamp) => _lamp = lamp;
    public void Tick() => _lamp.Tick(UnityEngine.Time.deltaTime);
}