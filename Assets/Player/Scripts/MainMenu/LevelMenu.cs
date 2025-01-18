using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Assets.Player.Scripts 
{
    public class LevelMenu : MonoBehaviour
    {
        public void Button_Level(string Name){
            SceneManager.sceneLoaded += OnSceneLoaded;
            SceneManager.LoadScene(1);
        }
        private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
        {
            PlayerController playerController = PlayerController.Instance;
            if (playerController != null)
            {
                playerController.transform.position = new Vector3(31, 0.8f, 0);
            }
            else
            {
                Debug.LogWarning("PlayerController not found");
            }
            SceneManager.sceneLoaded -= OnSceneLoaded;
        }

    }
}