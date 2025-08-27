using UnityEngine;
using VContainer;

namespace PlayerHealth
{
    public class DebugPlayerHealthGUI : MonoBehaviour
    {
        [Inject]
        private PlayerHealthCounter _playerHealthCounter;
        private Rect _win = new Rect(20, 20, 240, 120);
        private bool _visible = true;
    
#if !UNITY_EDITOR
        private const bool EnabledInBuild = true; // Поставить false, чтобы скрыть в билде
#else
        private const bool EnabledInBuild = true;
#endif
    
        private void OnGUI()
        {
            if (!EnabledInBuild || !_visible) return;
            _win = GUI.Window(44, _win, DrawWindow, "Player Health Debug");
        }

        private void DrawWindow(int id)
        {
            GUILayout.Label($"Value: {_playerHealthCounter.Value:0} / {_playerHealthCounter.Max:0}");

            GUILayout.BeginHorizontal();
            if (GUILayout.Button("+1")) _playerHealthCounter.Increase(1);
            if (GUILayout.Button("-1")) _playerHealthCounter.Decrease(1);
            if (GUILayout.Button("Fill")) _playerHealthCounter.Increase(_playerHealthCounter.Max);
            if (GUILayout.Button("Init")) _playerHealthCounter.Init();
            GUILayout.EndHorizontal();

            var pct = Mathf.Approximately(_playerHealthCounter.Max, 0) ? 0f : _playerHealthCounter.Value / _playerHealthCounter.Max;
            var bar = GUILayoutUtility.GetRect(200, 18);
            GUI.Box(bar, GUIContent.none);
            var fill = new Rect(bar.x + 2, bar.y + 2, Mathf.Max(0, (bar.width - 4) * pct), bar.height - 4);
            GUI.Box(fill, GUIContent.none);

            GUI.DragWindow(new Rect(0, 0, 10000, 20));
        }

        private void OnDestroy()
        {
            if (_playerHealthCounter != null) _playerHealthCounter.OnChanged -= null; // на случай подписок в будущем
        }
    }
}