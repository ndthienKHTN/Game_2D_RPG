using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Assets.Forest_Level.Scripts;
using Assets.Player.Scripts;

namespace Assets.Forest_Level.Scripts
{
    public class AreaEntrance : MonoBehaviour
    {
        [SerializeField] private string transitionName;

        private void Start()
        {
            if (transitionName == SceneManagement.Instance.SceneTransitionName)
            {
                PlayerController.Instance.transform.position = this.transform.position;
                //CameraController.Instance.SetPlayerCameraFollow();
            }
        }
    }
}