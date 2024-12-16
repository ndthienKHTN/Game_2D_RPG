using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Assets.Desert_Level.Scripts;

namespace Assets.Desert_Level.Scripts
{
    public class PlayerController : MonoBehaviour
    {
        [SerializeField] private float speed = 1.0f;

        private PlayerControls playerControls;
        private Vector2 movement;
        private Rigidbody2D rb;

        private void Awake()
        {
            playerControls = new PlayerControls();
            rb = GetComponent<Rigidbody2D>();
        }

        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {
            PlayerInput();
            if (Input.GetKeyDown(KeyCode.X))
            {
                RaycastHit2D hit = Physics2D.Raycast(rb.position + Vector2.up * 0.2f, new Vector2(0,0), 1.5f, LayerMask.GetMask("NPC"));

                if (hit.collider != null)
                {
                    Debug.Log("Raycast has hit the object " + hit.collider.name);
                    NPCController character = hit.collider.GetComponent<NPCController>();

                    if (character != null)
                    {
                        character.DisplayDialog();
                    }

                    //NonPlayerCharacter character = hit.collider.GetComponent<NonPlayerCharacter>();
                    //if (character != null)
                    //{
                    //    character.DisplayDialog();
                    //}
                }
            }
        }

        private void OnEnable()
        {
            playerControls.Enable();
        }

        private void FixedUpdate()
        {
            Move();
        }

        private void PlayerInput()
        {
            movement = playerControls.Movement.Move.ReadValue<Vector2>();

        }

        private void Move()
        {
            rb.MovePosition(rb.position + movement * (speed * Time.fixedDeltaTime));
        }

        internal void ChangeHealth(int v)
        {
            throw new NotImplementedException();
        }
    }

}
