using ChunkSpawner;
using Darkness;
using Darkness.Damage;
using DebugGUI;
using Entity.Movement;
using Input;
using LampFlame;
using LampFuel;
using LightAnimation;
using Losing;
using Player.Health;
using Player.Interacting;
using Player.Movement;
using Pause;
using Player.SpriteAnimation;
using TileFloorGeneration;
using UI;
using UnityEngine;
using UnityEngine.Tilemaps;
using VContainer;
using VContainer.Unity;
using Winning;

public class GameLifetimeScope : LifetimeScope
{
    [Header("Config")]
    [SerializeField] private LampFuelConfig lampFuelConfig;
    [SerializeField] private LampFlameConfig lampFlameConfig;
    [SerializeField] private DarknessConfig darknessConfig;
    [SerializeField] private PlayerHealthConfig healthConfig;
    [SerializeField] private PlayerMovementConfig playerMovementConfig;
    [SerializeField] private DarknessDamageConfig darknessDamageConfig;
    [SerializeField] private MovementConfig fireflyMovementConfig;
    
    [SerializeField] private ChunkSpawnerConfig spawnerConfig;
    
    [Header("Prefab")]
    [SerializeField] private GameObject playerPrefab;
    
    [Header("Tile")]
    [SerializeField] private Tile floorTile;
    
    protected override void Configure(IContainerBuilder builder)
    {
        // Chunk spawner
        builder.RegisterInstance(spawnerConfig);
        builder.RegisterEntryPoint<ChunksDestroyCooldownsCounter>().AsSelf();
        builder.RegisterEntryPoint<ChunkBoundaryWatcher>().AsSelf();
        builder.RegisterEntryPoint<Spawner>();
        
        // Floor generation
        var tilemap = CreateTilemap();
        builder.RegisterInstance(tilemap);
        builder.RegisterInstance(floorTile);
        builder.RegisterEntryPoint<FloorGenerator>();
        
        // Debug
        var levelDebugGUIGameObject = new GameObject("[LevelDebugGUI]");
        var debugGUIPositioner = levelDebugGUIGameObject.AddComponent<DebugGUIController>();
        builder.RegisterComponent(debugGUIPositioner);
        
        // Player
        var player = Instantiate(playerPrefab);
        var interactor = player.GetComponent<PlayerInteractor>();
        builder.RegisterComponent(interactor);
        
        // Movement
        builder.RegisterInstance(playerMovementConfig);
        builder.RegisterEntryPoint<PlayerMover>();
        
        // Player Sprite Flipping
        var playerSpriteRenderer = player.GetComponent<SpriteRenderer>();
        builder.RegisterInstance(playerSpriteRenderer).Keyed("Player");
        
        // Player Sprite Animation
        var playerAnimator = player.GetComponent<Animator>();
        builder.RegisterInstance(playerAnimator).Keyed("Player");
        builder.RegisterEntryPoint<SpriteAnimator>();
        
        // Player position
        builder.RegisterInstance(player.transform).Keyed("Player");
        
        // Camera
        builder.RegisterInstance(Camera.main);
        if (Camera.main != null) Camera.main.transform.SetParent(player.transform);
        
        // Lamp Fuel
        builder.RegisterInstance(lampFuelConfig);
        builder.RegisterEntryPoint<LampFuelBurner>();   
        builder.Register<LampFuelTank>(Lifetime.Singleton);
        builder.Register<IDebugGUIWindow, DebugLampFuelGUI>(Lifetime.Singleton);
        
        // Lamp Flame
        builder.RegisterInstance(lampFlameConfig);
        builder.Register<LampFlamePower>(Lifetime.Singleton);
        builder.RegisterEntryPoint<LampFlamer>();
        builder.Register<IDebugGUIWindow, DebugLampFlameGUI>(Lifetime.Singleton);
        var lampLight = player.GetComponentInChildren<LightTranslator>(); 
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
        
        // UI
        builder.RegisterEntryPoint<DamageScreenPulsar>();
        builder.RegisterComponentInHierarchy<GameParametersIndicator>();
        builder.RegisterComponentInHierarchy<GameTimer>();
        
        // Pause
        builder.RegisterEntryPoint<PauseManager>().AsSelf();
        builder.RegisterComponentInHierarchy<PauseMenu>();
        
        // Lose
        builder.RegisterEntryPoint<LoseController>().AsSelf();
        builder.RegisterComponentInHierarchy<LoseScreen>();
        
        // Win
        builder.RegisterEntryPoint<WinController>().AsSelf();
        builder.RegisterComponentInHierarchy<WinScreen>();

        // Darkness Damage
        builder.RegisterInstance(darknessDamageConfig);
        builder.RegisterEntryPoint<DarknessDamageDealer>();
        builder.Register<DebugDarknessDamageGUI>(Lifetime.Singleton).AsImplementedInterfaces()
            .AsSelf();
        
        // Input
        builder.RegisterEntryPoint<InputManager>().AsSelf();
    }
    
    private Tilemap CreateTilemap()
    {
        var gridObject = new GameObject("FloorGrid");
        var grid = gridObject.AddComponent<Grid>();
        grid.cellSize = floorTile.sprite.bounds.size;
            
        var tilemapObject = new GameObject("FloorTilemap");
        tilemapObject.transform.SetParent(gridObject.transform);
        var tilemap = tilemapObject.AddComponent<Tilemap>();
        var tilemapRenderer = tilemapObject.AddComponent<TilemapRenderer>();
        tilemapRenderer.sortingLayerName = "Floor";
        
        // Устанавливаем Lit материал для поддержки освещения Light2D в билде
        var litMaterial = GetLitMaterial();
        if (litMaterial != null)
        {
            tilemapRenderer.material = litMaterial;
        }
        
        return tilemap;
    }
    
    private Material GetLitMaterial()
    {
        // Создаем материал программно с правильным шейдером для поддержки освещения Light2D
        var shader = Shader.Find("Universal Render Pipeline/2D/Sprite-Lit-Default");
        if (shader == null) return null;
        var material = new Material(shader);
        material.name = "FloorLitMaterial";
        return material;

    }
}