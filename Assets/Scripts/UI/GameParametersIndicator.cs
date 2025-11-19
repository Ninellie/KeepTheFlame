using Darkness;
using LampFlame;
using LampFuel;
using Player.Health;
using UnityEngine;
using VContainer;

namespace UI
{
    public class GameParametersIndicator : MonoBehaviour
    {
        [Header("Visual Settings")]
        [Range(10f, 1000f)]
        [SerializeField] private float maxRadius = 50f;
        [Range(0f, 1000f)]
        [SerializeField] private float minRadius = 10f;
        [SerializeField] private Color fuelColor = new(1f, 0.8f, 0f, 0.8f);
        [SerializeField] private Color darknessColor = new(0.2f, 0.1f, 0.3f, 0.6f);
        [SerializeField] private Color healthColor = new(1f, 0.2f, 0.2f, 0.8f);
        [SerializeField] private Color boundaryColor = new(1f, 1f, 1f, 0.5f);
        
        [Header("Position")]
        [SerializeField] private UIPosition uiPosition = new UIPosition
        {
            AnchorMin = new Vector2(0f, 0f),
            AnchorMax = new Vector2(0f, 0f),
            Pivot = new Vector2(0.5f, 0.5f),
            Offset = new Vector2(100f, 100f)
        };
        
        [Range(3, 64)]
        [SerializeField] private int segments = 64;
        [Range(-50f, 200f)]
        [SerializeField] private float healthIndicatorsOffset = 20f;
        [Range(0.1f, 100f)]
        [SerializeField] private float healthIndicatorRadius = 5f;
        
        [Header("Screen Scaling")]
        [SerializeField] private Vector2 referenceResolution = new(1920f, 1080f);
        [SerializeField] private bool useScreenScaling = true;
        [Range(0.1f, 10f)]
        [SerializeField] private float scaleMultiplier = 1f;
        
        [Header("Visibility")]
        [SerializeField] private bool showFuelIndicator = true;
        [SerializeField] private bool showDarknessIndicator = true;
        [SerializeField] private bool showHealthIndicators = true;
        [SerializeField] private bool showBoundary = true;
        [SerializeField] private bool useFillMode = false;
        
        [Inject] private LampFuelTank _fuelTank;
        [Inject] private DarknessPower _darknessPower;
        [Inject] private PlayerHealthCounter _healthCounter;
        [Inject] private LampFlamePower _flamePower;
        
        private Material _glMaterial;
        
        private void Awake()
        {
            CreateGLMaterial();
        }
        
        private void OnValidate()
        {
            if (minRadius >= maxRadius)
            {
                minRadius = Mathf.Max(0.1f, maxRadius - 0.1f);
            }
            
            if (referenceResolution.x < 100f) referenceResolution.x = 100f;
            if (referenceResolution.y < 100f) referenceResolution.y = 100f;
            if (referenceResolution.x > 10000f) referenceResolution.x = 10000f;
            if (referenceResolution.y > 10000f) referenceResolution.y = 10000f;
            
            uiPosition?.OnValidate();
        }
        
        private void OnGUI()
        {
            DrawIndicator();
        }
        
        private void DrawIndicator()
        {
            var scale = GetScreenScale();
            var scaledMaxRadius = maxRadius * scale;
            var scaledMinRadius = minRadius * scale;
            var scaledHealthIndicatorsOffset = healthIndicatorsOffset * scale;
            var scaledHealthIndicatorRadius = healthIndicatorRadius * scale;
            
            var totalRadius = scaledMaxRadius;
            if (showHealthIndicators)
            {
                totalRadius = scaledMaxRadius + scaledHealthIndicatorsOffset + scaledHealthIndicatorRadius;
            }
            
            var screenSize = new Vector2(Screen.width, Screen.height);
            var elementSize = new Vector2(totalRadius * 2f, totalRadius * 2f);
            var scaledPosition = uiPosition.CalculateScreenPosition(elementSize, screenSize);
            
            var fuelNormalized = GetNormalizedValue(_fuelTank.Value, _fuelTank.Min, _fuelTank.Max);
            var darknessNormalized = GetNormalizedValue(_darknessPower.Value, _darknessPower.Min, _darknessPower.Max);
            
            if (useFillMode)
            {
                DrawFillMode(scaledPosition, scaledMaxRadius, scaledMinRadius, fuelNormalized, darknessNormalized);
            }
            else
            {
                DrawWireframeMode(scaledPosition, scaledMaxRadius, scaledMinRadius, fuelNormalized, darknessNormalized);
            }
            
            if (showHealthIndicators)
            {
                DrawHealthIndicators(scaledPosition, scaledMaxRadius + scaledHealthIndicatorsOffset, scaledHealthIndicatorRadius);
            }
        }
        
