using UnityEngine;

namespace Spawning
{
    public interface IPooledEntity
    {
        Vector2Int Sector { get; set; }
        Transform Transform { get; }
        GameObject GameObject { get; }
        void SetPool(EntityPool pool);
    }
}