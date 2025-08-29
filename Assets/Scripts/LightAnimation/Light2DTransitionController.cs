using DG.Tweening;
using DG.Tweening.Core;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class Light2DTransitionController : MonoBehaviour
{
    [SerializeField] private Ease ease = Ease.Linear;
    [SerializeField] private LoopType loopType = LoopType.Yoyo;
    [SerializeField] private  float duration = 3f;
    [SerializeField] private  float endValue = 5f;
    
    private Light2D _light;
    
    private void Awake()
    {
        _light = GetComponent<Light2D>();
    }

    private void OnEnable()
    {
        DOTween.To(Getter(), Setter, endValue, duration).SetEase(ease).SetLoops(-1, loopType);
    }

    private void Setter(float value)
    {
        _light.intensity = value;
    }

    private DOGetter<float> Getter()
    {
        return () => _light.intensity;
    }
}
