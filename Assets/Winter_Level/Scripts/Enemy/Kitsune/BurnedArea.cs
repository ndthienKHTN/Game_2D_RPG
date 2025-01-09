using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Assets.Common.Scripts;


namespace Assets.Winter_Level.Scripts.Enemy
{
    public class DamageZone : MonoBehaviour
    {
        [SerializeField] private int damagePerSecond = 10; // Sát thương mỗi giây
        [SerializeField] private float duration = 5f;     // Thời gian tồn tại

        private void Start()
        {
            // Hủy DamageZone sau khi hết thời gian tồn tại
            Destroy(gameObject, duration);
        }

        private void OnTriggerStay2D(Collider2D collision)
        {
            if (collision.gameObject.tag == "Player")
            {
                // Gây sát thương mỗi giây
                IPlayerController playerController = collision.gameObject.GetComponent<IPlayerController>();
                if (playerController != null)
                {
                    Debug.Log("Damage: " + damagePerSecond);
                    playerController.beAttacked(null, damagePerSecond);
                }
            }
        }
    }
}
