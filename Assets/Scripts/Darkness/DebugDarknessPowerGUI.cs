using DebugGUI;
using UnityEngine;

namespace Darkness
{
    public class DebugDarknessPowerGUI : IDebugGUIWindow
    {
        public Rect Rect { get; set; } = new Rect(20, 20, 240, 120);
        public string Name { get; set; } = "Darkness Power Debug";
        
        private readonly DarknessPower _darknessPower;
        
        public DebugDarknessPowerGUI(DarknessPower darknessPower)
        {
            _darknessPower = darknessPower;
        }

        public void DrawWindow(int id)
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
    }
}