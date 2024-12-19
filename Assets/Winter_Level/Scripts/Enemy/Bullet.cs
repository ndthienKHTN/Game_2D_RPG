using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Assets.Common.Scripts;


namespace Assets.Winter_Level.Scripts.Enemy
{
    public class Bullet : MonoBehaviour
    {
        [SerializeField] private int attackPower = 10; // Sát thương của viên đạn

        private void OnCollisionEnter2D(Collision2D collision)
        {
            if (collision.collider.CompareTag("Player"))
            {
                // Gọi hàm beAttacked của Player
                IPlayerController playerController = collision.collider.GetComponent<IPlayerController>();
                if (playerController != null)
                {
                    playerController.beAttacked(attackPower);
                }
            }

            // Dù trúng gì, viên đạn cũng tự hủy
            Destroy(gameObject);
        }
    }
}
