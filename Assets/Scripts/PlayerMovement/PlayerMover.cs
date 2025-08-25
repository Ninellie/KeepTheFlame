using UnityEngine;
using VContainer.Unity;

namespace PlayerMovement
{
    public class PlayerMover : IFixedTickable
    {
        public float Speed {get; private set;}
        public float MaxSpeed {get; private set;}
        public float MinSpeed {get; private set;}
        
        private readonly PlayerMovementInputHandler _playerMovementInputHandler;
        private readonly PlayerTransform _playerTransform;
        private readonly PlayerMovementConfig _playerMovementConfig;
        
        private Vector2 _direction = Vector2.zero;
        
        public PlayerMover(PlayerMovementInputHandler playerMovementInputHandler, PlayerTransform playerTransform, PlayerMovementConfig playerMovementConfig)
        {
            _playerMovementInputHandler = playerMovementInputHandler;
            _playerTransform = playerTransform;
            _playerMovementConfig = playerMovementConfig;

            _playerMovementInputHandler.OnIdle += () => { _direction = Vector2.zero; };
            _playerMovementInputHandler.OnRun += vector2 => { _direction = vector2; };
            
            Init();
        }

        public void Init()
        {
            Speed = _playerMovementConfig.startValue;
            MaxSpeed = _playerMovementConfig.maxValue;
            MinSpeed = _playerMovementConfig.minValue;
        }
        
        public void FixedTick()
        {
            _playerTransform.Value.Translate(_direction * Speed * Time.deltaTime, Space.World);
        }
    }
}