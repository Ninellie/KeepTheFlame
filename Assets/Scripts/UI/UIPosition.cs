using UnityEngine;

namespace UI
{
    [System.Serializable]
    public class UIPosition
    {
        [Header("Anchor")]
        [SerializeField] private Vector2 anchorMin = new(0f, 1f);
        [SerializeField] private Vector2 anchorMax = new(0f, 1f);
        
        [Header("Pivot")]
        [SerializeField] private Vector2 pivot = new(0f, 1f);
        
        [Header("Offset")]
        [SerializeField] private Vector2 offset = Vector2.zero;
        
        public Vector2 AnchorMin
        {
            get => anchorMin;
            set => anchorMin = value;
        }
        
        public Vector2 AnchorMax
        {
            get => anchorMax;
            set => anchorMax = value;
        }
        
        public Vector2 Pivot
        {
            get => pivot;
            set => pivot = value;
        }
        
        public Vector2 Offset
        {
            get => offset;
            set => offset = value;
        }
        
        public Vector2 CalculateScreenPosition(Vector2 elementSize, Vector2 screenSize)
        {
            var anchorCenterX = (anchorMin.x + anchorMax.x) * 0.5f;
            var anchorCenterY = (anchorMin.y + anchorMax.y) * 0.5f;
            
            var anchorPositionX = anchorCenterX * screenSize.x;
            var anchorPositionY = anchorCenterY * screenSize.y;
            
            var pivotOffsetX = (pivot.x - 0.5f) * elementSize.x;
            var pivotOffsetY = (pivot.y - 0.5f) * elementSize.y;
            
            var screenX = anchorPositionX + offset.x + pivotOffsetX;
            var screenY = screenSize.y - (anchorPositionY + offset.y) - pivotOffsetY;
            
            return new Vector2(screenX, screenY);
        }
        
        public void OnValidate()
        {
            anchorMin.x = Mathf.Clamp01(anchorMin.x);
            anchorMin.y = Mathf.Clamp01(anchorMin.y);
            anchorMax.x = Mathf.Clamp01(anchorMax.x);
            anchorMax.y = Mathf.Clamp01(anchorMax.y);
            pivot.x = Mathf.Clamp01(pivot.x);
            pivot.y = Mathf.Clamp01(pivot.y);
        }
    }
}

