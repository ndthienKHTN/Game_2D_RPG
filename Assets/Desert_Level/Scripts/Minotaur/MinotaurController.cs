using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Desert_Level.Scripts
{
    public class MinotaurController : MonoBehaviour
    {
        public float speed = 1.5f;
        public bool vertical;
        public int health = 10;

        int currentHealth;
        public int maxHealth = 10;

        Rigidbody2D rigidbody2d;
        float timer;
        int direction = 1;

        Animator animator;
        EnemyUIHealthBar enemyUIHealthBar;

        public float changeTime = 3.0f;
        // Start is called before the first frame update
        void Start()
        {
            rigidbody2d = GetComponent<Rigidbody2D>();
            animator = GetComponent<Animator>();
            timer = changeTime;
            enemyUIHealthBar = GetComponentInChildren<EnemyUIHealthBar>();
            currentHealth = health;
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

            RaycastHit2D hit = Physics2D.Raycast(rigidbody2d.position + Vector2.up * 0.2f, new Vector2(0, direction), 1.5f);

            if (hit.collider != null)
            {
                Debug.Log("Collision with: " + hit.collider.tag);
                if (hit.collider.tag == "Player")
                {
                    animator.SetTrigger("Attack");
                }
                else
                {
                    //animator.ResetTrigger("Attack");
                }

            }

            if (Input.GetKeyDown(KeyCode.X))
            {
                int amount = -1;
                currentHealth = Mathf.Clamp(currentHealth + amount, 0, maxHealth);
                //if (currentHealth <= 0)
                //{
                //    PauseController.gameIsPaused = true;
                //    PauseController.instance.PauseGame();
                //}
                Debug.Log(currentHealth + "/" + maxHealth);
                enemyUIHealthBar.SetValue(currentHealth / (float)maxHealth);
            }
        }

        private void FixedUpdate()
        {

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
    }
}