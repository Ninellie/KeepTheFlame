using UnityEngine;

namespace ChunkSpawner
{
    public class Entity : MonoBehaviour
    {
        [field:SerializeField] public Vector2Int Chunk { get; set; }
    }
}