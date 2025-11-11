using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace Player.Movement
{
    public class PlayerMover : IFixedTickable
    {
        public float Speed
        {
            get => _speed;
            private set => _speed = Mathf.Clamp(value, MinSpeed, MaxSpeed);
        }

        private float _speed;
        
        public float MaxSpeed { get; private set; }
        public float MinSpeed { get; private set; }
        
        private readonly MovementInputHandler _movementInputHandler;
        private readonly Transform _playerTransform;
        private readonly PlayerMovementConfig _playerMovementConfig;
        
        private Vector2 _direction = Vector2.zero;
        
        public PlayerMover(MovementInputHandler movementInputHandler,
            [Key("Player")] Transform playerTransform, PlayerMovementConfig playerMovementConfig)
        {
            _movementInputHandler = movementInputHandler;
            _playerTransform = playerTransform;
            _playerMovementConfig = playerMovementConfig;

            _movementInputHandler.OnIdle += () => { _direction = Vector2.zero; };
            _movementInputHandler.OnRun += vector2 => { _direction = vector2; };
            
            Init();
        }

        public void Init()
        {
            MaxSpeed = _playerMovementConfig.maxValue;
            MinSpeed = _playerMovementConfig.minValue;
            Speed = _playerMovementConfig.startValue;
        }
        
        public void FixedTick()
        {
            _playerTransform.Translate(_direction * Speed * Time.deltaTime, Space.World);
        }
    }
}