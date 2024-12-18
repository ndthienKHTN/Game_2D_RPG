using System.Collections;
using System.Collections.Generic;
using Assets.Common.Scripts;
using UnityEngine;

namespace Assets.Player.Scripts
{
    public class EnemyController : MonoBehaviour, IEnemyController
    {
        [SerializeField] private int startingHealth = 3;

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
            throw new System.NotImplementedException();
        }

        int IEnemyController.beAttacked(int atk)
        {
            throw new System.NotImplementedException();
        }
    }
}