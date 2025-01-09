using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Assets.Player.Scripts;
namespace Assets.Desert_Level.Scripts
{
    public class GoldCollectable : MonoBehaviour
    {
        public int goldValue = 100;
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
            //Debug.Log("Collision with: " + other.tag + " - " + other.gameObject.tag);
            if (other.CompareTag("Player"))
            {
                other.GetComponent<PlayerController>().AddGold(goldValue);
                GoldManager.Instance.RemoveGold(gameObject);
                Destroy(gameObject);
            }
        }

        private void OnCollisionEnter2D(Collision2D collision)
        {
            if (collision.gameObject.CompareTag("Player"))
            {
                collision.gameObject.GetComponent<PlayerController>().AddGold(goldValue);
                GoldManager.Instance.RemoveGold(gameObject);
                Destroy(gameObject);
            }
        }
    }

}