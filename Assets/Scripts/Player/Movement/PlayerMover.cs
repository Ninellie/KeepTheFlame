using System;
using Input;
using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace Player.Movement
{
    public class PlayerMover : IFixedTickable, IInitializable, IDisposable
    {
        private readonly Transform _playerTransform;
        private readonly PlayerMovementConfig _playerMovementConfig;
        private readonly InputManager _inputManager;

        private Vector2 _direction = Vector2.zero;
        private float _speed;
        
        public PlayerMover([Key("Player")] Transform playerTransform,
            PlayerMovementConfig playerMovementConfig, 
            InputManager inputManager)
        {
            _playerTransform = playerTransform;
            _playerMovementConfig = playerMovementConfig;
            _inputManager = inputManager;
        }
        
        public void FixedTick()
        {
            if (_direction  == Vector2.zero) return;
            var translation = _direction * _playerMovementConfig.startValue * Time.deltaTime;
            _playerTransform.Translate(translation, Space.World);
        }

        public void Dispose()
        {
            _inputManager.OnMove -= ChangeDirection;
        }

        public void Initialize()
        {
            _inputManager.OnMove += ChangeDirection;
        }

        private void ChangeDirection(Vector2 newDirection)
        {
            _direction = newDirection;
        }
    }
}