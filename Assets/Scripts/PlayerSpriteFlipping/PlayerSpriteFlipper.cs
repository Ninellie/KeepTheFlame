using UnityEngine;
using VContainer.Unity;
using PlayerMovement;
using VContainer;

namespace PlayerSpriteFlipping
{
    public class PlayerSpriteFlipper : IStartable
    {
        private readonly PlayerMovementInputHandler _playerMovementInputHandler;
        private readonly SpriteRenderer _playerSpriteRenderer;
        
        public PlayerSpriteFlipper(PlayerMovementInputHandler playerMovementInputHandler,
            [Key("Player")] SpriteRenderer playerSpriteRenderer)
        {
            _playerMovementInputHandler = playerMovementInputHandler;
            _playerSpriteRenderer = playerSpriteRenderer;
        }

        public void Start()
        {
            _playerMovementInputHandler.OnRun += OnPlayerRun;
        }
        
        private void OnPlayerRun(Vector2 direction)
        {
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
