using UnityEngine;
using UnityEngine.SceneManagement;

namespace UI
{
    public class MainMenuController : MonoBehaviour
    {
        public void Exit()
        {
            Application.Quit();
        }
        
        public void LoadGameScene()
        {
            SceneManager.LoadScene(sceneBuildIndex: 1);
        }
    }
}