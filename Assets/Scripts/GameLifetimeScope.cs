using Darkness;
using DarknessDamage;
using DebugGUI;
using FirefliesFuelReplenish;
using FirefliesSpawn;
using LampFlame;
using LampFuel;
using PlayerHealth;
using PlayerMovement;
using UnityEngine;
using VContainer;
using VContainer.Unity;

public class GameLifetimeScope : LifetimeScope
{
    [SerializeField] private LampFuelConfig lampFuelConfig;
    [SerializeField] private LampFlameConfig lampFlameConfig;
    [SerializeField] private DarknessConfig darknessConfig;
    [SerializeField] private PlayerHealthConfig healthConfig;
    [SerializeField] private PlayerMovementConfig playerMovementConfig;
    [SerializeField] private DarknessDamageConfig darknessDamageConfig;
    [SerializeField] private SpawnerConfig spawnerConfig;
    
    [SerializeField] private PlayerMovementInputHandler playerMovementInputHandler;
    
    protected override void Configure(IContainerBuilder builder)
    {
        var levelDebugGUIGameObject = new GameObject("[LevelDebugGUI]");
        var debugGUIPositioner = levelDebugGUIGameObject.AddComponent<DebugGUIController>();
        builder.RegisterComponent(debugGUIPositioner);
        
        // Камера
        builder.RegisterInstance(Camera.main);
        
        // Movement
        builder.RegisterInstance(playerMovementConfig);
        builder.RegisterEntryPoint<PlayerMover>();
        var player = Instantiate(playerMovementInputHandler);
        builder.RegisterComponent(player);
        
        // Player position
        var playerTransform = player.transform;
        builder.RegisterInstance(playerTransform).Keyed("Player");
        if (Camera.main != null) Camera.main.transform.SetParent(playerTransform);
        
        // Spawner
        builder.RegisterInstance(spawnerConfig);
        builder.RegisterEntryPoint<FireflySpawner>();
        builder.Register<FireflyPool>(Lifetime.Singleton);
        builder.Register<SpawnTimer>(Lifetime.Singleton);
        builder.Register<IDebugGUIWindow, DebugFireflySpawnerGUI>(Lifetime.Singleton);
        
        // Fuel
        builder.RegisterInstance(lampFuelConfig);
        builder.RegisterEntryPoint<LampFuelBurner>();
        builder.Register<LampFuelTank>(Lifetime.Singleton);
        builder.Register<IDebugGUIWindow, DebugLampFuelGUI>(Lifetime.Singleton);
        
        // Replenish
        builder.RegisterEntryPoint<FireflyFuelReplenisher>();
        
        // Flame
        builder.RegisterInstance(lampFlameConfig);
        builder.Register<LampFlamePower>(Lifetime.Singleton);
        builder.RegisterEntryPoint<LampFlamer>();
        builder.Register<IDebugGUIWindow, DebugLampFlameGUI>(Lifetime.Singleton);
        
        // Darkness
        builder.RegisterInstance(darknessConfig);
        builder.RegisterEntryPoint<DarknessGatherer>();
        builder.Register<DarknessPower>(Lifetime.Singleton);
        builder.Register<IDebugGUIWindow, DebugDarknessPowerGUI>(Lifetime.Singleton);

        // Health
        builder.RegisterInstance(healthConfig);
        builder.Register<PlayerHealthCounter>(Lifetime.Singleton);
        builder.Register<IDebugGUIWindow, DebugPlayerHealthGUI>(Lifetime.Singleton);
        
        // Darkness Damage
        builder.RegisterInstance(darknessDamageConfig);
        builder.RegisterEntryPoint<DarknessDamageDealer>();
        builder.Register<DebugDarknessDamageGUI>(Lifetime.Singleton).AsImplementedInterfaces()
            .AsSelf();
    }
}