using DebugGUI;
using UnityEngine;

namespace LampFlame
{
    public class DebugLampFlameGUI : IDebugGUIWindow
    {
        public Rect Rect { get; set; } = new Rect(20, 20, 240, 120);
        public string Name { get; set; } = "Lamp Flame Debug";
        
        private readonly LampFlamePower _flame;
        
        public DebugLampFlameGUI(LampFlamePower flame)
        {
            _flame = flame;
        }

        public void DrawWindow(int id)
        {
            GUILayout.Label($"Value: {_flame.Value:0.0} / {_flame.Max:0}");
            GUILayout.Label($"Is Lit: {_flame.IsLit}");

            GUILayout.BeginHorizontal();
            if (GUILayout.Button("Init")) _flame.Init();
            GUILayout.EndHorizontal();

            var pct = Mathf.Approximately(_flame.Max, 0) ? 0f : _flame.Value / _flame.Max;
            var bar = GUILayoutUtility.GetRect(200, 18);
            GUI.Box(bar, GUIContent.none);
            var fill = new Rect(bar.x + 2, bar.y + 2, Mathf.Max(0, (bar.width - 4) * pct), bar.height - 4);
            GUI.Box(fill, GUIContent.none);

            GUI.DragWindow(new Rect(0, 0, 10000, 20));
        }
    }
}