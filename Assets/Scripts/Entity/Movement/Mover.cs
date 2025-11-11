using Entity.Movement;
using UnityEngine;

namespace EntityMovement
{
    public class Mover : MonoBehaviour
    {
        [SerializeField] private MovementConfig config;

        private Transform _transform; 
        
        private void Awake()
        {
            _transform = GetComponent<Transform>();
        }

        public void FixedUpdate()
        {
            if (config == null)
            {
                Debug.LogError("Config is null");
                return;
            }
            
            var deltaTime = Time.fixedDeltaTime;
            
            _transform.Translate(Vector3.up * (config.MovementSpeed * deltaTime), Space.Self);
        }
    }
}
