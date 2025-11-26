using System;
using Input;
using UnityEngine;
using UnityEngine.SceneManagement;
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
            _inputManager.OnRestart += RestartScene;
        }

        public void Dispose()
        {
            _inputManager.OnPause -= Pause;
            _inputManager.OnUnpause -= Unpause;
            _inputManager.OnRestart -= RestartScene;
        }

        private void Pause()
        {
            IsPaused = true;
            Time.timeScale = 0f;
            AudioListener.pause = true;
            _inputManager.SwitchToUIMap();
        }

        public void Unpause()
        {
            IsPaused = false;
            Time.timeScale = 1f;
            AudioListener.pause = false;
            _inputManager.SwitchToPlayerMap();
        }
        
        public static void RestartScene()
        {
            var currentScene = SceneManager.GetActiveScene();
            SceneManager.LoadScene(currentScene.buildIndex);
        }
        
        public static void LoadMainMenu()
        {
            SceneManager.LoadScene(0);
        }
    }
}

