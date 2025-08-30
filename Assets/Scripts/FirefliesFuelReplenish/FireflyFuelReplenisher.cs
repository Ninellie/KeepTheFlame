using System;
using FirefliesSpawn;
using LampFuel;
using VContainer.Unity;

namespace FirefliesFuelReplenish
{
    public class FireflyFuelReplenisher : IDisposable, IStartable
    {
        public event Action OnFireflyReplenish; 
        
        private FireflyPool _pool;
        private LampFuelTank _fuelTank;

        public FireflyFuelReplenisher(FireflyPool pool, LampFuelTank fuelTank)
        {
            _pool = pool;
            _fuelTank = fuelTank;
        }
        
        public void Start()
        {
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