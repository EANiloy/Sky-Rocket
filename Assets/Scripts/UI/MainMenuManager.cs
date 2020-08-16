using UnityEngine;
using UnityEngine.SceneManagement;

namespace SpaceShip.UI
{
    public class MainMenuManager : MonoBehaviour
    {
        public void LoadSinglePlayerGame()
        {
            if(Time.timeScale <1f)
            {
                Time.timeScale = 1f;
            }
            SceneManager.LoadScene(1);
        }
        public void LoadCoOpPlayerGame()
        {
            if (Time.timeScale < 1f)
            {
                Time.timeScale = 1f;
            }
            SceneManager.LoadScene(2);
        }
        public void ExitGame()
        {
            Application.Quit();
        }
    }

}