using VContainer.Unity;

namespace Lamp
{
    public class LampFuelBurner : ITickable
    {
        private readonly LampFuel _fuel;
        private readonly LampFuelConfig _config;
    
        public LampFuelBurner(LampFuel fuel, LampFuelConfig config)
        {
            _fuel = fuel;
            _config = config;
        }

        public void Tick()
        {
            if (_fuel.Value <= _fuel.Min) return;
            if (_config.decayPerSecond <= 0f) return;
            _fuel.Subtract(_config.decayPerSecond * UnityEngine.Time.deltaTime);
        }
    }
}