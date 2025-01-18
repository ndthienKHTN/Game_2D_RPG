using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
//using Assets.Winter_Level.Scripts.Player;
//using Assets.Winter_Level.Scripts;
using Assets.Player.Scripts;
using UnityEngine.SceneManagement;

namespace Assets.Winter_Level.Scripts
{
    public class CameraController : Singleton<CameraController>
    {
        private CinemachineVirtualCamera cinemachineVirtualCamera;
        public void SetPlayerCameraFollow() //pass player info or load file here
        {
            cinemachineVirtualCamera = FindObjectOfType<CinemachineVirtualCamera>();
            cinemachineVirtualCamera.Follow = PlayerController.Instance.transform;

            //TODO: get health slider in ui canvas and set health of player
        }

        public CinemachineVirtualCamera virtualCamera; // Tham chiếu đến CinemachineVirtualCamera

        void OnEnable()
        {
            SceneManager.sceneLoaded += OnSceneLoaded;
        }

        void OnDisable()
        {
            SceneManager.sceneLoaded -= OnSceneLoaded;
        }

        void OnSceneLoaded(Scene scene, LoadSceneMode mode)
        {
            // Tìm Player sau khi Scene load
            GameObject player = GameObject.FindGameObjectWithTag("Player");
            if (player != null && virtualCamera != null)
            {
                virtualCamera.Follow = player.transform;
            }
            else
            {
                Debug.LogWarning("Player hoặc Virtual Camera không được tìm thấy!");
            }
        }

    }
}