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

        private void Start()
        {
            popup.SetActive(false);
            text.color = new Color(1f, 1f, 1f, 0f);
            popup.transform.localPosition = animationStartPosition.localPosition; 
        }
        
        private void OnTriggerEnter2D(Collider2D other)
        {
            if (!other.CompareTag("Player")) return;
            popup.SetActive(true);
            text.DOFade(1, openingDuration);
            popup.transform.DOLocalMoveY(animationEndPosition.localPosition.y, openingDuration);
        }

        private void OnTriggerExit2D(Collider2D other)
        {
            if (!other.CompareTag("Player")) return;
            text.DOFade(0, closingDuration);
            popup.transform.DOLocalMoveY(animationStartPosition.localPosition.y, closingDuration)
                .OnComplete( () => popup.SetActive(false));
        }
    }
}