        private void DrawWireframeMode(Vector2 scaledPosition, float scaledMaxRadius, float scaledMinRadius, float fuelNormalized, float darknessNormalized)
        {
            if (showBoundary)
            {
                DrawCircle(scaledPosition, scaledMaxRadius, boundaryColor);
            }
            
            if (showDarknessIndicator)
            {
                var outerRadius = Mathf.Lerp(scaledMaxRadius, scaledMinRadius, darknessNormalized);
                DrawCircle(scaledPosition, outerRadius, darknessColor);
            }
            
            if (showFuelIndicator)
            {
                var innerRadius = Mathf.Lerp(scaledMinRadius, scaledMaxRadius, fuelNormalized);
                DrawCircle(scaledPosition, innerRadius, fuelColor);
            }
        }
        
        private void DrawFillMode(Vector2 scaledPosition, float scaledMaxRadius, float scaledMinRadius, float fuelNormalized, float darknessNormalized)
        {
            if (showBoundary)
            {
                DrawFilledCircle(scaledPosition, scaledMaxRadius, boundaryColor);
            }
            
            if (showFuelIndicator)
            {
                var innerRadius = Mathf.Lerp(scaledMinRadius, scaledMaxRadius, fuelNormalized);
                DrawFilledCircle(scaledPosition, innerRadius, fuelColor);
            }
            
            if (showDarknessIndicator)
            {
                var innerDarknessRadius = Mathf.Lerp(scaledMaxRadius, scaledMinRadius, darknessNormalized);
                DrawFilledRing(scaledPosition, scaledMaxRadius, innerDarknessRadius, darknessColor);
            }
        }
        
        private void DrawCircle(Vector2 center, float radius, Color color)
        {
            var oldColor = GUI.color;
            GUI.color = color;
            
            var points = new Vector2[segments + 1];
            for (var i = 0; i <= segments; i++)
            {
                var angle = 2f * Mathf.PI * i / segments;
                points[i] = center + new Vector2(
                    Mathf.Cos(angle) * radius,
                    Mathf.Sin(angle) * radius
                );
            }
            
            for (var i = 0; i < segments; i++)
            {
                DrawLine(points[i], points[i + 1]);
            }
            
            GUI.color = oldColor;
        }
        
        private static void DrawLine(Vector2 start, Vector2 end)
        {
            var texture = Texture2D.whiteTexture;
            var matrix = GUI.matrix;
            
            var angle = Mathf.Atan2(end.y - start.y, end.x - start.x) * Mathf.Rad2Deg;
            var length = Vector2.Distance(start, end);
            
            GUIUtility.RotateAroundPivot(angle, start);
            GUI.DrawTexture(new Rect(start.x, start.y - 1f, length, 2f), texture);
            GUI.matrix = matrix;
        }
        
        private void DrawHealthIndicators(Vector2 center, float radius, float indicatorRadius)
        {
            var healthCount = _healthCounter.Max;
            var activeHealth = _healthCounter.Value;
            
            for (var i = 0; i < healthCount; i++)
            {
                var angle = 2f * Mathf.PI * i / healthCount - Mathf.PI / 2f;
                var pos = center + new Vector2(
                    Mathf.Cos(angle) * radius,
                    Mathf.Sin(angle) * radius
                );
                
                var color = i < activeHealth ? healthColor : Color.gray;
                
                if (useFillMode)
                {
                    DrawFilledCircle(pos, indicatorRadius, color);
                }
                else
                {
                    var oldColor = GUI.color;
                    GUI.color = color;
                    DrawCircle(pos, indicatorRadius, color);
                    GUI.color = oldColor;
                }
            }
        }
        
