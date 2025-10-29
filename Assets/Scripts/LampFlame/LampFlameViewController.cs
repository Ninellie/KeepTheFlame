using LightAnimation;
using VContainer.Unity;

namespace LampFlame
{
    public class LampFlameViewController : ITickable
    {
        private readonly LightTranslator _lampTranslator;
        private readonly LampFlamePower _flame;

        public LampFlameViewController(LightTranslator lampTranslator, LampFlamePower flame)
        {
            _lampTranslator = lampTranslator;
            _flame = flame;
        }
        
        public void Tick()
        {
            _lampTranslator.FlameValue = _flame.Value;
        }
    }
}