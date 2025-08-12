using UnityEngine;
using VContainer;
using VContainer.Unity;

public class LevelDebugGuiEntryPoint : IStartable
{
    private readonly IObjectResolver _resolver;
    private GameObject _go;

    public LevelDebugGuiEntryPoint(IObjectResolver resolver)
    {
        _resolver = resolver;
    }

    public void Start()
    {
        _go = new GameObject("[LevelDebugGUI]");
        var gui = _go.AddComponent<DebugLampGUI>();
        _resolver.Inject(gui); // инжектит ILamp из текущего (уровневого/родительского) scope
    }
}