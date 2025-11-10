using UnityEngine;

namespace ChunkSpawner
{
    public class GizmoSquare : MonoBehaviour
    {
        private Vector2 _bottomLeft;
        private Vector2 _topRight;
        private bool _isActive;
        private readonly Color _gizmoColor = Color.green;

        public void Set(Vector2 bottomLeft, Vector2 topRight)
        {
            _bottomLeft = bottomLeft;
            _topRight = topRight;
            _isActive = true;
        }

        private void OnDrawGizmos()
        {
            if (!_isActive) return;

            Gizmos.color = _gizmoColor;

            var bl = new Vector3(_bottomLeft.x, _bottomLeft.y, 0);
            var br = new Vector3(_topRight.x, _bottomLeft.y, 0);
            var tr = new Vector3(_topRight.x, _topRight.y, 0);
            var tl = new Vector3(_bottomLeft.x, _topRight.y, 0);
            
            Gizmos.DrawLine(bl, br);
            Gizmos.DrawLine(br, tr);
            Gizmos.DrawLine(tr, tl);
            Gizmos.DrawLine(tl, bl);
        }
    }
}