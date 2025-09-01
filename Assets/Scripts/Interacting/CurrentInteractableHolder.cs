namespace Interacting
{
    /// <summary>
    /// Хранит ссылку на текущий интерактивный объект.
    /// </summary>
    public class CurrentInteractableHolder
    {
        private Interactable _current;

        public void Interact()
        {
            if (_current != null) 
                _current.Interact();
        }

        public void Set(Interactable interactable)
        {
            _current = interactable;
        }

        public void Clear()
        {
            _current = null;
        }
    }
}