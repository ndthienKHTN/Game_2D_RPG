using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Assets.Player.Scripts;
namespace Assets.Player.Scripts {
    public class PotionCollect : MonoBehaviour
    {
        //Detect collision trigger with player
        public AudioClip collectSound;
        private void  OnTriggerEnter2D(Collider2D collision)
        {   
            PlayerController player = collision.GetComponent<PlayerController>();
            if (player != null)
            {
                if (player.health < player.maxHealth)
                {
                    player.ChangeHealth(40);
                    Destroy(gameObject);
                    player.PlaySound(collectSound);
                }
            }
            // Debug.Log("Collision with player");
        }
    }
} 
