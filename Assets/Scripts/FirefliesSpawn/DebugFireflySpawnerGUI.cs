using DebugGUI;
using Spawning;
using UnityEngine;
using VContainer;

namespace FirefliesSpawn
{
    public class DebugFireflySpawnerGUI : IDebugGUIWindow
    {
        public Rect Rect { get; set; } = new Rect(20, 20, 240, 120);
        public string Name { get; set; } = "Firefly Spawner Debug";
        
        private readonly EntityPool _pool;
        private readonly SpawnTimer _timer;
        
        public DebugFireflySpawnerGUI(FireflySpawner spawner)
        {
            _pool = spawner.Pool;
            _timer = spawner.Timer;
        }

        public void DrawWindow(int id)
        {
            GUILayout.Label($"Active: {_pool.Active} / {_pool.Size}");
            GUILayout.Label($"Time to spawn: {_timer.TimeToNextSpawn:0.00}s");

            GUILayout.BeginHorizontal();
            if (GUILayout.Button("Spawn")) _timer.Set(0);
            if (GUILayout.Button("Clear")) _pool.ReturnAllToPool();
            if (GUILayout.Button("Init")) _pool.Init();
            GUILayout.EndHorizontal();

            var pct = _pool.Size == 0 ? 0f : (float)_pool.Active / _pool.Size;
            var bar = GUILayoutUtility.GetRect(200, 18);
            GUI.Box(bar, GUIContent.none);
            var fill = new Rect(bar.x + 2, bar.y + 2, Mathf.Max(0, (bar.width - 4) * pct), bar.height - 4);
            GUI.Box(fill, GUIContent.none);

            GUI.DragWindow(new Rect(0, 0, 10000, 20));
        }
    }
}