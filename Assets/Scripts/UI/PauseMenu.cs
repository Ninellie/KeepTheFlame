using Input;
using Pause;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using VContainer;

namespace UI
{
    public class PauseMenu : MonoBehaviour
    {
        [Header("Visual Settings")]
        [SerializeField] private Color textColor = new(1f, 1f, 1f, 1f);
        [SerializeField] private int fontSize = 72;
        [SerializeField] private FontStyle fontStyle = FontStyle.Bold;
        
        [Header("Button Settings")]
        [SerializeField] private Color buttonColor = new(0.5f, 0.5f, 0.5f, 0.8f);
        [SerializeField] private Color buttonHoverColor = new(0.7f, 0.7f, 0.7f, 0.8f);
        [SerializeField] private Color buttonTextColor = new(1f, 1f, 1f, 1f);
        [SerializeField] private int buttonFontSize = 36;
        [SerializeField] private float buttonSpacing = 50f;
        [SerializeField] private Vector2 buttonSize = new(200f, 50f);
        
        [Header("Position")]
        [SerializeField] private UIPosition uiPosition = new()
        {
            AnchorMin = new Vector2(0.5f, 0.5f),
            AnchorMax = new Vector2(0.5f, 0.5f),
            Pivot = new Vector2(0.5f, 0.5f),
            Offset = Vector2.zero
        };
        
        [Header("Screen Scaling")]
        [SerializeField] private Vector2 referenceResolution = new(1920f, 1080f);
        [SerializeField] private bool useScreenScaling = true;
        [Range(0.1f, 10f)]
        [SerializeField] private float scaleMultiplier = 1f;
        
        [Inject] private PauseManager _pauseManager;
        [Inject] private InputManager _inputManager;
        
        private GUIStyle _textStyle;
        private GUIStyle _buttonStyle;
        private Texture2D _buttonNormalTexture;
        private Texture2D _buttonHoverTexture;
        private InputAction _restartAction;
        private InputAction _pauseAction;
        private bool _actionsSubscribed;
        
        private void Awake()
        {
            CreateTextStyle();
            CreateButtonStyle();
            CreateButtonTextures();
        }

        private void OnEnable()
        {
            _inputManager.OnRestart += RestartScene;
        }
        
        private void OnDisable()
        {
            _inputManager.OnRestart -= RestartScene;
        }
        
        private void OnValidate()
        {
            if (referenceResolution.x < 100f) referenceResolution.x = 100f;
            if (referenceResolution.y < 100f) referenceResolution.y = 100f;
            if (referenceResolution.x > 10000f) referenceResolution.x = 10000f;
            if (referenceResolution.y > 10000f) referenceResolution.y = 10000f;
            
            uiPosition?.OnValidate();
            CreateTextStyle();
            CreateButtonStyle();
        }
        
        private void OnGUI()
        {
            if (_pauseManager == null || !_pauseManager.IsPaused) return;
            
            DrawPauseMenu();
        }
        
        private void DrawPauseMenu()
        {
            var scale = GetScreenScale();
            var scaledFontSize = Mathf.RoundToInt(fontSize * scale);
            var scaledButtonFontSize = Mathf.RoundToInt(buttonFontSize * scale);
            var scaledButtonSpacing = buttonSpacing * scale;
            var scaledButtonSize = buttonSize * scale;
            
            _textStyle.fontSize = scaledFontSize;
            _textStyle.normal.textColor = textColor;
            
            _buttonStyle.fontSize = scaledButtonFontSize;
            _buttonStyle.normal.textColor = buttonTextColor;
            _buttonStyle.hover.textColor = buttonTextColor;
            _buttonStyle.normal.background = _buttonNormalTexture;
            _buttonStyle.hover.background = _buttonHoverTexture;
            
            const string text = "Pause";
            var content = new GUIContent(text);
            var textSize = _textStyle.CalcSize(content);
            
            var screenSize = new Vector2(Screen.width, Screen.height);
            var textPosition = uiPosition.CalculateScreenPosition(textSize, screenSize);
            
            GUI.Label(new Rect(textPosition.x, textPosition.y, textSize.x, textSize.y), text, _textStyle);
            
            var buttonPosition = new Vector2(
                textPosition.x + (textSize.x - scaledButtonSize.x) / 2f,
                textPosition.y + textSize.y + scaledButtonSpacing
            );
            
            if (GUI.Button(
                    new Rect(buttonPosition.x, buttonPosition.y, scaledButtonSize.x, scaledButtonSize.y),
                    "Restart", _buttonStyle))
            {
                RestartScene();
            }
        }
        
        private void CreateTextStyle()
        {
            _textStyle = new GUIStyle
            {
                fontStyle = fontStyle,
                alignment = TextAnchor.MiddleCenter
            };
        }
        
        private void CreateButtonStyle()
        {
            _buttonStyle = new GUIStyle
            {
                fontStyle = FontStyle.Normal,
                alignment = TextAnchor.MiddleCenter
            };
        }
        
        private void CreateButtonTextures()
        {
            _buttonNormalTexture = CreateColorTexture(buttonColor);
            _buttonHoverTexture = CreateColorTexture(buttonHoverColor);
        }
        
        private static Texture2D CreateColorTexture(Color color)
        {
            var texture = new Texture2D(1, 1);
            texture.SetPixel(0, 0, color);
            texture.Apply();
            return texture;
        }
        
        private static void RestartScene()
        {
            var currentScene = SceneManager.GetActiveScene();
            SceneManager.LoadScene(currentScene.buildIndex);
        }
        
        private void OnDestroy()
        {
            if (_buttonNormalTexture != null)
            {
                DestroyImmediate(_buttonNormalTexture);
            }
            
            if (_buttonHoverTexture != null)
            {
                DestroyImmediate(_buttonHoverTexture);
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
    }
}

