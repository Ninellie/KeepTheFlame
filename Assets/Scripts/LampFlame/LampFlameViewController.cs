using UnityEngine.Rendering.Universal;
using VContainer.Unity;

namespace LampFlame
{
    public class LampFlameViewController : ITickable
    {
        private readonly Light2D _lampLight;
        private readonly LampFlamePower _flame;

        public LampFlameViewController(Light2D lampLight, LampFlamePower flame)
        {
            _lampLight = lampLight;
            _flame = flame;
        }
        
        public void Tick()
        {
            _lampLight.intensity = _flame.Value;
        }
    }
}