using Darkness;
using DarknessDamage;
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
        
        // Камера
        builder.RegisterInstance(Camera.main);
        
        // Spawner
        builder.RegisterInstance(spawnerConfig);
        builder.Register<FireflyPool>(Lifetime.Singleton);
        builder.Register<SpawnTimer>(Lifetime.Singleton);
        builder.RegisterEntryPoint<FireflySpawner>();
        var spawnerGUI = levelDebugGUIGameObject.AddComponent<DebugFireflySpawnerGUI>();
        builder.RegisterComponent(spawnerGUI);
        
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
        builder.Register<PlayerHealthCounter>(Lifetime.Singleton);
        var healthGUI = levelDebugGUIGameObject.AddComponent<DebugPlayerHealthGUI>(); 
        builder.RegisterComponent(healthGUI);
        
        // Movement
        builder.RegisterInstance(playerMovementConfig);
        var player = Instantiate(playerMovementInputHandler);
        var playerTransform = player.transform;
        builder.RegisterInstance(playerTransform).Keyed("Player");
        builder.RegisterComponent(player);
        builder.RegisterEntryPoint<PlayerMover>();
        
        // Flame
        builder.RegisterInstance(lampFlameConfig);
        builder.Register<LampFlamePower>(Lifetime.Singleton);
        builder.RegisterEntryPoint<LampFlamer>();
        var flameDebugGUI = levelDebugGUIGameObject.AddComponent<DebugLampFlameGUI>();
        builder.RegisterComponent(flameDebugGUI);
        
        // Darkness Damage
        builder.RegisterInstance(darknessDamageConfig);
        builder.RegisterEntryPoint<DarknessDamageDealer>();
        var darknessDamageDebugGUI = levelDebugGUIGameObject.AddComponent<DebugDarknessDamageGUI>();
        builder.RegisterComponent(darknessDamageDebugGUI);
    }
}