using DebugGUI;
using UnityEngine;

namespace LampFuel
{
    public class DebugLampFuelGUI : IDebugGUIWindow
    {
        public string Name { get; set; } = "Lamp Fuel Debug";
        public Rect Rect { get; set; } = new Rect(20, 20, 240, 120);
        
        private readonly LampFuelTank _lamp;
        
        public DebugLampFuelGUI(LampFuelTank lamp)
        {
            _lamp = lamp;
        }

        public void DrawWindow(int id)
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
    }
}