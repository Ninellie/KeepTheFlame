using System;
using UnityEngine;
using UnityEngine.InputSystem;
using VContainer.Unity;

namespace Input
{
    public class InputManager : IInitializable, IDisposable
    {
        public event Action OnPause;
        public event Action<Vector2> OnMove;
        public event Action OnInteract;
        
        public event Action OnUnpause;
        public event Action OnRestart;
        
        public event Action OnSwitchDebugMode;

        private InputActionMap _playerMap;
        private InputActionMap _uiMap;
        private InputActionMap _gameEndMap;
        
        public void Initialize()
        {
            InputSystem.actions.Disable();
            
            _playerMap = InputSystem.actions.FindActionMap("Player");
            _playerMap.FindAction("Pause").performed += _ => OnPause?.Invoke();
            _playerMap.FindAction("Move").performed += context => OnMove?.Invoke(context.ReadValue<Vector2>().normalized);
            _playerMap.FindAction("Move").canceled += _ => OnMove?.Invoke(Vector2.zero);
            _playerMap.FindAction("Interact").performed += _ => OnInteract?.Invoke();
            _playerMap.FindAction("Debug").performed += _ => OnSwitchDebugMode?.Invoke();

            _uiMap = InputSystem.actions.FindActionMap("UI");
            _uiMap.FindAction("Unpause").performed += _ => OnUnpause?.Invoke();
            _uiMap.FindAction("Debug").performed += _ => OnSwitchDebugMode?.Invoke();
            _uiMap.FindAction("Restart").performed += _ => OnRestart?.Invoke();
            
            _gameEndMap = InputSystem.actions.FindActionMap("End");
            _gameEndMap.FindAction("Restart").performed += _ => OnRestart?.Invoke();
            _gameEndMap.FindAction("Debug").performed += _ => OnSwitchDebugMode?.Invoke();
            
            SwitchToPlayerMap();
        }

        public void Dispose()
        {
            _playerMap?.Dispose();
            _uiMap?.Dispose();
            _gameEndMap.Dispose();
            
            OnPause = null;
            OnMove = null;
            OnInteract = null;
            OnUnpause = null;
            OnRestart = null;
            OnSwitchDebugMode = null;
        }
        
        public void SwitchToUIMap()
        {
            _playerMap.Disable();
            _uiMap.Enable();
        }
        
        public void SwitchToPlayerMap()
        {
            _uiMap.Disable();
            _playerMap.Enable();
        }

        public void SwitchToGameEndMap()
        {
            _playerMap.Disable();
            _gameEndMap.Enable();
        }
    }
}