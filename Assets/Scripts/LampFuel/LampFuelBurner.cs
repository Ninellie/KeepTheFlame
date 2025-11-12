using LampFlame;
using VContainer.Unity;

namespace LampFuel
{
    public class LampFuelBurner : IFixedTickable
    {
        public float DecayPerSecond { get; private set; }
        
        private readonly LampFuelTank _fuelTank;
        private readonly LampFuelConfig _config;
        private readonly LampFlamePower _flame;
        
        public LampFuelBurner(LampFuelTank fuelTank, LampFuelConfig config, LampFlamePower flame)
        {
            _fuelTank = fuelTank;
            _config = config;
            _flame = flame;

            Init();
        }

        public void FixedTick()
        {
            if (!_flame.IsLit) return;
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