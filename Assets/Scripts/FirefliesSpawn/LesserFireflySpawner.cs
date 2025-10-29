using Spawning;
using UnityEngine;
using VContainer;

namespace FirefliesSpawn
{
    public class LesserFireflySpawner : Spawner
    {
        public LesserFireflySpawner(
            [Key(nameof(LesserFirefly))] SpawnerConfig config,
            LesserFireflyFactory factory,
            [Key("Player")] Transform playerTransform,
            Camera mainCamera)
            : base(config, factory, playerTransform, mainCamera)
        {
        }
    }
}