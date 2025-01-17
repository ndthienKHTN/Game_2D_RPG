using System;
using System.Collections;
using System.Collections.Generic;
using Assets.Common.Scripts;
using UnityEngine;
using Assets.Winter_Level.Scripts;

namespace Assets.Winter_Level.Scripts
{
    public class FrostGuardian : MonoBehaviour, IEnemyController
    {
        [Header("Attributes")]
        [SerializeField] private int Attack = 30;
        [SerializeField] private int Defence = 15;
        [SerializeField] private float Speed = 4f;
        [SerializeField] private int HP = 200;

        private int currentHP;

        // Phạm vi phát hiện và tấn công
        [Header("Detection & Attack")]
        [SerializeField] private float detectionRange = 10f;
        [SerializeField] private float attackRange = 2f;
        [SerializeField] private float attackCooldown = 2f;
        [SerializeField] private GameObject attackEffect;

        private bool isAttacking = false;

        // Thành phần
        Rigidbody2D rb;
        private Animator animator;
        private Transform player;

        EnemyUIHealthBar enemyUIHealthBar;

        // Hiển thị Completed Game
        [SerializeField] private GameObject endGameCanvas;

        private void Awake()
        {
            rb = GetComponent<Rigidbody2D>();
            animator = GetComponent<Animator>();
            currentHP = HP;
            enemyUIHealthBar = GetComponentInChildren<EnemyUIHealthBar>();
        }

        private void Update()
        {
            if (animator.GetBool("Death")) return;

            if (player == null)
            {
                GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
                if (playerObj != null)
                {
                    player = playerObj.transform;
                }
                return;
            }

            float distanceToPlayer = Vector2.Distance(transform.position, player.position);

            if (distanceToPlayer > detectionRange)
            {
                Idle();
            }
            else if (distanceToPlayer > attackRange)
            {
                MoveTowardsPlayer();
            }
            else
            {
                if (!isAttacking)
                {
                    StartCoroutine(AttackPlayer());
                }
            }
        }

        private void Idle()
        {
            animator.SetBool("IsWalking", false);
            rb.velocity = Vector2.zero;
        }

        private void MoveTowardsPlayer()
        {
            animator.SetBool("IsWalking", true);

            Vector2 direction = (player.position - transform.position).normalized;
            rb.velocity = direction * Speed;

            // Lật hướng sprite
            transform.localScale = new Vector3(direction.x < 0 ? 1 : -1, 1, 1);
        }

        private IEnumerator AttackPlayer()
        {
            if (currentHP <= 0)
            {
                yield break;
            }

            isAttacking = true;
            animator.SetTrigger("Attack");
            rb.velocity = Vector2.zero;

            // Đặt vị trí hiện tại của Frost Guardian
            Vector3 attackEffectPosition = transform.position;
            yield return new WaitForSeconds(0.5f);

            if (attackEffect != null)
            {
                GameObject effectInstance = Instantiate(attackEffect, attackEffectPosition, Quaternion.identity);

                // Bật hiệu ứng
                effectInstance.SetActive(true);

                // Gây sát thương nếu trong phạm vi
                float distanceToPlayer = Vector2.Distance(transform.position, player.position);
                if (distanceToPlayer <= attackRange)
                {
                    GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
                    attack(playerObj, Attack);
                }

                yield return new WaitForSeconds(attackCooldown - 0.5f);
                isAttacking = false;

                // Đợi 2 giây rồi tắt hiệu ứng tấn công
                yield return new WaitForSeconds(1f);
                Destroy(effectInstance);
            }
            else
            {
                Debug.LogError("Attack Effect is not assigned in the Inspector!");
                yield return new WaitForSeconds(attackCooldown - 0.5f);
                isAttacking = false;
            }  
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

        public int beAttacked(int atk)
        {
            if (animator.GetBool("Death")) return 0;

            // Tính toán sát thương thực tế
            int actualDamage = Mathf.RoundToInt(atk * (10f / (10f + Mathf.Sqrt(Defence))));
            currentHP -= actualDamage;
            currentHP = Math.Max(currentHP, 0);
            Debug.Log($"Frost Guardian nhận {actualDamage} sát thương! HP còn lại: {currentHP}");
            enemyUIHealthBar.SetValue(currentHP / (float)HP);

            if (currentHP <= 0)
            {
                DetectDeath();
            }
            else
            {
                animator.SetTrigger("TakeHit");
            }
            return actualDamage;
        }

        public void DetectDeath()
        {
            animator.SetTrigger("Death");
            rb.velocity = Vector2.zero;
            //Destroy(gameObject, 1.5f);
        }

        public void OnExitState()
        {
            // Hiển thị UI End Game
            if (endGameCanvas != null)
            {
                endGameCanvas.SetActive(true);
            }
            Destroy(gameObject);
        }


        private void OnDrawGizmosSelected()
        {
            // Vẽ phạm vi phát hiện và tấn công trong Scene
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, attackRange);

            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(transform.position, detectionRange);
        }

    }
}