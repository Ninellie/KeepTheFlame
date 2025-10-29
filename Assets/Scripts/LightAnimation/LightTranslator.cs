using TriInspector;
using UnityEngine;
using UnityEngine.Rendering.Universal;

namespace LightAnimation
{
    public class LightTranslator : MonoBehaviour
    {
        [ReadOnly] [SerializeField] private float flameValue = 5f;
        [ReadOnly] [SerializeField] private float targetFlameValue = 5f;
        [Header("Base")]
        [SerializeField] private float intensityScale = 0.5f;
        [SerializeField] private float outerRadiusScale = 5f;
        [SerializeField] private float innerRadiusScale = 1f;
        [SerializeField] private float initIntensity = 5f;
        
        [Header("Smoothness")]
        [SerializeField] private float valueChangePerSecond = 0.35f;
        
        public float FlameValue
        {
            set => targetFlameValue = value;
        }
        
        private Light2D _light;
        
        private void Awake()
        {
            _light = GetComponent<Light2D>();
        }

        private void Start()
        {
            flameValue = initIntensity;
            
            _light.intensity = flameValue * intensityScale;
            _light.pointLightOuterRadius = flameValue * outerRadiusScale;
            _light.pointLightInnerRadius = flameValue * innerRadiusScale;
        }

        private void Update()
        {
            var valueChange = Time.deltaTime * valueChangePerSecond;
            
            // плавное изменение текущего значения к целевому
            flameValue = Mathf.MoveTowards(flameValue, targetFlameValue, valueChange);
            
            _light.intensity = Mathf.MoveTowards(_light.intensity, flameValue * intensityScale, valueChange);
            _light.pointLightOuterRadius = Mathf.MoveTowards(_light.pointLightOuterRadius, flameValue * outerRadiusScale, valueChange);
            _light.pointLightInnerRadius =  Mathf.MoveTowards(_light.pointLightInnerRadius, flameValue * innerRadiusScale, valueChange);
        }
    }
}