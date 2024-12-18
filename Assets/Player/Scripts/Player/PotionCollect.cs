using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Assets.Player.Scripts {
    public class PotionCollect : MonoBehaviour
    {
        //Detect collision trigger with player
        private void  OnTriggerEnter2D(Collider2D collision)
        {
            PlayerController player = collision.GetComponent<PlayerController>();
            if (player != null)
            {
                if (player.health < player.maxHealth)
                {
                    player.ChangeHealth(40);
                    Destroy(gameObject);
                }
            }
            // Debug.Log("Collision with player");
        }
    }
} 
