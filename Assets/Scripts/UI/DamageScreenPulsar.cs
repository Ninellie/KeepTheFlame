using DG.Tweening;
using Player.Health;
using UnityEngine;
using UnityEngine.UI;
using VContainer.Unity;
using System;

namespace UI
{
    public class DamageScreenPulsar : IStartable, IDisposable
    {
        private const float PulseDuration = 0.3f;
        private const float MaxAlpha = 0.20f;
        
        private readonly PlayerHealthCounter _playerHealthCounter;
        
        private Canvas _canvas;
        private Image _overlayImage;
        private int _previousHealth;
        private Tween _pulseTween;
        
        public DamageScreenPulsar(PlayerHealthCounter playerHealthCounter)
        {
            _playerHealthCounter = playerHealthCounter;
        }
        
        public void Start()
        {
            _canvas = GameObject.FindFirstObjectByType<Canvas>();
            if (_canvas == null)
            {
                Debug.LogError("Canvas not found in scene!");
                return;
            }
            
            CreateOverlay();
            _previousHealth = _playerHealthCounter.Value;
            _playerHealthCounter.OnChanged += OnHealthChanged;
        }
        
        public void Dispose()
        {
            _playerHealthCounter.OnChanged -= OnHealthChanged;
            _pulseTween?.Kill();
        }
        
        private void CreateOverlay()
        {
            var overlayObject = new GameObject("DamageOverlay");
            overlayObject.transform.SetParent(_canvas.transform, false);
            
            var rectTransform = overlayObject.AddComponent<RectTransform>();
            rectTransform.anchorMin = Vector2.zero;
            rectTransform.anchorMax = Vector2.one;
            rectTransform.sizeDelta = Vector2.zero;
            rectTransform.anchoredPosition = Vector2.zero;
            
            _overlayImage = overlayObject.AddComponent<Image>();
            _overlayImage.color = new Color(1.000f, 0.000f, 0.000f, 0.000f);
            _overlayImage.raycastTarget = false;
        }
        
        private void OnHealthChanged(int newHealth)
        {
            if (newHealth < _previousHealth)
            {
                Pulse();
            }
            
            _previousHealth = newHealth;
        }
        
        private void Pulse()
        {
            _pulseTween?.Kill();
            
            SetAlpha(0f);
            
            var fadeInDuration = PulseDuration / 2f;
            var fadeOutDuration = PulseDuration / 2f;
            
            _pulseTween = CreateFadeInTween(fadeInDuration)
                .OnComplete(() => CreateFadeOutTween(fadeOutDuration));
        }
        
        private float GetAlpha()
        {
            return _overlayImage.color.a;
        }
        
        private void SetAlpha(float alpha)
        {
            var color = _overlayImage.color;
            color.a = alpha;
            _overlayImage.color = color;
        }
        
        private Tween CreateFadeInTween(float duration)
        {
            return DOTween.To(GetAlpha, SetAlpha, MaxAlpha, duration)
                .SetEase(DG.Tweening.Ease.OutQuad);
        }
        
        private Tween CreateFadeOutTween(float duration)
        {
            return DOTween.To(GetAlpha, SetAlpha, 0f, duration)
                .SetEase(DG.Tweening.Ease.InQuad);
        }
    }
}