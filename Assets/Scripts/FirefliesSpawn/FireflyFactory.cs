using FirefliesPicking;
using Spawning;
using UnityEngine;
using VContainer;

namespace FirefliesSpawn
{
    public class FireflyFactory : IEntityFactory
    {
        private readonly GameObject _prefab;
        private readonly FireflyPicker _picker;

        public FireflyFactory([Key(nameof(Firefly))] GameObject prefab, FireflyPicker picker)
        {
            _prefab = prefab;
            _picker = picker;
        }

        public IPooledEntity CreateInstance()
        {
            var go = Object.Instantiate(_prefab);
            var firefly = go.GetComponent<Firefly>();
            firefly.InjectDependencies(_picker, 1);
            firefly.gameObject.SetActive(false);
            return firefly;
        }
    }
}