using System;
using FirefliesPicking;
using FirefliesSpawn;
using LampFuel;
using VContainer.Unity;

namespace FirefliesFuelReplenish
{
    public class FireflyFuelReplenisher : IDisposable, IStartable
    {
        private FireflyPicker _picker;
        private LampFuelTank _fuelTank;

        public FireflyFuelReplenisher(FireflyPicker picker, LampFuelTank fuelTank)
        {
            _picker = picker;
            _fuelTank = fuelTank;
        }
        
        public void Replenish(Firefly firefly)
        {
            _fuelTank.Add(firefly.FuelAmount);
        }
        
        public void Start()
        {
            _picker.OnFireflyPickup += Replenish; 
        }
        
        public void Dispose()
        {
            _picker.OnFireflyPickup -= Replenish;
        }
    }
}