using DG.Tweening;
using TMPro;
using UnityEngine;

namespace PopupAnimation
{
    /// <summary>
    /// Показывает popup когда подходит игрок 
    /// </summary>
    public class PopupView : MonoBehaviour
    {
        [SerializeField] private GameObject popup;
        [SerializeField] private TMP_Text text;
        [SerializeField] private Transform animationStartPosition;
        [SerializeField] private Transform animationEndPosition;
        [SerializeField] private float openingDuration = 0.75f;
        [SerializeField] private float closingDuration = 0.5f;

        private Tween _openingTweenMove;
        private Tween _openingTweenFade;
        
        private Tween _closingTweenMove;
        private Tween _closingTweenFade;
        
        private void Start()
        {
            popup.SetActive(false);
            text.color = new Color(text.color.r, text.color.g, text.color.b, 0f);
            popup.transform.localPosition = animationStartPosition.localPosition; 
        }
        
        private void OnTriggerEnter2D(Collider2D other)
        {
            if (!other.CompareTag("Player")) return;

            _closingTweenFade?.Kill();
            _closingTweenMove?.Kill();

            popup.SetActive(true);
            _openingTweenFade = text.DOFade(1, openingDuration);
            _openingTweenMove = popup.transform.DOLocalMoveY(animationEndPosition.localPosition.y, openingDuration);
        }

        private void OnTriggerExit2D(Collider2D other)
        {
            if (!other.CompareTag("Player")) return;
            
            _openingTweenFade?.Kill();
            _openingTweenMove?.Kill();
            
            _closingTweenFade = text.DOFade(0, closingDuration);
            _closingTweenMove = popup.transform.DOLocalMoveY(animationStartPosition.localPosition.y, closingDuration)
                .OnComplete(() => popup.SetActive(false));
        }
    }
}