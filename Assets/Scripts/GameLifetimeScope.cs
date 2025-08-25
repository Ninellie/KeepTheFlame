using Darkness;
using LampFuel;
using PlayerHealth;
using UnityEngine;
using VContainer;
using VContainer.Unity;

public class GameLifetimeScope : LifetimeScope
{
    [SerializeField] private LampFuelConfig lampFuelConfig;
    [SerializeField] private DarknessConfig darknessConfig;
    [SerializeField] private PlayerHealthConfig healthConfig;
    
    protected override void Configure(IContainerBuilder builder)
    {
        var levelDebugGUIGameObject = GetLevelDebugGUIGameObject();
        
        // Lamp
        builder.RegisterInstance(lampFuelConfig);
        builder.Register<LampFuelTank>(Lifetime.Singleton);
        
        var lampFuelGUI = GetLampFuelGUI(levelDebugGUIGameObject);
        builder.RegisterComponent(lampFuelGUI);
        
        builder.RegisterEntryPoint<LampFuelBurner>();
        
        // Darkness
        builder.RegisterInstance(darknessConfig);
        builder.Register<DarknessPower>(Lifetime.Singleton);
        
        var darknessPowerGUI = GetDarknessPowerGUI(levelDebugGUIGameObject);
        builder.RegisterComponent(darknessPowerGUI);
        
        builder.RegisterEntryPoint<DarknessGatherer>();

        // Health
        builder.RegisterInstance(healthConfig);
        builder.Register<PlayerHealth.PlayerHealth>(Lifetime.Singleton);
        
        var healthGUI = GetHealthGUI(levelDebugGUIGameObject);
        builder.RegisterComponent(healthGUI);
    }

    private static GameObject GetLevelDebugGUIGameObject()
    {
        return new GameObject("[LevelDebugGUI]");
    }
    
    private static DebugPlayerHealthGUI GetHealthGUI(GameObject levelDebugGUIGameObject)
    {
        return levelDebugGUIGameObject.AddComponent<DebugPlayerHealthGUI>();
    }
    
    private static DebugLampFuelGUI GetLampFuelGUI(GameObject levelDebugGUIGameObject)
    {
        return levelDebugGUIGameObject.AddComponent<DebugLampFuelGUI>();
    }
    
    private static DebugDarknessPowerGUI GetDarknessPowerGUI(GameObject levelDebugGUIGameObject)
    {
        return levelDebugGUIGameObject.AddComponent<DebugDarknessPowerGUI>();
    }
}