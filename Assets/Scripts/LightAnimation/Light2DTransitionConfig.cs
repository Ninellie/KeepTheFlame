using DG.Tweening;
using UnityEngine;

namespace LightAnimation
{
    [CreateAssetMenu(menuName = "Config/View/Light Transition")]
    public class Light2DTransitionConfig : ScriptableObject
    {
        [SerializeField] public Ease ease = Ease.Linear;
        [SerializeField] public LoopType loopType = LoopType.Yoyo;
        [SerializeField] public float duration = 3f;
        [SerializeField] public float endValue = 5f;
        [SerializeField] public float startValue = 0.1f;
    }
}