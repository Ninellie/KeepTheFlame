using UnityEngine;
using VContainer;

namespace FirefliesSpawn
{
    public class DebugFireflySpawnerGUI : MonoBehaviour
    {
        [Inject]
        private FireflyPool _fireflyPool;
        
        [Inject]
        private SpawnTimer _spawnTimer;
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
            _win = GUI.Window(47, _win, DrawWindow, "Firefly Spawner Debug");
        }

        private void DrawWindow(int id)
        {
            GUILayout.Label($"Active: {_fireflyPool.Active} / {_fireflyPool.Size}");
            GUILayout.Label($"Time to spawn: {_spawnTimer.TimeToNextSpawn:0.1}s");

            GUILayout.BeginHorizontal();
            if (GUILayout.Button("Spawn")) _spawnTimer.Set(0);
            if (GUILayout.Button("Clear")) _fireflyPool.ReturnAllToPool();
            GUILayout.EndHorizontal();

            var pct = _fireflyPool.Size == 0 ? 0f : (float)_fireflyPool.Active / _fireflyPool.Size;
            var bar = GUILayoutUtility.GetRect(200, 18);
            GUI.Box(bar, GUIContent.none);
            var fill = new Rect(bar.x + 2, bar.y + 2, Mathf.Max(0, (bar.width - 4) * pct), bar.height - 4);
            GUI.Box(fill, GUIContent.none);

            GUI.DragWindow(new Rect(0, 0, 10000, 20));
        }
    }
}