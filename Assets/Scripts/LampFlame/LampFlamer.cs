using Darkness;
using LampFuel;
using UnityEngine;
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
            var maxFlamePower = _fuelTank.Max - darknessPower;
            
            _flame.SetValue(Mathf.Min(fuel, maxFlamePower));
        }
    }
}