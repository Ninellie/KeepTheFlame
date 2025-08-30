using UnityEngine;

namespace DebugGUI
{
    public interface IDebugGUIWindow
    {
        string Name { get; set; }
        Rect Rect { get; set; }
        void DrawWindow(int id);
    }
}