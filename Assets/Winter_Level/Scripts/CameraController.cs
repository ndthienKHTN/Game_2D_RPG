using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
//using Assets.Winter_Level.Scripts.Player;
//using Assets.Winter_Level.Scripts;
using Assets.Player.Scripts;

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
    }
}