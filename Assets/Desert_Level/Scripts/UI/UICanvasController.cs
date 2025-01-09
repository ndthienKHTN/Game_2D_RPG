using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Assets.Desert_Level.Scripts;
namespace Assets.Desert_Level.Scripts
{
    public class UICanvasController : Singleton<UICanvasController>
    {
        public static bool gameIsPaused = false;
        public GameObject inventoryWindow;
        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                gameIsPaused = !gameIsPaused;
                PauseGame();
            }
        }

        public void PauseGame()
        {
            if (gameIsPaused)
            {
                Time.timeScale = 0f;
                DisplayDialog();

            }
            else
            {
                Time.timeScale = 1;
            }
        }

        public void DisplayDialog()
        {
            inventoryWindow.SetActive(true);
        }
    }
}