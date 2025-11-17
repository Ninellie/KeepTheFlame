using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;
using VContainer;

namespace DebugGUI
{
    public class DebugGUIController : MonoBehaviour
    {
        [Inject]
        private IEnumerable<IDebugGUIWindow> _windows;
        
        private bool _positioned;
        private bool _isVisible = true;
        
        private void Update()
        {
            if (Keyboard.current != null && Keyboard.current.f1Key.wasPressedThisFrame)
            {
                _isVisible = !_isVisible;
            }
        }
        
        private void OnGUI()
        {
            if (!_isVisible) return;
            
            if (!_positioned)
            {
                ArrangeWindows();
                _positioned = true;
            }
            
            var id = 42;
            foreach (var window in _windows)
            {
                window.Rect = GUI.Window(++id, window.Rect, window.DrawWindow, window.Name);
            }
        }
        
        private void ArrangeWindows()
        {
            var windowsList = _windows.OrderBy(w => w.Name).ToList();
            if (windowsList.Count == 0) return;
            
            var screenWidth = Screen.width;
            var screenHeight = Screen.height;
            const float margin = 20f;
            
            // Стандартный размер окна
            const float windowWidth = 240f;
            const float windowHeight = 120f;
            
            // Вычисляем количество колонок и строк, которое помещается на экране
            var maxCols = Mathf.Max(1, Mathf.FloorToInt((screenWidth - 2 * margin) / windowWidth));
            var maxRows = Mathf.Max(1, Mathf.FloorToInt((screenHeight - 2 * margin) / windowHeight));
            
            // Вычисляем равномерные интервалы между колонками и строками
            var colSpacing = maxCols > 1 ? (screenWidth - 2 * margin - maxCols * windowWidth) / (maxCols - 1) : 0f;
            var rowSpacing = maxRows > 1 ? (screenHeight - 2 * margin - maxRows * windowHeight) / (maxRows - 1) : 0f;
            
            var windowIndex = 0;
            
            // Распределяем окна по приоритету: левая колонка, правая колонка, верхняя строка, нижняя строка, и так далее
            
            // 1. Заполняем всю левую колонку (сверху вниз)
            for (var row = 0; row < maxRows && windowIndex < windowsList.Count; row++)
            {
                var window = windowsList[windowIndex++];
                var x = margin;
                var y = margin + row * (windowHeight + rowSpacing);
                window.Rect = new Rect(x, y, windowWidth, windowHeight);
            }
            
            // 2. Заполняем всю правую колонку (сверху вниз), если есть место для нескольких колонок
            if (maxCols > 1)
            {
                for (var row = 0; row < maxRows && windowIndex < windowsList.Count; row++)
                {
                    var window = windowsList[windowIndex++];
                    var x = margin + (maxCols - 1) * (windowWidth + colSpacing);
                    var y = margin + row * (windowHeight + rowSpacing);
                    window.Rect = new Rect(x, y, windowWidth, windowHeight);
                }
            }
            
            // 3. Заполняем верхнюю строку (промежуточные колонки)
            for (var col = 1; col < maxCols - 1 && windowIndex < windowsList.Count; col++)
            {
                var window = windowsList[windowIndex++];
                var x = margin + col * (windowWidth + colSpacing);
                var y = margin;
                window.Rect = new Rect(x, y, windowWidth, windowHeight);
            }
            
            // 4. Заполняем нижнюю строку (промежуточные колонки), если есть место для нескольких строк
            if (maxRows > 1)
            {
                for (var col = 1; col < maxCols - 1 && windowIndex < windowsList.Count; col++)
                {
                    var window = windowsList[windowIndex++];
                    var x = margin + col * (windowWidth + colSpacing);
                    var y = margin + (maxRows - 1) * (windowHeight + rowSpacing);
                    window.Rect = new Rect(x, y, windowWidth, windowHeight);
                }
            }
            
            // 5. Заполняем остальные ячейки центра
            for (var row = 1; row < maxRows - 1 && windowIndex < windowsList.Count; row++)
            {
                for (var col = 1; col < maxCols - 1 && windowIndex < windowsList.Count; col++)
                {
                    var window = windowsList[windowIndex++];
                    var x = margin + col * (windowWidth + colSpacing);
                    var y = margin + row * (windowHeight + rowSpacing);
                    window.Rect = new Rect(x, y, windowWidth, windowHeight);
                }
            }
        }
    }
}