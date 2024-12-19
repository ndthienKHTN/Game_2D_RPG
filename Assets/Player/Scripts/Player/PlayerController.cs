using System.Collections;
using System.Collections.Generic;
using Assets.Common.Scripts;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using Assets.Common.Scripts;
using Assets.Player.Scripts;
using Assets.Desert_Level.Scripts;
using TMPro;
// public class PlayerController : Singleton<PlayerController>
namespace Assets.Player.Scripts
{
    public class PlayerController : MonoBehaviour, IPlayerController
    {
        public bool FacingLeft { get { return facingLeft; } set { facingLeft = value; } }
        public static PlayerController Instance;

        [SerializeField] private float moveSpeed = 4f;

        private PlayerControls playerControls;
        private Vector2 movement;
        private Rigidbody2D rb;
        private Animator myAnimator;
        private SpriteRenderer mySpriteRender;

    private bool facingLeft = false;
    AudioSource audioSource;

        Vector2 lookDirection;
        private float horizontal;
        private float vertical;

        public int goldCounter=0;
        public TextMeshProUGUI goldText;
        //--------------------------Health--------------------------

        public int maxHealth = 100;
        int currentHealth;
        public int health
        {
            get { return currentHealth; }
            set { currentHealth = value; }
        }
        public Slider healthSlider;
        // --health-bar-

        private void UpdateHealthSlider()
        {
            /*if (healthSlider == null) {
                healthSlider = GameObject.Find("Health Slider").GetComponent<Slider>();
            }*/

            healthSlider.maxValue = maxHealth;
            healthSlider.value = currentHealth;
        }
        // Timer for invincibility
        public float timeInvincible = 2.0f;
        bool isInvincible;
        float invincibleTimer;
        void Start()
        {
            audioSource = GetComponent<AudioSource>();
            currentHealth = maxHealth;
            UpdateHealthSlider();
        }
        public void ChangeHealth(int amount)
        {
            if (amount < 0)
            {
                if (isInvincible)
                    return;
                isInvincible = true;
                invincibleTimer = timeInvincible;
            }
            currentHealth = Mathf.Clamp(currentHealth + amount, 0, maxHealth);
            Debug.Log(currentHealth + "/" + maxHealth);
            //test
            if (currentHealth<=0)
            {
                currentHealth = 100;
            }
            UpdateHealthSlider();
        }


        // -----------------------------------------------------


        // protected override void Awake()
        private void Awake()
        {
            // base.Awake();
            Instance = this;
            playerControls = new PlayerControls();
            rb = GetComponent<Rigidbody2D>();
            myAnimator = GetComponent<Animator>();
            mySpriteRender = GetComponent<SpriteRenderer>();
        }

        private void OnEnable()
        {
            playerControls.Enable();
        }

        private void Update()
        {
            PlayerInput();
            if (isInvincible)
            {
                invincibleTimer -= Time.deltaTime;
                if (invincibleTimer < 0)
                {
                    isInvincible = false;
                }
            }

            AdjustLookDirection();
            
            if (Input.GetKeyDown(KeyCode.C))
            {
                //Debug.Log("C key was pressed");
                OpenNPCChat();

            }
            
        }

        private void FixedUpdate()
        {
            AdjustPlayerFacingDirection();
            Move();

        }

        private void PlayerInput()
        {
            movement = playerControls.Movement.Move.ReadValue<Vector2>();

            myAnimator.SetFloat("moveX", movement.x);
            myAnimator.SetFloat("moveY", movement.y);
        }

        private void Move()
        {
            rb.MovePosition(rb.position + movement * (moveSpeed * Time.fixedDeltaTime));
        }


        private void AdjustPlayerFacingDirection()
        {
            Vector3 mousePos = Input.mousePosition;
            Vector3 playerScreenPoint = Camera.main.WorldToScreenPoint(transform.position);

            if (mousePos.x < playerScreenPoint.x)
            {
                mySpriteRender.flipX = true;
                FacingLeft = true;
            }
            else
            {
                mySpriteRender.flipX = false;
                FacingLeft = false;
            }
        }

        private void AdjustLookDirection()
        {
            horizontal = Input.GetAxis("Horizontal");
            vertical = Input.GetAxis("Vertical");

            Vector2 move = new Vector2(horizontal, vertical);

            if (!Mathf.Approximately(move.x, 0.0f) || !Mathf.Approximately(move.y, 0.0f))
            {
                lookDirection.Set(move.x, move.y);
                lookDirection.Normalize();
            }
        }

        private void OpenNPCChat()
        {
            Debug.DrawLine(rb.position + Vector2.up * 0.2f,
                rb.position + Vector2.up * 0.2f + lookDirection * 1.5f, Color.green);
            RaycastHit2D hit = Physics2D.Raycast(
                rb.position + Vector2.up * 0.2f, 
                lookDirection, 
                2.5f, 
                LayerMask.GetMask("NPC"));

            if (hit.collider != null)
            {
                Debug.Log("Raycast has hit the object " + hit.collider.name);
                NPCController npc = hit.collider.GetComponent<NPCController>();

                if (npc != null)
                {
                    npc.DisplayDialog();
                }
            } else
            {
                Debug.Log("Raycast has not hit any object");
            }
        }

        public int attack(GameObject enemy, int atk)
        {
            IEnemyController controller = enemy.GetComponent<IEnemyController>();
            if (controller != null)
            {
                return controller.beAttacked(atk);
            }
            return 0;
        }

        public int beAttacked(int atk)
        {
            ChangeHealth(-atk);
            return currentHealth;
        }

        public void AddGold(int amount)
        {

            goldCounter += amount;
            //goldText.text = goldCounter.ToString();
            goldText.SetText(goldCounter.ToString());
            Debug.Log("Gold: " + goldCounter);
        }
        public void PlaySound(AudioClip clip)
        {
            audioSource.PlayOneShot(clip);
        }
    }
}