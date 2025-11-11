using Player.Movement;
using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace Player.SpriteAnimation
{
    public class SpriteAnimator : IStartable
    {
        private readonly MovementInputHandler _movementInputHandler;
        private readonly Animator _playerAnimator;
        
        public SpriteAnimator(MovementInputHandler movementInputHandler,
            [Key("Player")] Animator playerAnimator)
        {
            _movementInputHandler = movementInputHandler;
            _playerAnimator = playerAnimator;
        }

        public void Start()
        {
            _movementInputHandler.OnRun += Run;
            _movementInputHandler.OnIdle += Idle;
        }
        
        private void Run(Vector2 direction)
        {
            _playerAnimator.SetBool("IsRunning", true);
        }
        
        private void Idle()
        {
            _playerAnimator.SetBool("IsRunning", false);
        }
    }
}
