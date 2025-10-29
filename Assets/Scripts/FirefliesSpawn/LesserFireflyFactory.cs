using FirefliesPicking;
using Spawning;
using UnityEngine;
using VContainer;

namespace FirefliesSpawn
{
    public class LesserFireflyFactory : IEntityFactory
    {
        private readonly GameObject _prefab;
        private readonly FireflyPicker _picker;

        public LesserFireflyFactory([Key(nameof(LesserFirefly))] GameObject prefab, FireflyPicker picker)
        {
            _prefab = prefab;
            _picker = picker;
        }

        public IPooledEntity CreateInstance()
        {
            var go = Object.Instantiate(_prefab);
            var firefly = go.GetComponent<Firefly>();
            firefly.InjectDependencies(_picker,  0.1f);
            firefly.gameObject.SetActive(false);
            return firefly;
        }
    }
}