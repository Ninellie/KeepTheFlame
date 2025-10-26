using FirefliesPicking;
using FireflyMovement;
using Spawning;
using UnityEngine;
using VContainer;

namespace FirefliesSpawn
{
    public class FireflyFactory : IEntityFactory
    {
        private readonly GameObject _prefab;
        private readonly FireflyPicker _picker;
        private readonly FireflyMover _fireflyMover;

        public FireflyFactory([Key(nameof(Firefly))] GameObject prefab, FireflyPicker picker, FireflyMover fireflyMover)
        {
            _prefab = prefab;
            _picker = picker;
            _fireflyMover = fireflyMover;
        }

        public IPooledEntity CreateInstance()
        {
            var go = Object.Instantiate(_prefab);
            var firefly = go.GetComponent<Firefly>();
            firefly.InjectDependencies(_picker,  _fireflyMover, 1);
            firefly.gameObject.SetActive(false);
            return firefly;
        }
    }
}