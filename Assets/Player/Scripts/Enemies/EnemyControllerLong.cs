using System.Collections;
using System.Collections.Generic;
using Assets.Common.Scripts;
using Unity.VisualScripting;
using UnityEngine;
using Assets.Player.Scripts;

namespace Assets.Player.Scripts
{
    public class SlimeController : MonoBehaviour, IEnemyController
    {
        [SerializeField] private int startingHealth = 100;
        
        private int currentHealth;
        private Knockback knockback;


        private void Awake() {
            knockback = GetComponent<Knockback>();
        }

        private void Start() {
            currentHealth = startingHealth;
        }

        public void beAttacked(int atk) {
            currentHealth -= atk;
            knockback.GetKnockedBack(PlayerController.Instance.transform, 15f);
        }

        public void DetectDeath() {
            if (currentHealth <= 0) {
                Destroy(gameObject);
            }
        }

        public int attack(GameObject player, int atk)
        {
            return 0;
        }

        int IEnemyController.beAttacked(int atk)
        {
            return 0;
        }

        void OnTriggerStay2D(Collider2D other)
        {
            IPlayerController player = other.gameObject.GetComponent<IPlayerController>();
            if (player != null)
            {
                player.beAttacked(this.gameObject,5);
            }
            // Debug.Log("Collision with DamageZone");
        }
    }
}