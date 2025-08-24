using UnityEngine;
using VContainer;
using VContainer.Unity;

public class GameLifetimeScope : LifetimeScope
{
    [SerializeField] private LampFuelConfig lampFuelConfig;
    
    protected override void Configure(IContainerBuilder builder)
    {
        builder.RegisterInstance(lampFuelConfig);
        builder.Register<LampFuelService>(Lifetime.Singleton);
        
        
        builder.RegisterEntryPoint<LevelDebugGuiEntryPoint>();
        builder.RegisterEntryPoint<LampUpdater>();
    }
}