using UnityEngine;
using VContainer;

namespace Darkness
{
    public class DebugDarknessPowerGUI : MonoBehaviour
    {
        [Inject]
        private DarknessPower _darknessPower;
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
            _win = GUI.Window(43, _win, DrawWindow, "Darkness Power Debug");
        }

        private void DrawWindow(int id)
        {
            GUILayout.Label($"Value: {_darknessPower.Value:0.0} / {_darknessPower.Max:0}");

            GUILayout.BeginHorizontal();
            if (GUILayout.Button("+1")) _darknessPower.Increase(1f);
            if (GUILayout.Button("-1")) _darknessPower.Decrease(1f);
            if (GUILayout.Button("Fill")) _darknessPower.Increase(_darknessPower.Max);
            if (GUILayout.Button("Init")) _darknessPower.Init();
            GUILayout.EndHorizontal();

            var pct = Mathf.Approximately(_darknessPower.Max, 0) ? 0f : _darknessPower.Value / _darknessPower.Max;
            var bar = GUILayoutUtility.GetRect(200, 18);
            GUI.Box(bar, GUIContent.none);
            var fill = new Rect(bar.x + 2, bar.y + 2, Mathf.Max(0, (bar.width - 4) * pct), bar.height - 4);
            GUI.Box(fill, GUIContent.none);

            GUI.DragWindow(new Rect(0, 0, 10000, 20));
        }

        private void OnDestroy()
        {
            if (_darknessPower != null) _darknessPower.OnChanged -= null; // на случай подписок в будущем
        }
    }
}