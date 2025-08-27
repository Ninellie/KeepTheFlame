using Darkness;
using LampFuel;
using VContainer.Unity;

namespace LampFlame
{
    public class LampFlamer : IFixedTickable
    {
        private readonly LampFlamePower _flame;
        private readonly DarknessPower _darkness;
        private readonly LampFuelTank _fuelTank;

        public LampFlamer(DarknessPower darkness, LampFuelTank fuelTank, LampFlamePower flame)
        {
            _darkness = darkness;
            _fuelTank = fuelTank;
            _flame = flame;
        }

        public void FixedTick()
        {
            var darknessPower = _darkness.Value;
            var fuel = _fuelTank.Value;
            
            _flame.SetValue(fuel - darknessPower);
        }
    }
}