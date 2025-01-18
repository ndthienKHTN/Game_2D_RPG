using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using Assets.Player.Scripts;
using UnityEngine.SceneManagement;

namespace Assets.Forest_Level.Scripts
{
    public class CameraController : Singleton<CameraController>
    {
        public CinemachineVirtualCamera cinemachineVirtualCamera;
        public void SetPlayerCameraFollow()
        {
            cinemachineVirtualCamera = FindObjectOfType<CinemachineVirtualCamera>();
            cinemachineVirtualCamera.Follow = PlayerController.Instance.transform;
        }
        //public CinemachineVirtualCamera virtualCamera; // Tham chiếu đến CinemachineVirtualCamera

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
            if (player != null && cinemachineVirtualCamera != null)
            {
                cinemachineVirtualCamera.Follow = player.transform;
            }
            else
            {
                cinemachineVirtualCamera = FindObjectOfType<CinemachineVirtualCamera>();
                cinemachineVirtualCamera.Follow = PlayerController.Instance.transform;
                Debug.LogWarning("Player hoặc Virtual Camera không được tìm thấy!");
            }
        }
    }
}