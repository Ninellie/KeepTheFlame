using UnityEngine;

namespace UI
{
    public class GameTimer : MonoBehaviour
    {
        [Header("Visual Settings")]
        [SerializeField] private Color textColor = new(1f, 1f, 1f, 0.8f);
        [SerializeField] private int fontSize = 24;
        [SerializeField] private FontStyle fontStyle = FontStyle.Normal;
        
        [Header("Position")]
        [SerializeField] private UIPosition uiPosition = new()
        {
            AnchorMin = new Vector2(0f, 1f),
            AnchorMax = new Vector2(0f, 1f),
            Pivot = new Vector2(0f, 1f),
            Offset = new Vector2(50f, -50f)
        };
        
        [Header("Screen Scaling")]
        [SerializeField] private Vector2 referenceResolution = new(1920f, 1080f);
        [SerializeField] private bool useScreenScaling = true;
        [Range(0.1f, 10f)]
        [SerializeField] private float scaleMultiplier = 1f;
        
        private GUIStyle _textStyle;
        private float _elapsedTime;
        
        private void Awake()
        {
            CreateTextStyle();
            _elapsedTime = 600f;
        }
        
        private void OnValidate()
        {
            if (referenceResolution.x < 100f) referenceResolution.x = 100f;
            if (referenceResolution.y < 100f) referenceResolution.y = 100f;
            if (referenceResolution.x > 10000f) referenceResolution.x = 10000f;
            if (referenceResolution.y > 10000f) referenceResolution.y = 10000f;
            
            uiPosition?.OnValidate();
            CreateTextStyle();
        }
        
        private void Update()
        {
            _elapsedTime -= Time.deltaTime;
        }
        
        private void OnGUI()
        {
            DrawTimer();
        }
        
        private void DrawTimer()
        {
            var scale = GetScreenScale();
            var scaledFontSize = Mathf.RoundToInt(fontSize * scale);
            
            _textStyle.fontSize = scaledFontSize;
            _textStyle.normal.textColor = textColor;
            
            var timeString = FormatTime(_elapsedTime);
            var content = new GUIContent(timeString);
            var textSize = _textStyle.CalcSize(content);
            
            var screenSize = new Vector2(Screen.width, Screen.height);
            var screenPosition = uiPosition.CalculateScreenPosition(textSize, screenSize);
            
            GUI.Label(new Rect(screenPosition.x, screenPosition.y, textSize.x, textSize.y), timeString, _textStyle);
        }
        
        private void CreateTextStyle()
        {
            _textStyle = new GUIStyle
            {
                fontStyle = fontStyle,
                alignment = TextAnchor.UpperLeft
            };
        }
        
        private static string FormatTime(float timeInSeconds)
        {
            var minutes = Mathf.FloorToInt(timeInSeconds / 60f);
            var seconds = Mathf.FloorToInt(timeInSeconds % 60f);
            return $"{minutes:00}:{seconds:00}";
        }
        
        private float GetScreenScale()
        {
            if (!useScreenScaling) return scaleMultiplier;
            
            var scaleX = Screen.width / referenceResolution.x;
            var scaleY = Screen.height / referenceResolution.y;
            var baseScale = Mathf.Min(scaleX, scaleY);
            return baseScale * scaleMultiplier;
        }
    }
}

