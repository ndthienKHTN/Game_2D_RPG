using Assets.Common.Scripts;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Desert_Level.Scripts
{
    public class DemonController : MonoBehaviour, IEnemyController
    {
        public float speed = 1.5f;
        public bool vertical;
        Rigidbody2D rigidbody2d;

        public float changeTime = 10.0f;
        float timer;
        int direction = 1;

        Animator animator;

        public int maxHealth = 10;
        int currentHealth;

        bool attacking = false;

        public int atk;
        public int level;
        public int exp;
        public int def;

        Vector2 lookDirection;

        bool playerInCollision = false;
        EnemyUIHealthBar enemyUIHealthBar;
        public GameObject reward;

        // Start is called before the first frame update
        void Start()
        {
            rigidbody2d = GetComponent<Rigidbody2D>();
            animator = GetComponent<Animator>();
            timer = changeTime;
            currentHealth = maxHealth;
            enemyUIHealthBar = GetComponentInChildren<EnemyUIHealthBar>();

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
                animator.SetFloat("MoveX", 0);
                animator.SetFloat("MoveY", direction);
            }
            else
            {
                animator.SetFloat("MoveX", direction);
                animator.SetFloat("MoveY", 0);
            }

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
            yield return new WaitForSeconds(1f);
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
            IPlayerController playerController = player.GetComponent<IPlayerController>();
            if (playerController != null)
            {
                Debug.Log("Cobra attacking player");
                return playerController.beAttacked(null, atk);
            }
            return 0;
        }

        public int beAttacked(int atk)
        {
            int damage = (int)(atk * (10f / (10f + Mathf.Sqrt(def))));
            currentHealth = Mathf.Clamp(currentHealth - damage, 0, maxHealth);
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
            yield return new WaitForSeconds(1.1f);
            Instantiate(reward, transform.position, Quaternion.identity);
            Destroy(gameObject);
        }

        public void DetectDeath()
        {
            
        }
    }
}