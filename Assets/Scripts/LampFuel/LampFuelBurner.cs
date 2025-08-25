using VContainer.Unity;

namespace LampFuel
{
    public class LampFuelBurner : IFixedTickable
    {
        public float DecayPerSecond { get; private set; }
        
        private readonly LampFuelTank _fuelTank;
        private readonly LampFuelConfig _config;
        
        public LampFuelBurner(LampFuelTank fuelTank, LampFuelConfig config)
        {
            _fuelTank = fuelTank;
            _config = config;
            
            Init();
        }

        public void FixedTick()
        {
            if (_fuelTank.Value <= _fuelTank.Min) return;
            if (_config.decayPerSecond <= 0f) return;
            _fuelTank.Subtract(_config.decayPerSecond * UnityEngine.Time.deltaTime);
        }

        public void Init()
        {
            DecayPerSecond = _config.decayPerSecond;
        }
    }
}