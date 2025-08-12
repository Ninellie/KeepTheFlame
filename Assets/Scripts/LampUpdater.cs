using VContainer.Unity;

public class LampUpdater : ITickable
{
    private readonly ILamp _lamp;
    public LampUpdater(ILamp lamp) => _lamp = lamp;
    public void Tick() => _lamp.Tick(UnityEngine.Time.deltaTime);
}