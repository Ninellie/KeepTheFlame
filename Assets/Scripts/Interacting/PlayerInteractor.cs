using UnityEngine;
using UnityEngine.InputSystem;
using VContainer;

namespace Interacting
{
    public class PlayerInteractor : MonoBehaviour
    {
        // Singleton хранилище интерактивного объекта
        [Inject]
        private CurrentInteractableHolder _currentInteractable;
        
        /// <summary>
        /// PlayerInput Interact(CallbackContext context) на префабе игрока должен вызывать данный метод
        /// </summary>
        public void Interact(InputAction.CallbackContext context)
        {
            _currentInteractable.Interact();
        }
    }
}