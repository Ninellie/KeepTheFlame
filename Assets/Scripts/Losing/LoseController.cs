using System;
using Input;
using Player.Health;
using UnityEngine;
using VContainer.Unity;

namespace Losing
{
    public class LoseController : IInitializable, IDisposable
    {
        private readonly PlayerHealthCounter _healthCounter;
        private readonly InputManager _input;

        public LoseController(PlayerHealthCounter healthCounter, InputManager input)
        {
            _healthCounter = healthCounter;
            _input = input;
        }

        public void Initialize()
        {
            _healthCounter.OnEmpty += EndGameSession;
        }

        public void Dispose()
        {
            _healthCounter.OnEmpty -= EndGameSession;
        }

        private void EndGameSession()
        {
            Time.timeScale = 0f;
            AudioListener.pause = true;
            _input.SwitchToGameEndMap();
        }
    }
}