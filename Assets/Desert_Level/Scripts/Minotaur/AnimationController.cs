using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Desert_Level.Scripts
{
    public class AnimationController : MonoBehaviour
    {
        private Animator animator;
        private SpriteRenderer spriteRenderer;
        private Vector2 movement;

        private void Awake()
        {
            animator = GetComponent<Animator>();
            spriteRenderer = GetComponent<SpriteRenderer>();
        }

        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {
            animator.SetFloat("moveX", movement.x);
            animator.SetFloat("moveY", movement.y);
        }

        private void FixedUpdate()
        {

        }
    }
}