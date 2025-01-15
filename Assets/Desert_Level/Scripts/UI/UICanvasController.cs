using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Assets.Desert_Level.Scripts;

namespace Assets.Desert_Level.Scripts
{
    public class UICanvasController : Singleton<UICanvasController>
    {
        public static bool gameIsPaused = false;
        public GameObject inventoryWindow;

        public Slider _musicSlider, _sfxSlider;

        public void ToggleMusic()
        {
            AudioManager.Instance.ToggleMusic();
        }

        public void ToggleSFX()
        {
            AudioManager.Instance.ToggleSFX();
        }

        public void MusicVolume()
        {
            AudioManager.Instance.MusicVolume(_musicSlider.value);
        }

        public void SFXVolume()
        {
            AudioManager.Instance.SFXVolume(_sfxSlider.value);
        }

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