using Spawning;
using UnityEngine;
using VContainer;

namespace FirePitSpawn
{
    public class FirePitSpawner : Spawner
    {
        public FirePitSpawner(
            [Key(nameof(FirePit))] SpawnerConfig config,
            FirePitFactory factory,
            [Key("Player")] Transform playerTransform,
            Camera mainCamera)
            : base(config, factory, playerTransform, mainCamera)
        {
        }
    }
}