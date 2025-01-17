using System;
using System.Collections;
using System.Collections.Generic;
using Assets.Common.Scripts;
using UnityEngine;
using Assets.Winter_Level.Scripts;

namespace Assets.Winter_Level.Scripts
{
    public class Slime : MonoBehaviour, IEnemyController
    {
        // Thông số tĩnh của Slime
        public int Attack { get; private set; } = 30;
        public int Defence { get; private set; } = 30;
        public float Speed { get; private set; } = 7f;
        public int HP { get; private set; } = 180;

        private int currentHP;

        Rigidbody2D rigidbody2d;
        bool broken = true;
        private Flash flash;
        [SerializeField] private GameObject deathVFXPrefab;

        EnemyUIHealthBar enemyUIHealthBar;


        private void Awake()
        {
            flash = GetComponent<Flash>();
            currentHP = HP;
            enemyUIHealthBar = GetComponentInChildren<EnemyUIHealthBar>();
        }

        void Start()
        {
            rigidbody2d = GetComponent<Rigidbody2D>();
        }
        void Update()
        {
            if (!broken)
            {
                return;
            }
        }
        private void OnCollisionEnter2D(Collision2D other)
        {
            if (other.collider.tag == "Player")
            {
                attack(other.gameObject, Attack);
            }
        }

        public int beAttacked(int atk)
        {
            // Công thức tính damage
            int damage = Mathf.RoundToInt(atk * (10f / (10f + Mathf.Sqrt(Defence))));
            currentHP -= damage;

            // Bảo vệ giá trị HP không âm
            currentHP = Math.Max(currentHP, 0);
            StartCoroutine(flash.FlashRoutine());
            enemyUIHealthBar.SetValue(currentHP / (float)HP);
            Debug.Log($"Slime nhận {damage} sát thương! HP còn lại: {currentHP}");
            return damage;
        }

        public int attack(GameObject player, int atk)
        {
            //Debug.Log("Shardsoul attacking");
            IPlayerController playerController = player.GetComponent<IPlayerController>();
            if (playerController != null)
            {
                Debug.Log("Enemy attacking player");
                return playerController.beAttacked(null, atk);
            }
            return 0;
        }

        public void DetectDeath()
        {
            if (currentHP <= 0)
            {
                Instantiate(deathVFXPrefab, transform.position, Quaternion.identity);
                Destroy(gameObject);
            }
        }

        public void Fix()
        {
            broken = false;
            rigidbody2d.simulated = false;
        }

    }
}