using Input;
using Pause;
using UnityEngine;
using VContainer;

namespace UI
{
    public class PauseMenu : MonoBehaviour
    {
        [SerializeField] private GameObject menu;
        
        [Inject] private PauseManager _pauseManager;
        [Inject] private InputManager _inputManager;

        private void Start()
        {
            _inputManager.OnPause += ShowMenu;
            _inputManager.OnUnpause += CloseMenu;
            CloseMenu();
        }

        private void OnDestroy()
        {
            _inputManager.OnPause -= ShowMenu;
            _inputManager.OnUnpause -= CloseMenu;
        }

        public void Unpause()
        {
            _pauseManager.Unpause();
        }

        public void Restart()
        {
            PauseManager.RestartScene();
        }

        public void MainMenu()
        {
            PauseManager.LoadMainMenu();
        }

        private void ShowMenu()
        {
            menu.SetActive(true);
        }

        private void CloseMenu()
        {
            menu.SetActive(false);
        }
    }
}
