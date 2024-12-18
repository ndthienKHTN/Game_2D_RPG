using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
namespace Assets.Player.Scripts 
{
    public class PauseMenu : MonoBehaviour
    {
        // [SerializeField] GameObject pauseMenu;
                
        public void Button_Pause()
        {
            Time.timeScale = 0;
            gameObject.SetActive(true);
        }
        public void Button_MainMenu()
        {   
            Time.timeScale = 1;
            SceneManager.LoadScene("MainMenu");
            
        }
        public void Button_Continue()
        {
            Time.timeScale = 1;
            gameObject.SetActive(false);
        }

        public void Button_SaveGame()
        {
            Debug.Log("Game Saved");
        }
        public void Button_Options()
        {
            Application.Quit();
        }
    }
}