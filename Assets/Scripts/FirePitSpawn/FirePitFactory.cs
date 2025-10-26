using Darkness;
using Interacting;
using LampFuel;
using Spawning;
using UnityEngine;
using VContainer;

namespace FirePitSpawn
{
    public class FirePitFactory : IEntityFactory
    {
        private readonly GameObject _prefab;
        private readonly DarknessPower _darknessPower;
        private readonly LampFuelTank _fuelTank;
        private readonly CurrentInteractableHolder _currentInteractableHolder;

        public FirePitFactory(
            [Key(nameof(FirePit))] GameObject prefab,
            DarknessPower darknessPower, 
            LampFuelTank fuelTank, 
            CurrentInteractableHolder currentInteractableHolder)
        {
            _prefab = prefab;
            _currentInteractableHolder = currentInteractableHolder;
            _darknessPower = darknessPower;
            _fuelTank = fuelTank;
        }
        
        public IPooledEntity CreateInstance()
        {
            var go = Object.Instantiate(_prefab);
            var firePit = go.GetComponent<FirePit>();
            firePit.InjectDependencies(_darknessPower, _fuelTank);
            firePit.gameObject.SetActive(false);
            go.GetComponent<Interactable>().InjectDependencies(_currentInteractableHolder);
            return firePit;
        }
    }
}