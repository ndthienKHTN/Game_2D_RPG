using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Assets.Common.Scripts;
using static UnityEngine.InputSystem.LowLevel.InputStateHistory;

namespace Assets.Desert_Level.Scripts
{
    public class MinotaurController : MonoBehaviour, IEnemyController
    {
        public float speed = 1.5f;
        public bool vertical;
        //public int health = 10;

        int currentHealth;
        public int maxHealth = 10;

        Rigidbody2D rigidbody2d;
        float timer;
        int direction = 1;

        Animator animator;
        EnemyUIHealthBar enemyUIHealthBar;

        public float changeTime = 3.0f;

        public int atk;
        public int level;
        public int exp;
        public int def;

        Vector2 lookDirection;

        bool playerInCollision = false;
        public GameObject reward;
        
        public GameObject chickenRewardPrefab;
        public GameObject goldRewardPrefab;

        // Start is called before the first frame update
        void Start()
        {
            rigidbody2d = GetComponent<Rigidbody2D>();
            animator = GetComponent<Animator>();
            timer = changeTime;
            enemyUIHealthBar = GetComponentInChildren<EnemyUIHealthBar>();
            currentHealth = maxHealth;
        }

        // Update is called once per frame
        void Update()
        {
            timer -= Time.deltaTime;
            if (timer < 0)
            {
                direction = -direction;
                timer = changeTime;
            }

            if (vertical)
            {
                animator.SetFloat("moveX", 0);
                animator.SetFloat("moveY", direction);
            }
            else
            {
                animator.SetFloat("moveX", direction);
                animator.SetFloat("moveY", 0);
            }

            /*RaycastHit2D hit = Physics2D.Raycast(rigidbody2d.position + Vector2.up * 0.2f, new Vector2(0, direction), 1.5f);

            if (hit.collider != null)
            {
                //Debug.Log("Collision with: " + hit.collider.tag);
                if (hit.collider.tag == "Player")
                {
                    animator.SetTrigger("Attack");
                }
                else
                {
                    //animator.ResetTrigger("Attack");
                }

            }*/

            if (Input.GetKeyDown(KeyCode.Y))
            {
                int amount = -1;
                currentHealth = Mathf.Clamp(currentHealth + amount, 0, maxHealth);
                if (currentHealth <= 0)
                {
                    StartCoroutine(DieAnimation());
                }
                Debug.Log(currentHealth + "/" + maxHealth);
                enemyUIHealthBar.SetValue(currentHealth / (float)maxHealth);
            }
        }

        private void FixedUpdate()
        {
            if (vertical)
            {
                lookDirection.Set(0, direction);
            }
            else
            {
                lookDirection.Set(direction, 0);
            }

            Vector2 position = rigidbody2d.position;

            if (vertical)
            {
                position.y = position.y + Time.deltaTime * speed * direction;
            }
            else
            {
                position.x = position.x + Time.deltaTime * speed * direction;

            }

            rigidbody2d.MovePosition(position);
        }

        private void OnCollisionEnter2D(Collision2D other)
        {
            if (other.collider.tag == "Player")
            {
                StartAttackAnimation(other.gameObject);
            }
        }

        private void OnCollisionExit2D(Collision2D other)
        {
            if (other.collider.tag == "Player")
            {
                StopAttack();
            }
        }

        private void StartAttackAnimation(GameObject player)
        {
            playerInCollision = true;
            animator.SetBool("Attack", true);
            StartCoroutine(Attacking(player));
        }

        private void BeAttacked()
        {

        }

        private void StopAttack()
        {
            playerInCollision = false;
            StartCoroutine(StopAttackAnimation());
        }

        private IEnumerator Attacking(GameObject player)
        {
            yield return new WaitForSeconds(0.65f);
            if (playerInCollision)
            {
                attack(player, atk);
            }
        }
        private IEnumerator StopAttackAnimation()
        {
            yield return new WaitForSeconds(1f);
            if (!playerInCollision)
            {
                animator.SetBool("Attack", false);
            }
        }

        public int attack(GameObject player, int atk)
        {
            //Debug.Log("Minotaur attacking");
            IPlayerController playerController = player.GetComponent<IPlayerController>();
            if (playerController != null)
            {
                Debug.Log("Minotaur attacking player");
                return playerController.beAttacked(atk);
            }
            return 0;
        }

        public int beAttacked(int atk)
        {
            currentHealth = Mathf.Clamp(currentHealth - atk, 0, maxHealth);
            if (currentHealth <= 0)
            {
                StartCoroutine(DieAnimation());
            }
            Debug.Log(currentHealth + "/" + maxHealth);
            enemyUIHealthBar.SetValue(currentHealth / (float)maxHealth);
            return currentHealth;
        }

        private IEnumerator DieAnimation()
        {
            rigidbody2d.simulated = false;
            animator.SetTrigger("Die");
            yield return new WaitForSeconds(1.00f);
            GenerateReward();
            //Instantiate(reward, transform.position, Quaternion.identity);
            Destroy(gameObject);
        }

        void GenerateReward()
        {
            float randomValue = Random.Range(0f, 1f);

            if (randomValue <= 0.3f)
            {
                // 30% chance to generate chicken reward
                Instantiate(chickenRewardPrefab, transform.position, Quaternion.identity);
            }
            else if (randomValue <= 0.9f)
            {
                // 60% chance to generate gold reward
                Instantiate(goldRewardPrefab, transform.position, Quaternion.identity);
            }
            else
            {
                // 10% chance to generate both rewards
                Instantiate(chickenRewardPrefab, transform.position, Quaternion.identity);
                Instantiate(goldRewardPrefab, transform.position, Quaternion.identity);
            }
        }
    }
}