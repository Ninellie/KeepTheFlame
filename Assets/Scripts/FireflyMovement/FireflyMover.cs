using System.Collections.Generic;
using FirefliesSpawn;
using UnityEngine;
using VContainer.Unity;

namespace FireflyMovement
{
    public class FireflyMover : IStartable, IFixedTickable
    {
        private readonly List<Transform> _fireflyTransforms = new List<Transform>();
        private readonly FireflyMovementConfig _config;
        
        // Movement parameters
        public float MovementSpeed
        {
            get => _movementSpeed;
            private set => _movementSpeed = Mathf.Clamp(value, MinMovementSpeed, MaxMovementSpeed);
        }
        
        private float _movementSpeed;
        public float MaxMovementSpeed { get; private set; }
        public float MinMovementSpeed { get; private set; }
        
        // Rotation parameters
        public float RotationSpeed
        {
            get => _rotationSpeed;
            private set => _rotationSpeed = Mathf.Clamp(value, MinRotationSpeed, MaxRotationSpeed);
        }
        
        private float _rotationSpeed;
        public float MaxRotationSpeed { get; private set; }
        public float MinRotationSpeed { get; private set; }
        
        public FireflyMover(FireflyMovementConfig config)
        {
            _config = config;
            Init();
        }
        
        public void Init()
        {
            MaxMovementSpeed = _config.maxMovementSpeed;
            MinMovementSpeed = _config.minMovementSpeed;
            MovementSpeed = _config.startMovementSpeed;
            
            MaxRotationSpeed = _config.maxRotationSpeed;
            MinRotationSpeed = _config.minRotationSpeed;
            RotationSpeed = _config.startRotationSpeed;
        }
        
        public void Start()
        {
            Init();
        }
        
        public void FixedTick()
        {
            var deltaTime = Time.fixedDeltaTime;
            
            foreach (var fireflyTransform in _fireflyTransforms)
            {
                if (fireflyTransform == null) continue;
                
                // Сдвигаем светлячка вперед по его текущему направлению
                fireflyTransform.Translate(Vector3.up * MovementSpeed * deltaTime, Space.Self);
            }
        }
        
        public void RegisterFirefly(Firefly firefly)
        {
            var transform = firefly.transform;
            if (!_fireflyTransforms.Contains(transform))
            {
                _fireflyTransforms.Add(transform);
            }
        }
        
        public void UnregisterFirefly(Firefly firefly)
        {
            _fireflyTransforms.Remove(firefly.transform);
        }
    }
}
