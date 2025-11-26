using Player.Health;
using UnityEngine;
using VContainer;

namespace UI
{
    public class LoseScreen : MonoBehaviour
    {
        [SerializeField] private GameObject menu;
        
        [Inject] private PlayerHealthCounter _healthCounter;

        private void Start()
        {
            _healthCounter.OnEmpty += ShowMenu;
        }

        private void OnDestroy()
        {
            _healthCounter.OnEmpty -= ShowMenu;
        }

        private void ShowMenu()
        {
            menu.SetActive(true);
        }
    }
}