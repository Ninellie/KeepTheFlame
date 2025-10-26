using Spawning;
using UnityEngine;
using VContainer;

namespace FirefliesSpawn
{
    public class FireflySpawner : Spawner
    {
        public FireflySpawner(
            [Key(nameof(Firefly))] SpawnerConfig config,
            FireflyFactory factory,
            [Key("Player")] Transform playerTransform,
            Camera mainCamera)
            : base(config, factory, playerTransform, mainCamera)
        {
        }
    }
}