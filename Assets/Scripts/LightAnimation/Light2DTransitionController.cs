using DG.Tweening;
using DG.Tweening.Core;
using UnityEngine;
using UnityEngine.Rendering.Universal;

namespace LightAnimation
{
    public class Light2DTransitionController : MonoBehaviour
    {
        [SerializeField] private Light2DTransitionConfig config;
    
        private Light2D _light;
        private Tween _tween;
    
        private void Awake()
        {
            _light = GetComponent<Light2D>();
        }

        private void OnEnable()
        {
            _light.intensity = config.StartValue;
            
            _tween?.Kill();
            _tween = DOTween.To(Get(), Set, config.EndValue, config.Duration)
                .SetEase(config.Ease)
                .SetLoops(-1, config.LoopType);
        }

        private void OnDisable()
        {
            _tween?.Kill();
            _tween = null;
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