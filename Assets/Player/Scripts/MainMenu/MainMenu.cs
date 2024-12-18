using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
namespace Assets.Player.Scripts 
{
    public class MainMenu : MonoBehaviour
    {
        // Start is called before the first frame update
        public void Button_Continue()
        {
            SceneManager.LoadScene("DayTime_DemoScene");
        }

        public void Button_Quit()
        {
            Application.Quit();
        }
    }
}