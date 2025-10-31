using Spawning;
using UnityEngine;
using VContainer;

namespace ScaryTreeSpawn
{
    public class ScaryTreeSpawner : Spawner
    {
        public ScaryTreeSpawner(
            [Key(nameof(ScaryTree))] SpawnerConfig config,
            ScaryTreeFactory factory,
            [Key("Player")] Transform playerTransform,
            Camera mainCamera)
            : base(config, factory, playerTransform, mainCamera)
        {
        }
    }
}