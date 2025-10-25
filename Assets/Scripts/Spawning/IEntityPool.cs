using UnityEngine;

namespace Spawning
{
    public interface IEntityPool
    {
        int Size { get; }
        int Active { get; }
        bool IsSectorFull(Vector2Int sector);
        IPooledEntity GetFromPool();
        void Init();
        void ReturnAllToPool();
    }
}