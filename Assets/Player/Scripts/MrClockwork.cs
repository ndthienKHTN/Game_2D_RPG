using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MrClockwork : MonoBehaviour
{
    public float speed =2.5f;
    public bool hozizontal;
    Rigidbody2D rb2d;

    int direction = -1;
    float movingTimer;
    public float movingTime = 1.5f;

    private Animator enemyAnimator;
    private SpriteRenderer enemySprite;
    public bool isFacingLeft = true;
    
    // Start is called before the first frame update
    void Start()
    {
        rb2d = GetComponent<Rigidbody2D>();    
        movingTimer = movingTime;
        enemyAnimator = GetComponent<Animator>();
        enemySprite = GetComponent<SpriteRenderer>();
    }

    void Update() {
        movingTimer -= Time.deltaTime;
        if (movingTimer < 0) {
            direction = -direction;
            movingTimer = movingTime;
            enemySprite.flipX = !enemySprite.flipX;
            isFacingLeft = !isFacingLeft;
        }
    }
    void FixedUpdate()
    {
        Vector2 pos = rb2d.position;
        if (hozizontal) {
            pos.y += speed * Time.deltaTime * direction;
        } else {
            pos.x += speed * Time.deltaTime *direction;
        }
        rb2d.MovePosition(pos);     
    }
}
