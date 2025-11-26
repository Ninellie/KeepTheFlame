using System;
using Input;
using UnityEngine;
using VContainer.Unity;

namespace Winning
{
    public class WinController : ITickable
    {
        public event Action OnWin;
        private const float WinTime = 600;
        private readonly InputManager _input;
        private bool _isGameWon;

        public WinController(InputManager input)
        {
            _input = input;
        }

        public void Tick()
        {
            if (_isGameWon) return;
            if (!(Time.timeSinceLevelLoad > WinTime)) return;
            _isGameWon = true;
            Time.timeScale = 0f;
            AudioListener.pause = true;
            OnWin?.Invoke();
            _input.SwitchToGameEndMap();
        }
    }
}