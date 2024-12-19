using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Assets.Winter_Level.Scripts;
using Assets.Winter_Level.Scripts.Player;

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
                SceneManager.LoadScene(sceneToLoad);
                SceneManagement.Instance.SetTransitionName(sceneTransitionname);
            }
        }
    }
}