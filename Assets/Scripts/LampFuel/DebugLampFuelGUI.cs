using UnityEngine;
using VContainer;

namespace LampFuel
{
    public class DebugLampFuelGUI : MonoBehaviour
    {
        [Inject]
        private LampFuelTank _lamp;
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
            _win = GUI.Window(42, _win, DrawWindow, "Lamp Fuel Debug");
        }

        private void DrawWindow(int id)
        {
            GUILayout.Label($"Value: {_lamp.Value:0.0} / {_lamp.Max:0}");

            GUILayout.BeginHorizontal();
            if (GUILayout.Button("-1")) _lamp.Add(-1f);
            if (GUILayout.Button("+1")) _lamp.Add(1f);
            if (GUILayout.Button("Fill")) _lamp.Add(_lamp.Max);
            if (GUILayout.Button("Init")) _lamp.Init();
            GUILayout.EndHorizontal();

            var pct = Mathf.Approximately(_lamp.Max, 0) ? 0f : _lamp.Value / _lamp.Max;
            var bar = GUILayoutUtility.GetRect(200, 18);
            GUI.Box(bar, GUIContent.none);
            var fill = new Rect(bar.x + 2, bar.y + 2, Mathf.Max(0, (bar.width - 4) * pct), bar.height - 4);
            GUI.Box(fill, GUIContent.none);

            GUI.DragWindow(new Rect(0, 0, 10000, 20));
        }

        public Vector2 GetWindowSize()
        {
            return new Vector2(_win.width, _win.height);
        }
        
        public void SetWindowPosition(Vector2 position)
        {
            _win.x = position.x;
            _win.y = position.y;
        }
        
        public void SetWindowSize(Vector2 size)
        {
            _win.width = size.x;
            _win.height = size.y;
        }

        private void OnDestroy()
        {
            if (_lamp != null) _lamp.OnChanged -= null; // на случай подписок в будущем
        }
    }
}