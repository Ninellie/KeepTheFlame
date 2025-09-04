using TriInspector;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using Random = UnityEngine.Random;

namespace LightAnimation
{
    public class LightFlicker : MonoBehaviour
    {
        [ReadOnly] [SerializeField] private float _flameValue = 5f;
        [ReadOnly] [SerializeField] private float _targetFlameValue = 5f;
        [Header("Base")]
        [SerializeField] private float intensityScale = 0.5f;
        [SerializeField] private float outerRadiusScale = 5f;
        [SerializeField] private float innerRadiusScale = 1f;
        [SerializeField] private float initIntensity = 5f;

        [Header("Flicker")]
        [SerializeField] private float intensityAmplitude = 0.25f; // разброс яркости
        [SerializeField] private float radiusAmplitude = 0.5f;     // разброс радиуса
        [SerializeField] private float speed = 1.5f;               // скорость шума

        [Header("Hue")]
        [SerializeField] private Color baseColor = new(1f, 0.86f, 0.6f);
        [SerializeField] private float hueJitter = 0.05f;          // очень лёгкий сдвиг оттенка

        [Header("Smoothness")]
        [SerializeField] private float valueChangePerSecond = 0.35f;// скорость изменения FlameValue в секунду
        
        public float FlameValue
        {
            set => _targetFlameValue = value;
        }
        
        private Light2D _light;
        private float _seed;

        private float _intensity;
        private float _outerRadius;
        private float _innerRadius;
        
        private void Awake()
        {
            _light = GetComponent<Light2D>();
            _seed = Random.value * 1000f;
        }

        private void Start()
        {
            Init(initIntensity);
        }

        public void Init(float flameValue)
        {
            _flameValue = flameValue;
            
            _intensity = _flameValue * intensityScale;
            _outerRadius = _flameValue * outerRadiusScale;
            _innerRadius = _flameValue * innerRadiusScale;
            
            _light.intensity = _intensity;
            _light.pointLightOuterRadius = _outerRadius;
            _light.pointLightInnerRadius = _innerRadius;
        }

        private void Update()
        {
            // плавное изменение текущего значения к целевому
            _flameValue = Mathf.MoveTowards(_flameValue, _targetFlameValue, Time.deltaTime * valueChangePerSecond);

            UpdateLightProperties(Time.deltaTime * valueChangePerSecond);
            UpdateFlicker();
        }

        private void UpdateLightProperties(float t)
        {
            // _flameValue
            _intensity = Mathf.MoveTowards(_intensity, _flameValue * intensityScale, t);
            _outerRadius = Mathf.MoveTowards(_outerRadius, _flameValue * outerRadiusScale, t);
            _innerRadius =  Mathf.MoveTowards(_innerRadius, _flameValue * innerRadiusScale, t);
            
            _light.intensity = _intensity;
            _light.pointLightOuterRadius = _outerRadius;
            _light.pointLightInnerRadius = _innerRadius;
        }

        private void UpdateFlicker()
        {
            var t = Time.time * speed + _seed;

            // плавный шум
            var n1 = Mathf.PerlinNoise(t, 0f) * 2f - 1f; // -1..1
            var n2 = Mathf.PerlinNoise(0f, t) * 2f - 1f;
            
            // мерцание (независимо от _flameValue)
            var flickerIntensity = intensityAmplitude * n1;
            var flickerOuterRadius = radiusAmplitude * n2;
            var flickerInnerRadius = radiusAmplitude * n2 * 0.5f;
            
            // применяем мерцание к текущим значениям
            _light.intensity += flickerIntensity;
            _light.pointLightOuterRadius += flickerOuterRadius;
            _light.pointLightInnerRadius += flickerInnerRadius;

            // лёгкое «теплохолодное» мерцание
            var hueShift = hueJitter * n1;
            Color.RGBToHSV(baseColor, out var h, out var s, out var v);
            h = Mathf.Repeat(h + hueShift, 1f);
            _light.color = Color.HSVToRGB(h, s, v);
        }
    }
}