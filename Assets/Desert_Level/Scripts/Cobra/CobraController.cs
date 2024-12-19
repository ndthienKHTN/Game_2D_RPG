using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Desert_Level.Scripts
{
    public class CobraController : MonoBehaviour
    {
        public float speed = 1.5f;
        public bool vertical;
        Rigidbody2D rigidbody2d;

        public float changeTime = 5.0f;
        float timer;
        int direction = 1;

        Animator animator;

        bool attacking = false;
        bool beAttacked = false;

        public int atk;
        public int level;
        public int exp;
        public int def;

        // Start is called before the first frame update
        void Start()
        {
            rigidbody2d = GetComponent<Rigidbody2D>();
            animator = GetComponent<Animator>();
            timer = changeTime;

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
                //Debug.Log("Direction: " + direction);
                animator.SetFloat("MoveX", direction);
                animator.SetFloat("MoveY", 0);
            }
        }

        private void FixedUpdate()
        {

            RaycastHit2D hit = Physics2D.Raycast(rigidbody2d.position + Vector2.up * 0.2f, new Vector2(0, direction), 1.5f);

            if (hit.collider != null)
            {
                Debug.Log("Collision with: " + hit.collider.tag);
                if (hit.collider.tag == "Player")
                {

                    //animator.Play("Cobra_Attack");
                    animator.SetBool("Attack", true);
                }
                else
                {
                    animator.SetBool("Attack", false);
                    //animator.ResetTrigger("Attack");
                }

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
                // player = other.gameObject.GetComponent<PlayerController>();
                //player.beAttacked();
            }
        }

        private void Attack()
        {

        }

        private void BeAttacked()
        {

        }
    }

}