using UnityEngine;
using UnityEngine.Rendering.Universal;

namespace LightAnimation
{
    public class LightFlicker : MonoBehaviour
    {
        [Header("База")]
        [SerializeField] private float baseIntensity = 1.2f;
        [SerializeField] private float baseOuterRadius = 6f;

        [Header("Фликер")]
        [SerializeField] private float intensityAmplitude = 0.25f; // разброс яркости
        [SerializeField] private float radiusAmplitude = 0.5f;     // разброс радиуса
        [SerializeField] private float speed = 1.5f;               // скорость шума

        [Header("Цвет")]
        [SerializeField] private Color baseColor = new(1f, 0.86f, 0.6f);
        [SerializeField] private float hueJitter = 0.01f;          // очень лёгкий сдвиг оттенка

        public float FlameValue { get; set; }
        
        private Light2D _light;
        private float _seed;

        private void Awake()
        {
            _light = GetComponent<Light2D>();
            _seed = Random.value * 1000f;
        }

        private void Update()
        {
            var t = Time.time * speed + _seed;

            // плавный шум
            var n1 = Mathf.PerlinNoise(t, 0f) * 2f - 1f; // -1..1
            var n2 = Mathf.PerlinNoise(0f, t) * 2f - 1f;
            
            // интенсивность и радиус
            var intensity = baseIntensity * FlameValue + intensityAmplitude * n1;
            var radius = baseOuterRadius * Mathf.Lerp(0.5f, 1f, FlameValue) + radiusAmplitude * n2;

            _light.intensity = Mathf.Max(0f, intensity);
            _light.pointLightOuterRadius = Mathf.Max(0.1f, radius);

            // лёгкое «теплохолодное» мерцание
            var hueShift = hueJitter * n1;
            Color.RGBToHSV(baseColor, out var h, out var s, out var v);
            h = Mathf.Repeat(h + hueShift, 1f);
            _light.color = Color.HSVToRGB(h, s, v);
        }
    }
}