using Darkness;
using DarknessDamage;
using DebugGUI;
using FirefliesFuelReplenish;
using FirefliesPicking;
using FirefliesSpawn;
using FirePitSpawn;
using Interacting;
using LampFlame;
using LampFuel;
using PlayerHealth;
using PlayerMovement;
using Spawn;
using UnityEngine;
using UnityEngine.Rendering.Universal;
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
    
    [SerializeField] private SpawnerConfig fireflySpawnerConfig;
    [SerializeField] private SpawnerConfig firePitSpawnerConfig;
    
    [SerializeField] private GameObject playerPrefab;
    
    protected override void Configure(IContainerBuilder builder)
    {
        // Debug
        var levelDebugGUIGameObject = new GameObject("[LevelDebugGUI]");
        var debugGUIPositioner = levelDebugGUIGameObject.AddComponent<DebugGUIController>();
        builder.RegisterComponent(debugGUIPositioner);
        
        // Interacting
        builder.Register<CurrentInteractableHolder>(Lifetime.Singleton);
        
        // Fire Pit Spawn
        builder.RegisterInstance(firePitSpawnerConfig).Keyed(nameof(FirePit));
        builder.RegisterInstance(firePitSpawnerConfig.prefab).Keyed(nameof(FirePit));
        builder.Register<FirePitFactory>(Lifetime.Singleton);
        builder.RegisterEntryPoint<FirePitPool>().AsSelf();
        builder.RegisterEntryPoint<FirePitSpawner>();
        
        
        // Player
        var player = Instantiate(playerPrefab);
        var interactor = player.GetComponent<PlayerInteractor>();
        builder.RegisterComponent(interactor);
        
        // Movement
        builder.RegisterInstance(playerMovementConfig);
        builder.RegisterEntryPoint<PlayerMover>();
        var movementInputHandler = player.GetComponent<PlayerMovementInputHandler>();
        builder.RegisterComponent(movementInputHandler);
        
        // Player position
        builder.RegisterInstance(player.transform).Keyed("Player");
        
        // Camera
        builder.RegisterInstance(Camera.main);
        if (Camera.main != null) Camera.main.transform.SetParent(player.transform);
        
        // Firefly Spawn
        builder.RegisterInstance(fireflySpawnerConfig).Keyed(nameof(Firefly));
        builder.RegisterEntryPoint<FireflySpawner>();
        builder.Register<FireflyPool>(Lifetime.Singleton);
        builder.Register<SpawnTimer>(Lifetime.Singleton);
        builder.Register<IDebugGUIWindow, DebugFireflySpawnerGUI>(Lifetime.Singleton);
        
        // Firefly Picking
        builder.Register<FireflyPicker>(Lifetime.Singleton);
        
        // Lamp Fuel
        builder.RegisterInstance(lampFuelConfig);
        builder.RegisterEntryPoint<LampFuelBurner>();
        builder.Register<LampFuelTank>(Lifetime.Singleton);
        builder.Register<IDebugGUIWindow, DebugLampFuelGUI>(Lifetime.Singleton);
        
        // Firefly Lamp Fuel Replenish
        builder.RegisterEntryPoint<FireflyFuelReplenisher>();
        
        // Lamp Flame
        builder.RegisterInstance(lampFlameConfig);
        builder.Register<LampFlamePower>(Lifetime.Singleton);
        builder.RegisterEntryPoint<LampFlamer>();
        builder.Register<IDebugGUIWindow, DebugLampFlameGUI>(Lifetime.Singleton);
        var lampLight = player.GetComponent<Light2D>(); 
        builder.RegisterComponent(lampLight);
        builder.RegisterEntryPoint<LampFlameViewController>();
        
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