using PlayerHealth;
using Spawning;
using UnityEngine;
using VContainer;

namespace ScaryTreeSpawn
{
    public class ScaryTreeFactory : IEntityFactory
    {
        private readonly GameObject _prefab;
        private readonly PlayerHealthCounter _playerHealthCounter;

        public ScaryTreeFactory([Key(nameof(ScaryTree))] GameObject prefab, PlayerHealthCounter playerHealthCounter)
        {
            _prefab = prefab;
            _playerHealthCounter = playerHealthCounter;
        }

        public IPooledEntity CreateInstance()
        {
            var go = Object.Instantiate(_prefab);
            var scaryTree = go.GetComponent<ScaryTree>();
            scaryTree.Damage = 1;
            scaryTree.PlayerHealthCounter = _playerHealthCounter;
            scaryTree.gameObject.SetActive(false);
            return scaryTree;
        }
    }
}