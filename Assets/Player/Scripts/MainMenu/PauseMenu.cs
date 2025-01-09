using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
namespace Assets.Player.Scripts
{
    public class PauseMenu : MonoBehaviour
    {
        // [SerializeField] GameObject pauseMenu;
        SavingFile savingFile;
         
        private void Start()
        {
            savingFile = FindObjectOfType<SavingFile>();
            savingFile.loadData();

        }
        public void Button_Pause()
        {
            Time.timeScale = 0;
            gameObject.SetActive(true);
        }
        public void Button_MainMenu()
        {   
            Time.timeScale = 1;
            SceneManager.LoadScene("MainMenu");
            ///gameObject.SetActive(false);

        }
        public void Button_Continue()
        {
            Time.timeScale = 1;
            gameObject.SetActive(false);
        }

        public void Button_SaveGame()
        {
            if (savingFile != null)
            {
                // Gọi hàm SaveGame trong SavingFile
                savingFile.SaveGame();
            }
            else
            {
                Debug.LogError("SavingFile không được tìm thấy trong scene!");
            }
        }
        public void Button_Options()
        {
            Debug.Log("Open Options");
        }
    }
}