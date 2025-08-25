using Darkness;
using LampFuel;
using PlayerHealth;
using PlayerMovement;
using UnityEngine;
using VContainer;
using VContainer.Unity;

public class GameLifetimeScope : LifetimeScope
{
    [SerializeField] private LampFuelConfig lampFuelConfig;
    [SerializeField] private DarknessConfig darknessConfig;
    [SerializeField] private PlayerHealthConfig healthConfig;
    [SerializeField] private PlayerMovementConfig playerMovementConfig;
    
    [SerializeField] private PlayerMovementInputHandler playerMovementInputHandler;
    
    protected override void Configure(IContainerBuilder builder)
    {
        var levelDebugGUIGameObject = new GameObject("[LevelDebugGUI]");
        
        // Lamp
        builder.RegisterInstance(lampFuelConfig);
        builder.Register<LampFuelTank>(Lifetime.Singleton);
        
        var lampFuelGUI = levelDebugGUIGameObject.AddComponent<DebugLampFuelGUI>();
        builder.RegisterComponent(lampFuelGUI);
        
        builder.RegisterEntryPoint<LampFuelBurner>();
        
        // Darkness
        builder.RegisterInstance(darknessConfig);
        builder.Register<DarknessPower>(Lifetime.Singleton);

        var darknessPowerGUI = levelDebugGUIGameObject.AddComponent<DebugDarknessPowerGUI>();
        builder.RegisterComponent(darknessPowerGUI);
        
        builder.RegisterEntryPoint<DarknessGatherer>();

        // Health
        builder.RegisterInstance(healthConfig);
        builder.Register<PlayerHealth.PlayerHealth>(Lifetime.Singleton);

        var healthGUI = levelDebugGUIGameObject.AddComponent<DebugPlayerHealthGUI>(); 
        builder.RegisterComponent(healthGUI);
        
        // Movement
        builder.RegisterInstance(playerMovementConfig);
        var player = Instantiate(playerMovementInputHandler);
        var playerTransform = new PlayerTransform(player.transform);
        builder.RegisterInstance(playerTransform);
        builder.RegisterComponent(player);
        builder.RegisterEntryPoint<PlayerMover>();
    }
}