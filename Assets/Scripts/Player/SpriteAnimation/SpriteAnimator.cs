using System;
using Input;
using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace Player.SpriteAnimation
{
    public class SpriteAnimator : IInitializable, IDisposable
    {
        private readonly InputManager _inputManager;
        private readonly Animator _playerAnimator;
        private readonly SpriteRenderer _playerSpriteRenderer;
        
        public SpriteAnimator(InputManager inputManager, 
            [Key("Player")] Animator playerAnimator, 
            [Key("Player")]SpriteRenderer playerSpriteRenderer)
        {
            _playerAnimator = playerAnimator;
            _inputManager = inputManager;
            _playerSpriteRenderer = playerSpriteRenderer;
        }

        public void Initialize()
        {
            _inputManager.OnMove += OnMove;
        }

        public void Dispose()
        {
            _inputManager?.Dispose();
        }

        private void OnMove(Vector2 direction)
        {
            var isRunning = direction != Vector2.zero;
            _playerAnimator.SetBool("IsRunning", isRunning);
            
            if (direction.x < 0)
            {
                _playerSpriteRenderer.flipX = true;
            }
            else if (direction.x > 0)
            {
                _playerSpriteRenderer.flipX = false;
            }
        }
    }
}
