using DG.Tweening;
using DG.Tweening.Core;
using UnityEngine;
using UnityEngine.Rendering.Universal;

namespace LightAnimation
{
    public class Light2DTransitionController : MonoBehaviour
    {
        [SerializeField] private Light2DTransitionConfig _config;
    
        private Light2D _light;
    
        private void Awake()
        {
            _light = GetComponent<Light2D>();
        }

        private void OnEnable()
        {
            _light.intensity = _config.startValue;
            
            DOTween.To(Get(), Set, _config.endValue, _config.duration)
                .SetEase(_config.ease)
                .SetLoops(-1, _config.loopType);
        }

        private void Set(float value)
        {
            _light.intensity = value;
        }

        private DOGetter<float> Get()
        {
            return () => _light.intensity;
        }
    }
}