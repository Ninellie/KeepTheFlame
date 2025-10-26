using FirefliesPicking;
using FireflyMovement;
using Spawning;
using UnityEngine;
using VContainer;

namespace FirefliesSpawn
{
    public class LesserFirefly
    {
        
    }
    
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
    
    public class LesserFireflyFactory : IEntityFactory
    {
        private readonly GameObject _prefab;
        private readonly FireflyPicker _picker;
        private readonly FireflyMover _fireflyMover;

        public LesserFireflyFactory([Key(nameof(LesserFirefly))] GameObject prefab, FireflyPicker picker, FireflyMover fireflyMover)
        {
            _prefab = prefab;
            _picker = picker;
            _fireflyMover = fireflyMover;
        }

        public IPooledEntity CreateInstance()
        {
            var go = Object.Instantiate(_prefab);
            var firefly = go.GetComponent<Firefly>();
            firefly.InjectDependencies(_picker,  _fireflyMover, 0.1f);
            firefly.gameObject.SetActive(false);
            return firefly;
        }
    }
}