using UnityEngine;
using UnityEngine.InputSystem;

namespace Interacting
{
    public class PlayerInteractor : MonoBehaviour
    {
        public Interactable Interactable { get; set; }
        
        /// <summary>
        /// PlayerInput Interact(CallbackContext context) на префабе игрока должен вызывать данный метод
        /// </summary>
        public void Interact(InputAction.CallbackContext context)
        {
            if (Interactable == null) return;
            Interactable.Interact();
        }
    }
}