        private void CreateGLMaterial()
        {
            var shader = Shader.Find("Hidden/Internal-Colored");
            if (shader == null)
            {
                Debug.LogError("Shader 'Hidden/Internal-Colored' not found!");
                return;
            }
            
            _glMaterial = new Material(shader);
            _glMaterial.hideFlags = HideFlags.HideAndDontSave;
            _glMaterial.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.SrcAlpha);
            _glMaterial.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.OneMinusSrcAlpha);
            _glMaterial.SetInt("_Cull", (int)UnityEngine.Rendering.CullMode.Off);
            _glMaterial.SetInt("_ZWrite", 0);
        }
        
        private void DrawFilledCircle(Vector2 center, float radius, Color color)
        {
            if (_glMaterial == null)
            {
                CreateGLMaterial();
                if (_glMaterial == null) return;
            }
            
            _glMaterial.SetPass(0);
            
            GL.PushMatrix();
            GL.LoadPixelMatrix(0, Screen.width, Screen.height, 0);
            
            GL.Begin(GL.TRIANGLES);
            GL.Color(color);
            
            var center3D = new Vector3(center.x, center.y, 0f);
            
            for (var i = 0; i < segments; i++)
            {
                var angle1 = 2f * Mathf.PI * i / segments;
                var angle2 = 2f * Mathf.PI * (i + 1) / segments;
                
                var point1 = center3D + new Vector3(
                    Mathf.Cos(angle1) * radius,
                    Mathf.Sin(angle1) * radius,
                    0f
                );
                
                var point2 = center3D + new Vector3(
                    Mathf.Cos(angle2) * radius,
                    Mathf.Sin(angle2) * radius,
                    0f
                );
                
                GL.Vertex(center3D);
                GL.Vertex(point1);
                GL.Vertex(point2);
            }
            
            GL.End();
            GL.PopMatrix();
        }
        
        private void DrawFilledRing(Vector2 center, float outerRadius, float innerRadius, Color color)
        {
            if (_glMaterial == null)
            {
                CreateGLMaterial();
                if (_glMaterial == null) return;
            }
            
            _glMaterial.SetPass(0);
            
            GL.PushMatrix();
            GL.LoadPixelMatrix(0, Screen.width, Screen.height, 0);
            
            GL.Begin(GL.QUADS);
            GL.Color(color);
            
            var center3D = new Vector3(center.x, center.y, 0f);
            
            for (var i = 0; i < segments; i++)
            {
                var angle1 = 2f * Mathf.PI * i / segments;
                var angle2 = 2f * Mathf.PI * (i + 1) / segments;
                
                var outerPoint1 = center3D + new Vector3(
                    Mathf.Cos(angle1) * outerRadius,
                    Mathf.Sin(angle1) * outerRadius,
                    0f
                );
                
                var outerPoint2 = center3D + new Vector3(
                    Mathf.Cos(angle2) * outerRadius,
                    Mathf.Sin(angle2) * outerRadius,
                    0f
                );
                
                var innerPoint1 = center3D + new Vector3(
                    Mathf.Cos(angle1) * innerRadius,
                    Mathf.Sin(angle1) * innerRadius,
                    0f
                );
                
                var innerPoint2 = center3D + new Vector3(
                    Mathf.Cos(angle2) * innerRadius,
                    Mathf.Sin(angle2) * innerRadius,
                    0f
                );
                
                GL.Vertex(outerPoint1);
                GL.Vertex(outerPoint2);
                GL.Vertex(innerPoint2);
                GL.Vertex(innerPoint1);
            }
            
            GL.End();
            GL.PopMatrix();
        }
        
        private void OnDestroy()
        {
            if (_glMaterial != null)
            {
                DestroyImmediate(_glMaterial);
            }
        }
        
        private float GetScreenScale()
        {
            if (!useScreenScaling) return scaleMultiplier;
            
            var scaleX = Screen.width / referenceResolution.x;
            var scaleY = Screen.height / referenceResolution.y;
            var baseScale = Mathf.Min(scaleX, scaleY);
            return baseScale * scaleMultiplier;
        }
        
        private static float GetNormalizedValue(float value, float min, float max)
        {
            return Mathf.Approximately(max, min) ? 0f : Mathf.Clamp01((value - min) / (max - min));
        }
    }
}
