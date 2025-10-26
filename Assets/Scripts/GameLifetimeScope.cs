using Darkness;
using DarknessDamage;
using DebugGUI;
using FirefliesFuelReplenish;
using FirefliesPicking;
using FirefliesSpawn;
using FireflyMovement;
using FirePitSpawn;
using Interacting;
using LampFlame;
using LampFuel;
using LightAnimation;
using PlayerHealth;
using PlayerMovement;
using PlayerSpriteFlipping;
using PlayerSpriteAnimation;
using Spawning;
using TileFloorGeneration;
using UnityEngine;
using UnityEngine.Tilemaps;
using VContainer;
using VContainer.Unity;

public class GameLifetimeScope : LifetimeScope
{
    [Header("Config")]
    [SerializeField] private LampFuelConfig lampFuelConfig;
    [SerializeField] private LampFlameConfig lampFlameConfig;
    [SerializeField] private DarknessConfig darknessConfig;
    [SerializeField] private PlayerHealthConfig healthConfig;
    [SerializeField] private PlayerMovementConfig playerMovementConfig;
    [SerializeField] private DarknessDamageConfig darknessDamageConfig;
    [SerializeField] private FireflyMovementConfig fireflyMovementConfig;
    
    [SerializeField] private SpawnerConfig fireflySpawnerConfig;
    [SerializeField] private SpawnerConfig firePitSpawnerConfig;
    [SerializeField] private SpawnerConfig lesserFireflySpawnerConfig;
    
    [Header("Prefab")]
    [SerializeField] private GameObject playerPrefab;
    
    [Header("Tile")]
    [SerializeField] private Tile floorTile;
    
    protected override void Configure(IContainerBuilder builder)
    {
        // Debug
        var levelDebugGUIGameObject = new GameObject("[LevelDebugGUI]");
        var debugGUIPositioner = levelDebugGUIGameObject.AddComponent<DebugGUIController>();
        builder.RegisterComponent(debugGUIPositioner);
        
        // Interacting
        builder.Register<CurrentInteractableHolder>(Lifetime.Singleton);
        
        // Player
        var player = Instantiate(playerPrefab);
        var interactor = player.GetComponent<PlayerInteractor>();
        builder.RegisterComponent(interactor);
        
        // Movement
        builder.RegisterInstance(playerMovementConfig);
        builder.RegisterEntryPoint<PlayerMover>();
        var movementInputHandler = player.GetComponent<PlayerMovementInputHandler>();
        builder.RegisterComponent(movementInputHandler);
        
        // Player Sprite Flipping
        var playerSpriteRenderer = player.GetComponent<SpriteRenderer>();
        builder.RegisterInstance(playerSpriteRenderer).Keyed("Player");
        builder.RegisterEntryPoint<PlayerSpriteFlipper>();
        
        // Player Sprite Animation
        var playerAnimator = player.GetComponent<Animator>();
        builder.RegisterInstance(playerAnimator).Keyed("Player");
        builder.RegisterEntryPoint<PlayerSpriteAnimator>();
        
        // Player position
        builder.RegisterInstance(player.transform).Keyed("Player");
        
        // Camera
        builder.RegisterInstance(Camera.main);
        if (Camera.main != null) Camera.main.transform.SetParent(player.transform);
        
        // Fire Pit Spawn
        RegisterFirePitSpawner(builder, firePitSpawnerConfig);
        
        // Lesser firefly Spawn
        RegisterLesserFireflySpawner(builder, lesserFireflySpawnerConfig);
        
        // Firefly Spawn
        RegisterFireflySpawner(builder, fireflySpawnerConfig);

        // Firefly Picking
        builder.Register<FireflyPicker>(Lifetime.Singleton);
        
        // Firefly Movement
        builder.RegisterInstance(fireflyMovementConfig);
        builder.RegisterEntryPoint<FireflyMover>().AsSelf();
        
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
        var lampLight = player.GetComponentInChildren<LightFlicker>(); 
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
        
        // Floor generation
        builder.RegisterInstance(floorTile);
        builder.RegisterEntryPoint<FloorGenerator>();
    }

    private void RegisterLesserFireflySpawner(IContainerBuilder builder, SpawnerConfig config)
    {
        builder.RegisterInstance(config).Keyed(nameof(LesserFirefly));
        builder.RegisterInstance(config.prefab).Keyed(nameof(LesserFirefly));
        builder.Register<LesserFireflyFactory>(Lifetime.Singleton);
        builder.RegisterEntryPoint<LesserFireflySpawner>().AsSelf();
    }

    private void RegisterFireflySpawner(IContainerBuilder builder, SpawnerConfig config)
    {
        builder.RegisterInstance(config).Keyed(nameof(Firefly));
        builder.RegisterInstance(config.prefab).Keyed(nameof(Firefly));
        builder.Register<FireflyFactory>(Lifetime.Singleton);
        builder.RegisterEntryPoint<FireflySpawner>().AsSelf();
        
        builder.Register<IDebugGUIWindow, DebugFireflySpawnerGUI>(Lifetime.Singleton);
    }

    private void RegisterFirePitSpawner(IContainerBuilder builder, SpawnerConfig config)
    {
        builder.RegisterInstance(config).Keyed(nameof(FirePit));
        builder.RegisterInstance(config.prefab).Keyed(nameof(FirePit));
        builder.Register<FirePitFactory>(Lifetime.Singleton);
        builder.RegisterEntryPoint<FirePitSpawner>().AsSelf();
    }
}