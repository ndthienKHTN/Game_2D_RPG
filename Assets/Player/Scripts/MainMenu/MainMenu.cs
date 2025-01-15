using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
namespace Assets.Player.Scripts 
{
    public class MainMenu : MonoBehaviour
    {
        SavingFile savingFile;
        public void Start()
        {
            savingFile = SavingFile.Instance;
        }
        public void Button_Continue()
        {
            savingFile.loadData();

            SceneManager.LoadScene(savingFile.GameSaveData.currentScene);
        }
        public void Button_Options(){
            
        }

        public void Button_Quit()
        {
            Application.Quit();
        }
    }
}