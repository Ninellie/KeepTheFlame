using DebugGUI;
using UnityEngine;

namespace DarknessDamage
{
    public class DebugDarknessDamageGUI : IDebugGUIWindow
    {
        public Rect Rect { get; set; } = new Rect(20, 20, 240, 120);
        public string Name { get; set; } = "Darkness Damage Debug";
        
        private DarknessDamageDealer _dealer;

        public void SetDealer(DarknessDamageDealer dealer)
        {
            _dealer = dealer;
        }

        public void DrawWindow(int id)
        {
            GUILayout.Label($"Damage: {_dealer.DamageAmount}");
            GUILayout.Label($"Interval: {_dealer.DamageDealInterval}");
            
            GUILayout.Label($"Time to hit: {_dealer.SecondsToNextDamage:0.0} sec");

            GUILayout.BeginHorizontal();
            if (GUILayout.Button("Hit")) _dealer.DealDamage();
            if (GUILayout.Button("Init")) _dealer.Init();
            GUILayout.EndHorizontal();

            var pct = _dealer.DamageDealInterval == 0 ? 0f : _dealer.SecondsToNextDamage / _dealer.DamageDealInterval;
            var bar = GUILayoutUtility.GetRect(200, 18);
            GUI.Box(bar, GUIContent.none);
            var fill = new Rect(bar.x + 2, bar.y + 2, Mathf.Max(0, (bar.width - 4) * pct), bar.height - 4);
            GUI.Box(fill, GUIContent.none);

            GUI.DragWindow(new Rect(0, 0, 10000, 20));
        }
    }
}