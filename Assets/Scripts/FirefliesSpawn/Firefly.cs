using UnityEngine;

namespace FirefliesSpawn
{
    public class Firefly : MonoBehaviour
    {
        public Vector2Int Sector { get; set; }
        
        private FireflyPool _pool;
    
        public void Init(FireflyPool pool)
        {
            _pool = pool;
        }
    
        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.CompareTag("Player"))
            {
                _pool.ReturnToPool(this);
            }
        }
    }
}