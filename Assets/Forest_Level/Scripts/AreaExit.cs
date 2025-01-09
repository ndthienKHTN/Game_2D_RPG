using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Assets.Player.Scripts;

namespace Assets.Forest_Level.Scripts
{
    public class AreaExit : MonoBehaviour
    {
        [SerializeField] private string sceneToLoad;
        [SerializeField] private string sceneTransitionname;
        PlayerController player;
        void Start()
        {
            player = PlayerController.Instance;
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.gameObject.GetComponent<PlayerController>())
            {
                if(player.currentScene == 1)
                    player.currentScene = 2;
                else if (player.currentScene == 2)
                    player.currentScene = 1;
                SceneManager.LoadScene(sceneToLoad);
                SceneManagement.Instance.SetTransitionName(sceneTransitionname);
            }
        }
    }
}