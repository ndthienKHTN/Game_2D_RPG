using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Assets.Player.Scripts;

namespace Assets.Desert_Level.Scripts
{
    public class HealthPotionController : MonoBehaviour
    {
        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.CompareTag("Player"))
            {
                other.GetComponent<PlayerController>().ChangeHealth(20);
                Destroy(gameObject);
            }
        }
    }
}