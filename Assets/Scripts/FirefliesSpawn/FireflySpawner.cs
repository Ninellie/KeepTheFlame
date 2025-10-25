using Spawning;
using UnityEngine;
using VContainer;

namespace FirefliesSpawn
{
    public class FireflySpawner : Spawner
    {
        public FireflySpawner(
            [Key(nameof(Firefly))] SpawnerConfig config,
            [Key(nameof(Firefly))] IEntityPool pool, 
            [Key("Player")] Transform playerTransform,
            Camera mainCamera)
            : base(config, pool, playerTransform, mainCamera)
        {
        }
    }
}