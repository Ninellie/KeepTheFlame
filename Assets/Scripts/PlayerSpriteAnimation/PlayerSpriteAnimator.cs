using UnityEngine;
using VContainer;
using VContainer.Unity;
using PlayerMovement;

namespace PlayerSpriteAnimation
{
    public class PlayerSpriteAnimator : IStartable
    {
        private readonly PlayerMovementInputHandler _playerMovementInputHandler;
        private readonly Animator _playerAnimator;
        
        public PlayerSpriteAnimator(PlayerMovementInputHandler playerMovementInputHandler,
            [Key("Player")] Animator playerAnimator)
        {
            _playerMovementInputHandler = playerMovementInputHandler;
            _playerAnimator = playerAnimator;
        }

        public void Start()
        {
            _playerMovementInputHandler.OnRun += OnPlayerRun;
            _playerMovementInputHandler.OnIdle += OnPlayerIdle;
        }
        
        private void OnPlayerRun(Vector2 direction)
        {
            _playerAnimator.SetBool("IsRunning", true);
        }
        
        private void OnPlayerIdle()
        {
            _playerAnimator.SetBool("IsRunning", false);
        }
    }
}
