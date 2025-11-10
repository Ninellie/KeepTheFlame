using System;
using UnityEngine;

namespace Interacting
{
    public class Interactable : MonoBehaviour
    {
        public event Action OnInteractAction;
        
        public void Interact()
        {
            OnInteractAction?.Invoke();
        }
        
        private void OnTriggerEnter2D(Collider2D other)
        {
            if (!other.CompareTag("Player")) return;
            other.GetComponent<PlayerInteractor>().Interactable = this;
        }

        private void OnTriggerExit2D(Collider2D other)
        {
            if (!other.CompareTag("Player")) return;
            var interactor = other.GetComponent<PlayerInteractor>();
            if (interactor.Interactable != this) return;
            interactor.Interactable = null;
        }
    }
}