using System;
using System.Collections;
using System.Collections.Generic;
using Assets.Common.Scripts;
using Assets.Forest_Level.Scripts;
using UnityEngine;
using static UnityEngine.InputSystem.LowLevel.InputStateHistory;
namespace Assets.Forest_Level.Scripts
{
    public class EnemyOrc4 : MonoBehaviour, IEnemyController
    {
        public int Attack { get; private set; } = 20;
        public int Defence { get; private set; } = 20;
        public float Speed { get; private set; } = 3f;
        public int HP { get; private set; } = 150;

        public int Gold { get; private set; } = 20;
        public int Exp { get; private set; } = 6;

        public bool vertical;
        Rigidbody2D rigidbody2d;
        int direction = 1;
        float movingTimer;
        public float movingTime = 2.0f;
        Animator animator;
        bool broken = true;
        private Flash flash;
        public GameObject reward;
        [SerializeField] private GameObject deathVFXPrefab;

        public int currentHP;
        EnemyUIHealthBar enemyUIHealthBar;

        private void Awake()
        {
            flash = GetComponent<Flash>();
        }
        void Start()
        {
            rigidbody2d = GetComponent<Rigidbody2D>();
            animator = GetComponent<Animator>();
            movingTimer = movingTime;
            currentHP = HP;
            enemyUIHealthBar = GetComponentInChildren<EnemyUIHealthBar>();
        }
        void Update()
        {
            if (!broken)
            {
                return;
            }
            movingTimer -= Time.deltaTime;
            if (movingTimer < 0)
            {
                direction = -direction;
                movingTimer = movingTime;
            }
            if (vertical)
            {
                animator.SetFloat("Move X", 0);
                animator.SetFloat("Move Y", direction);
            }
            else
            {
                animator.SetFloat("Move X", direction);
                animator.SetFloat("Move Y", 0);
            }
        }
        private void FixedUpdate()
        {
            Vector2 position = rigidbody2d.position;
            if (vertical)
            {
                position.y = position.y + Speed * Time.deltaTime * direction;
            }
            else
            {
                position.x = position.x + Speed * Time.deltaTime * direction;
            }
            rigidbody2d.MovePosition(position);
        }
        private void OnCollisionEnter2D(Collision2D other)
        {
            int damage = Mathf.RoundToInt(Attack * (15f / (10f + Mathf.Sqrt(Defence))));
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
            float randomFactor = UnityEngine.Random.Range(0.8f, 1.2f);
            int damage = Mathf.RoundToInt((atk * atk / (atk + Defence)) * randomFactor);
            currentHP -= damage;
            currentHP = Math.Max(currentHP, 0);
            enemyUIHealthBar.SetValue(currentHP / (float)HP);
            if (flash != null)
            {
                StartCoroutine(flash.FlashRoutine());
            }
            Debug.Log($"Orc 1 nhận {damage} sát thương! HP còn lại: {HP}");
            return damage;
        }

        public void DetectDeath()
        {
            if (currentHP <= 0)
            {
                if (reward != null)
                {
                    GameObject rewardClone = Instantiate(reward, transform.position, Quaternion.identity);
                    GoldCollectable rewardCollected = rewardClone.GetComponent<GoldCollectable>();
                    if (rewardCollected != null)
                    {
                        rewardCollected.goldValue = Gold;
                    }
                    else
                    {
                        Debug.LogWarning("GoldCollectable component not found on rewardClone.");
                    }
                }
                else
                {
                    Debug.LogWarning("Reward GameObject is not assigned.");
                }
                Instantiate(deathVFXPrefab, transform.position, Quaternion.identity);
                Destroy(gameObject);
            }
        }
    }

}

