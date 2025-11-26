using UnityEngine;
using VContainer;
using Winning;

namespace UI
{
    public class WinScreen : MonoBehaviour
    {
        [SerializeField] private GameObject menu;
        
        [Inject] private WinController _winController;

        private void Start()
        {
            _winController.OnWin += ShowMenu;
        }

        private void OnDestroy()
        {
            _winController.OnWin -= ShowMenu;
        }

        private void ShowMenu()
        {
            menu.SetActive(true);
        }
    }
}