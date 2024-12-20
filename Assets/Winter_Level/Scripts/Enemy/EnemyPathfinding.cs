using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Winter_Level.Scripts.Enemy
{
    public class EnemyPathfinding : MonoBehaviour
    {
        [SerializeField] private float moveSpeed = 2f;

        private Rigidbody2D rb;
        private Vector2 moveDir;
        private SpriteRenderer spriteRenderer;


        private void Awake()
        {
            rb = GetComponent<Rigidbody2D>();
            spriteRenderer = GetComponent<SpriteRenderer>();
        }

        private void FixedUpdate()
        {
            rb.MovePosition(rb.position + moveDir * (moveSpeed * Time.fixedDeltaTime));
            AdjustSpriteFacingDirection();
        }

        public void MoveTo(Vector2 targetPosition)
        {
            moveDir = targetPosition;
        }

        private void AdjustSpriteFacingDirection()
        {
            if (moveDir.x < 0)
            {
                spriteRenderer.flipX = false; // Đi sang trái
            }
            else if (moveDir.x > 0)
            {
                spriteRenderer.flipX = true; // Đi sang phải
            }
        }
    }
}