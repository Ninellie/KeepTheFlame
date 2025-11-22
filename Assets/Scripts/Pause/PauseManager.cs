using System;
using Input;
using UnityEngine;
using VContainer.Unity;

namespace Pause
{
    public class PauseManager : IInitializable, IDisposable
    {
        public bool IsPaused { get; private set; }
        
        private readonly InputManager _inputManager;
        private bool _actionsSubscribed;
        
        public PauseManager(InputManager inputManager)
        {
            _inputManager = inputManager;
            IsPaused = false;
            Time.timeScale = 1f;
            AudioListener.pause = false;
        }

        public void Initialize()
        {
            _inputManager.OnPause += Pause;
            _inputManager.OnUnpause += Unpause;
        }

        public void Dispose()
        {
            _inputManager.OnPause -= Pause;
            _inputManager.OnUnpause -= Unpause;
        }

        private void Pause()
        {
            IsPaused = true;
            Time.timeScale = 0f;
            AudioListener.pause = true;
            _inputManager.SwitchToUIMap();
        }

        private void Unpause()
        {
            IsPaused = false;
            Time.timeScale = 1f;
            AudioListener.pause = false;
            _inputManager.SwitchToPlayerMap();
        }
    }
}

