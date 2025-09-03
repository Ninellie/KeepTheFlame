using LightAnimation;
using VContainer.Unity;

namespace LampFlame
{
    public class LampFlameViewController : ITickable
    {
        private readonly LightFlicker _lampFlicker;
        private readonly LampFlamePower _flame;

        public LampFlameViewController(LightFlicker lampFlicker, LampFlamePower flame)
        {
            _lampFlicker = lampFlicker;
            _flame = flame;
        }
        
        public void Tick()
        {
            _lampFlicker.FlameValue = _flame.Value;
        }
    }
}