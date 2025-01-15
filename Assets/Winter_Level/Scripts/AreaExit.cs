using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
//using Assets.Winter_Level.Scripts;
//using Assets.Winter_Level.Scripts.Player;
using Assets.Player.Scripts;

namespace Assets.Winter_Level.Scripts
{
    public class AreaExit : MonoBehaviour
    {
        [SerializeField] private string sceneToLoad;
        [SerializeField] private string sceneTransitionname;

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.gameObject.GetComponent<PlayerController>())
            {
                PlayerController playerController = other.gameObject.GetComponent<PlayerController>();
                PlayerPrefs.SetInt("PlayerHealth", playerController.currentHealth);
                //PlayerControls current = other.gameObject.GetComponent<PlayerControls>();
                //current.Movement.Disable();
                //current.Combat.Disable();
                //current.Inventory.Disable();

                SceneManager.LoadScene(sceneToLoad);
                SceneManagement.Instance.SetTransitionName(sceneTransitionname);
            }
        }
    }
}