using DebugGUI;
using UnityEngine;
using VContainer;

namespace FirefliesSpawn
{
    public class DebugFireflySpawnerGUI : IDebugGUIWindow
    {
        public Rect Rect { get; set; } = new Rect(20, 20, 240, 120);
        public string Name { get; set; } = "Firefly Spawner Debug";
        
        private readonly FireflyPool _fireflyPool;
        private readonly SpawnTimer _spawnTimer;
        
        public DebugFireflySpawnerGUI(
            [Key(nameof(Firefly))] FireflyPool fireflyPool,
            [Key(nameof(Firefly))] SpawnTimer spawnTimer)
        {
            _fireflyPool = fireflyPool;
            _spawnTimer = spawnTimer;
        }

        public void DrawWindow(int id)
        {
            GUILayout.Label($"Active: {_fireflyPool.Active} / {_fireflyPool.Size}");
            GUILayout.Label($"Time to spawn: {_spawnTimer.TimeToNextSpawn:0.00}s");

            GUILayout.BeginHorizontal();
            if (GUILayout.Button("Spawn")) _spawnTimer.Set(0);
            if (GUILayout.Button("Clear")) _fireflyPool.ReturnAllToPool();
            if (GUILayout.Button("Init")) _fireflyPool.Init();
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