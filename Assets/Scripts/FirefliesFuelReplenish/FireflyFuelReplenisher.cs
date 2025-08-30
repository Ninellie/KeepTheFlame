using System;
using FirefliesSpawn;
using LampFuel;

namespace FirefliesFuelReplenish
{
    public class FireflyFuelReplenisher : IDisposable
    {
        public event Action OnFireflyReplenish; 
        
        private FireflyPool _pool;
        private LampFuelTank _fuelTank;

        public FireflyFuelReplenisher(FireflyPool pool, LampFuelTank fuelTank)
        {
            _pool = pool;
            _fuelTank = fuelTank;
            
            _pool.OnFireflyCollected += Replenish; 
        }

        public void Replenish(Firefly firefly)
        {
            _fuelTank.Add(1);
            OnFireflyReplenish?.Invoke();
        }
        
        public void Dispose()
        {
            _pool.OnFireflyCollected -= Replenish;
        }
    }
}