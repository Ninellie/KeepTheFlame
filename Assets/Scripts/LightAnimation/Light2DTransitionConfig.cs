using DG.Tweening;
using UnityEngine;

namespace LightAnimation
{
    [CreateAssetMenu(menuName = "Config/View/Light Transition")]
    public class Light2DTransitionConfig : ScriptableObject
    {
        [SerializeField] private Ease ease = Ease.Linear;
        [SerializeField] private LoopType loopType = LoopType.Yoyo;
        [SerializeField] private float duration = 3f;
        [SerializeField] private float endValue = 5f;
        [SerializeField] private float startValue = 0.1f;
        
        public Ease Ease => ease;
        public LoopType LoopType => loopType;
        public float Duration => duration;
        public float EndValue => endValue;
        public float StartValue => startValue;
    }
}