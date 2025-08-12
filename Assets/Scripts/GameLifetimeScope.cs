using UnityEngine;
using VContainer;
using VContainer.Unity;

public class GameLifetimeScope : LifetimeScope
{
    [SerializeField] private LampConfig lampConfig;
    
    protected override void Configure(IContainerBuilder builder)
    {
        builder.RegisterInstance(lampConfig);
        builder.Register<ILamp, LampService>(Lifetime.Singleton);
        
        
        builder.RegisterEntryPoint<LevelDebugGuiEntryPoint>();
        builder.RegisterEntryPoint<LampUpdater>();
    }
}