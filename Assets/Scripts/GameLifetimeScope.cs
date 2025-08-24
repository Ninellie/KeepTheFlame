using Lamp;
using UnityEngine;
using VContainer;
using VContainer.Unity;

public class GameLifetimeScope : LifetimeScope
{
    [SerializeField] private LampFuelConfig lampFuelConfig;
    
    protected override void Configure(IContainerBuilder builder)
    {
        builder.RegisterInstance(lampFuelConfig);
        builder.Register<LampFuel>(Lifetime.Singleton);
        
        var lampFuelGUI = GetLampFuelGUI();
        builder.RegisterComponent(lampFuelGUI);
        builder.RegisterEntryPoint<LampFuelBurner>();
    }

    private static DebugLampFuelGUI GetLampFuelGUI()
    {
        var levelDebugGUIGameObject = new GameObject("[LevelDebugGUI]");
        var lampFuelGUI = levelDebugGUIGameObject.AddComponent<DebugLampFuelGUI>();
        return lampFuelGUI;
    }
}