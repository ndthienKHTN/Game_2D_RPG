using System;
using System.Collections;
using System.Collections.Generic;
using Assets.Common.Scripts;
using Assets.Forest_Level.Scripts;
using Assets.Winter_Level.Scripts;
using UnityEngine;

namespace Assets.Forest_Level.Scripts
{
    public class EnemyBoss : MonoBehaviour, IEnemyController
    {
        public int Attack { get; private set; } = 30;
        public int Defence { get; private set; } = 30;
        public float Speed { get; private set; } = 3f;
        public int HP { get; private set; } = 150;

        public bool vertical;
        Rigidbody2D rigidbody2d;
        int direction = 1;
        float movingTimer;
        public float movingTime = 2.0f;
        Animator animator;
        bool broken = true;
        private Flash flash;
        [SerializeField] private GameObject deathVFXPrefab;
        private Transform playerTransform;
        private bool shouldFollowPlayer = false;

        private void Awake()
        {
            flash = GetComponent<Flash>();
        }

        void Start()
        {
            rigidbody2d = GetComponent<Rigidbody2D>();
            animator = GetComponent<Animator>();
            movingTimer = movingTime;
            playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
        }

        void Update()
        {
            if (!broken || !shouldFollowPlayer)
            {
                return;
            }

            if (playerTransform != null)
            {
                Vector2 direction = (playerTransform.position - transform.position).normalized;
                animator.SetFloat("Move X", direction.x);
                animator.SetFloat("Move Y", direction.y);
            }
        }

        private void FixedUpdate()
        {
            if (playerTransform != null && shouldFollowPlayer)
            {
                Vector2 position = rigidbody2d.position;
                Vector2 direction = (playerTransform.position - transform.position).normalized;
                position += direction * Speed * Time.deltaTime;
                rigidbody2d.MovePosition(position);
            }
        }

        private void OnCollisionEnter2D(Collision2D other)
        {
            int atk = 15; // Example value
            int damage = Mathf.RoundToInt(atk * (15f / (10f + Mathf.Sqrt(Defence))));
            Debug.Log($"Player Hit Damage: {Attack}");
            attack(other.gameObject, Attack);
        }

        public void Fix()
        {
            broken = false;
            rigidbody2d.simulated = false;
            animator.SetTrigger("Fixed");
        }

        public int attack(GameObject player, int atk)
        {
            int attack = Attack;
            IPlayerController playerController = player.GetComponent<IPlayerController>();
            if (playerController != null)
            {
                Debug.Log("Enemy attacking player");
                return playerController.beAttacked(null, atk);
            }
            return 0;
        }

        public int beAttacked(int atk)
        {
            atk = 15; // Example value
            float randomFactor = UnityEngine.Random.Range(0.8f, 1.2f);
            int damage = Mathf.RoundToInt((atk * atk / (atk + Defence)) * randomFactor);
            HP -= damage;

            HP = Math.Max(HP, 0);
            if (flash != null)
            {
                StartCoroutine(flash.FlashRoutine());
            }
            Debug.Log($"Orc 1 nhận {damage} sát thương! HP còn lại: {HP}");
            return damage;
        }

        public void DetectDeath()
        {
            if (HP <= 0)
            {
                Instantiate(deathVFXPrefab, transform.position, Quaternion.identity);
                Destroy(gameObject);
            }
        }

        public void StartFollowingPlayer()
        {
            shouldFollowPlayer = true;
        }
        public void StopFollowingPlayer()
        {
            shouldFollowPlayer = false;
        }
    }
}
