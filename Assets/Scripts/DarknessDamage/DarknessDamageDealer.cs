using System;
using LampFlame;
using Player.Health;
using UnityEngine;
using VContainer.Unity;

namespace DarknessDamage
{
    public class DarknessDamageDealer : IFixedTickable, IDisposable
    {
        public int DamageAmount { get; private set; }
        public float DamageDealInterval { get; private set; }
        public float SecondsToNextDamage { get; private set; }
        
        private bool _isFlameBurning = true;
        
        private readonly DarknessDamageConfig _config;
        private readonly LampFlamePower _flame;
        private readonly PlayerHealthCounter _playerHealthCounter;
        
        private Action _onExtinguishedHandler;
        private Action _onLitHandler;
        
        public DarknessDamageDealer(DarknessDamageConfig config, LampFlamePower flame, PlayerHealthCounter playerHealthCounter,
        DebugDarknessDamageGUI debugDarknessDamageGUI)
        {
            _config = config;
            _flame = flame;
            _playerHealthCounter = playerHealthCounter;
            
            Init();
            Subscribe();
            
            debugDarknessDamageGUI.SetDealer(this);
        }

        public void FixedTick()
        {
            if (_isFlameBurning) return;
            SecondsToNextDamage -= Time.deltaTime;

            if (SecondsToNextDamage > 0) return;
            SecondsToNextDamage = _config.damageDealInterval;

            DealDamage();
        }

        public void DealDamage()
        {
            _playerHealthCounter.Decrease(DamageAmount);
        }
        
        public void Init()
        {
            DamageAmount =  _config.damage;
            DamageDealInterval = _config.damageDealInterval;
        }
        
        private void Subscribe()
        {
            _onExtinguishedHandler = () =>
            {
                _isFlameBurning = false;
                SecondsToNextDamage = DamageDealInterval;
            };
            
            _onLitHandler = () => { 
                _isFlameBurning = true;
                SecondsToNextDamage = 0;
            };
            
            _flame.OnExtinguished += _onExtinguishedHandler;
            _flame.OnLit += _onLitHandler;
        }
        
        public void Dispose()
        {
            if (_flame != null)
            {
                _flame.OnExtinguished -= _onExtinguishedHandler;
                _flame.OnLit -= _onLitHandler;
            }
        }
    }
}