using DG.Tweening;
using UnityEngine;

namespace PopupAnimation
{
    /// <summary>
    /// Показывает popup когда подходит игрок 
    /// </summary>
    public class PopupView : MonoBehaviour
    {
        [SerializeField] private GameObject popup;
        [SerializeField] private Transform animationStartPosition;
        [SerializeField] private Transform animationEndPosition;
        [SerializeField] private float openingDuration = 0.75f;
        [SerializeField] private float closingDuration = 0.5f;

        private void Start()
        {
            popup.SetActive(false);
            popup.transform.localPosition = animationStartPosition.localPosition; 
        }
        
        private void OnTriggerEnter2D(Collider2D other)
        {
            if (!other.CompareTag("Player")) return;
            popup.SetActive(true);
            popup.transform.DOLocalMoveY(animationEndPosition.localPosition.y, openingDuration);
        }

        private void OnTriggerExit2D(Collider2D other)
        {
            if (!other.CompareTag("Player")) return;
            popup.transform.DOLocalMoveY(animationStartPosition.localPosition.y, closingDuration)
                .OnComplete( () => popup.SetActive(false));
        }
    }
}