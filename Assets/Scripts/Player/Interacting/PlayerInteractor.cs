using Input;
using UnityEngine;
using VContainer;

namespace Player.Interacting
{
    public class PlayerInteractor : MonoBehaviour
    {
        public Interactable Interactable { get; set; }

        [Inject] private InputManager _inputManager;
        
        private void Start()
        {
            _inputManager.OnInteract += ()=> Interactable?.Interact();
        }
    }
}