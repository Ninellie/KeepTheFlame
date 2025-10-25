using Spawning;
using UnityEngine;
using VContainer;

namespace FirePitSpawn
{
    public class FirePitSpawner : Spawner
    {
        public FirePitSpawner(
            [Key(nameof(FirePit))] SpawnerConfig config,
            [Key(nameof(FirePit))] IEntityPool pool, 
            [Key("Player")] Transform playerTransform,
            Camera mainCamera)
            : base(config, pool, playerTransform, mainCamera)
        {
        }
    }
}