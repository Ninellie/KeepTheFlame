using System;
using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace PlayerMovement
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
        
        private readonly PlayerMovementInputHandler _playerMovementInputHandler;
        private readonly Transform _playerTransform;
        private readonly PlayerMovementConfig _playerMovementConfig;
        
        private Vector2 _direction = Vector2.zero;
        
        public PlayerMover(PlayerMovementInputHandler playerMovementInputHandler,
            [Key("Player")] Transform playerTransform, PlayerMovementConfig playerMovementConfig)
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