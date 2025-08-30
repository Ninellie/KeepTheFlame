using Darkness;
using DarknessDamage;
using DebugGUI;
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
        
        // Spawner
        builder.RegisterInstance(spawnerConfig);
        builder.RegisterEntryPoint<FireflySpawner>();
        builder.Register<FireflyPool>(Lifetime.Singleton);
        builder.Register<SpawnTimer>(Lifetime.Singleton);
        builder.Register<IDebugGUIWindow, DebugFireflySpawnerGUI>(Lifetime.Singleton);
            // .Keyed(nameof(DebugFireflySpawnerGUI));
        
        // Lamp
        builder.RegisterInstance(lampFuelConfig);
        builder.RegisterEntryPoint<LampFuelBurner>();
        builder.Register<LampFuelTank>(Lifetime.Singleton);
        builder.Register<IDebugGUIWindow, DebugLampFuelGUI>(Lifetime.Singleton);
            // .Keyed(nameof(DebugLampFuelGUI));
        
        // Darkness
        builder.RegisterInstance(darknessConfig);
        builder.RegisterEntryPoint<DarknessGatherer>();
        builder.Register<DarknessPower>(Lifetime.Singleton);
        builder.Register<IDebugGUIWindow, DebugDarknessPowerGUI>(Lifetime.Singleton);
            // .Keyed(nameof(DebugDarknessPowerGUI));

        // Health
        builder.RegisterInstance(healthConfig);
        builder.Register<PlayerHealthCounter>(Lifetime.Singleton);
        builder.Register<IDebugGUIWindow, DebugPlayerHealthGUI>(Lifetime.Singleton);
            // .Keyed(nameof(DebugPlayerHealthGUI));
        
        // Movement
        builder.RegisterInstance(playerMovementConfig);
        builder.RegisterEntryPoint<PlayerMover>();
        var player = Instantiate(playerMovementInputHandler);
        builder.RegisterComponent(player);
        
        // Player position
        var playerTransform = player.transform;
        builder.RegisterInstance(playerTransform).Keyed("Player");
        
        // Flame
        builder.RegisterInstance(lampFlameConfig);
        builder.Register<LampFlamePower>(Lifetime.Singleton);
        builder.RegisterEntryPoint<LampFlamer>();
        builder.Register<IDebugGUIWindow, DebugLampFlameGUI>(Lifetime.Singleton);
            // .Keyed(nameof(DebugLampFlameGUI));
        
        // Darkness Damage
        builder.RegisterInstance(darknessDamageConfig);
        builder.RegisterEntryPoint<DarknessDamageDealer>();
        builder.Register<DebugDarknessDamageGUI>(Lifetime.Singleton).AsImplementedInterfaces()
            .AsSelf();
            // .Keyed(nameof(DebugDarknessDamageGUI));
    }
}