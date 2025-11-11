using Player.Movement;
using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace Player.SpriteFlipping
{
    public class SpriteFlipper : IStartable
    {
        private readonly MovementInputHandler _movementInputHandler;
        private readonly SpriteRenderer _playerSpriteRenderer;
        
        public SpriteFlipper(MovementInputHandler movementInputHandler,
            [Key("Player")] SpriteRenderer playerSpriteRenderer)
        {
            _movementInputHandler = movementInputHandler;
            _playerSpriteRenderer = playerSpriteRenderer;
        }

        public void Start()
        {
            _movementInputHandler.OnRun += Run;
        }
        
        private void Run(Vector2 direction)
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
