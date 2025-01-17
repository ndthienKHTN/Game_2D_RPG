using Assets.Common.Scripts;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Desert_Level.Scripts
{
    public class GolemController : MonoBehaviour, IEnemyController
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

        public GameObject projectile;

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


        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.gameObject.tag == "Player")
            {
                StartAttackAnimation(collision.gameObject);
            }
        }

        void Launch(GameObject player, int atk)
        {
            GameObject projectileObject = Instantiate(this.projectile, rigidbody2d.position + Vector2.up * 0.5f, Quaternion.identity);
            projectileObject.SetActive(true);
            ArmProjectileController projectile = projectileObject.GetComponent<ArmProjectileController>();
            if (projectile.GetComponent<Rigidbody2D>() == null)
            {
                Debug.LogError("Rigidbody2D component not found when instantiate");
            }

            Vector2 direction = (player.transform.position - transform.position).normalized;
            projectile.Launch(direction, 3000f, atk);

           // animator.SetTrigger("Launch");

        }

        private void StartAttackAnimation(GameObject player)
        {
            playerInCollision = true;
            animator.SetBool("Attack", true);
            StartCoroutine(Attacking(player));
        }

        private void StopAttack()
        {
            playerInCollision = false;
            StartCoroutine(StopAttackAnimation());
        }

        private IEnumerator StopAttackAnimation()
        {
            yield return new WaitForSeconds(1f);
            if (!playerInCollision)
            {
                animator.SetBool("Attack", false);
            }
        }

        private IEnumerator Attacking(GameObject player)
        {
            yield return new WaitForSeconds(1f);
            if (playerInCollision)
            {
                attack(player, atk);
            }
        }

        public int attack(GameObject player, int atk)
        {
            Launch(player,  atk);
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
            yield return new WaitForSeconds(1.1f);
            Instantiate(reward, transform.position, Quaternion.identity);
            Destroy(gameObject);
        }

        public void DetectDeath()
        {

        }
    }
}