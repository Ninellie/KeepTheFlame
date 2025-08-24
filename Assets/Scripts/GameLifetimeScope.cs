using Darkness;
using Lamp;
using UnityEngine;
using VContainer;
using VContainer.Unity;

public class GameLifetimeScope : LifetimeScope
{
    [SerializeField] private LampFuelConfig lampFuelConfig;
    [SerializeField] private DarknessConfig darknessConfig;
    
    protected override void Configure(IContainerBuilder builder)
    {
        var levelDebugGUIGameObject = GetLevelDebugGUIGameObject();
        
        // Lamp
        builder.RegisterInstance(lampFuelConfig);
        builder.Register<LampFuel>(Lifetime.Singleton);
        
        var lampFuelGUI = GetLampFuelGUI(levelDebugGUIGameObject);
        builder.RegisterComponent(lampFuelGUI);
        builder.RegisterEntryPoint<LampFuelBurner>();
        
        // Darkness
        builder.RegisterInstance(darknessConfig);
        builder.Register<DarknessPower>(Lifetime.Singleton);

        var darknessPowerGUI = GetDarknessPowerGUI(levelDebugGUIGameObject);
        builder.RegisterComponent(darknessPowerGUI);
        builder.RegisterEntryPoint<DarknessGatherer>();

    }

    private static GameObject GetLevelDebugGUIGameObject()
    {
        return new GameObject("[LevelDebugGUI]");
    }
    
    private static DebugLampFuelGUI GetLampFuelGUI(GameObject levelDebugGUIGameObject)
    {
        var lampFuelGUI = levelDebugGUIGameObject.AddComponent<DebugLampFuelGUI>();
        return lampFuelGUI;
    }
    
    private static DebugDarknessPowerGUI GetDarknessPowerGUI(GameObject levelDebugGUIGameObject)
    {
        var darknessPowerGUI = levelDebugGUIGameObject.AddComponent<DebugDarknessPowerGUI>();
        return darknessPowerGUI;
    }
}