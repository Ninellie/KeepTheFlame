using System;
using UnityEngine;

namespace Interacting
{
    public class Interactable : MonoBehaviour
    {
        public event Action OnInteractAction;
        
        // Singleton хранилище интерактивного объекта
        private CurrentInteractableHolder _currentInteractable;

        public void InjectDependencies(CurrentInteractableHolder currentInteractableHolder)
        {
            _currentInteractable = currentInteractableHolder;
        }
        
        public void Interact()
        {
            OnInteractAction?.Invoke();
        }
        
        private void OnTriggerEnter2D(Collider2D other)
        {
            if (!other.CompareTag("Player")) return;
            
            _currentInteractable.Set(this);
        }

        private void OnTriggerExit2D(Collider2D other)
        {
            if (!other.CompareTag("Player")) return;
            
            _currentInteractable.Clear();
        }
    }
}