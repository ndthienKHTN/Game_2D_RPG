﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using Assets.Common.Scripts;
using Assets.Player.Scripts;
using Assets.Winter_Level.Scripts;
//using Assets.Desert_Level.Scripts;
using TMPro;
namespace Assets.Player.Scripts
{
    // public class PlayerController : MonoBehaviour, IPlayerController, ICheckpoint, IPlayerStatController
    public class PlayerController : Singleton<PlayerController>, IPlayerController, ICheckpoint, IPlayerStatController
    {
        //public bool FacingLeft { get { return facingLeft; } set { facingLeft = value; } }
        // public static PlayerController Instance;

        private bool isMovementDisabled = false;
        [SerializeField] private float moveSpeed = 4f;
        [SerializeField] private float speedBoostMultiplier = 1.5f;
        [SerializeField] private float speedBoostDuration = 5f;
        [SerializeField] private float speedBoostCooldown = 10f;
        [SerializeField] private float teleportDistance = 5f;
        [SerializeField] private float teleportCooldown = 10f;
        [SerializeField] private float dashSpeed = 10f;
        [SerializeField] private TrailRenderer myTrailRenderer;
        [SerializeField] private Transform weaponCollider;

        private PlayerControls playerControls;
        private Vector2 movement;
        private Rigidbody2D rb;
        private Animator myAnimator;
        private SpriteRenderer mySpriteRender;

        private bool facingLeft = false;
        private bool isDashing = false;
        public bool FacingLeft { get { return facingLeft; } set { facingLeft = value; } }
        AudioSource audioSource;

        private Vector3 checkpointPosition;

        private bool isSpeedBoostActive = false;
        private bool isSpeedBoostOnCooldown = false;
        private float speedBoostTimer = 0f;
        private float speedBoostCooldownTimer = 0f;

        private bool isTeleportOnCooldown = false;
        private float teleportCooldownTimer = 0f;

        Vector2 lookDirection;
        private float horizontal;
        private float vertical;

        public int goldCounter = 0;
        public TextMeshProUGUI goldText;

        public int defense = 0;

        //-------------------------Other-Stats--------------------------
        public int Level = 1;
        private int currentLevel = 1;
        public int Attack => Mathf.CeilToInt(15f * Mathf.Pow(1.05f, Level - 1));
        public int Defence => Mathf.CeilToInt(20f * Mathf.Pow(1.05f, Level - 1));
        public float Speed => 10f * Mathf.Pow(1.05f, Level - 1);
        public float EXP { get; private set; } = 0;
        private float expToNextLevel => 10 * Mathf.Pow(1.5f, Level);

        public TextMeshProUGUI lvText;

        //--------------------------Health--------------------------
        public int maxHealth => Mathf.RoundToInt(100 * Mathf.Pow(1.05f, Level - 1));
        [SerializeField] private float knockBackThrustAmount = 10f;

        [SerializeField] private float damageRecoveryTime = 0.5f;
        public int currentHealth;
        public int health
        {
            get { return currentHealth; }
            set { currentHealth = value; }
        }

        public int maxHP
        {
            get { return maxHealth; }
        }

        private bool canTakeDamage = true;
        private Knockback knockback;
        private Flash flash;

        public Slider healthSlider;
        public Slider expSlider;
        // --health-bar-

        public int currentScene { get; set; } = 1;
        public void UpdateCurrentScene(int newScene)
        {
            currentScene = newScene;
        }

        private void UpdateHealthSlider()
        {
            if (healthSlider == null)
            {
                healthSlider = GameObject.Find("Health Slider").GetComponent<Slider>();
            }

            healthSlider.maxValue = maxHealth;
            healthSlider.value = currentHealth;
        }

        private void UpdateExpSlider()
        {
            if (expSlider == null)
            {
                expSlider = GameObject.Find("EXP Slider")?.GetComponent<Slider>();
                if (expSlider == null) return; // Exit if expSlider is still null
            }

            expSlider.maxValue = expToNextLevel;
            expSlider.value = EXP;
        }


        void Start()
        {
            audioSource = GetComponent<AudioSource>();
            currentHealth = maxHealth;
            checkpointPosition = transform.position;
            UpdateHealthSlider();
            UpdateExpSlider();
            playerControls.Combat.Dash.performed += _ => Dash();
            UpdateLevelText();
        }
        private void Dash()
        {
            if (!isDashing)
            {
                isDashing = true;
                moveSpeed *= dashSpeed;
                myTrailRenderer.emitting = true;
                StartCoroutine(EndDashRoutine());
            }
        }
        private IEnumerator EndDashRoutine()
        {
            float dashTime = 0.2f;
            float dashCD = 0.25f;
            yield return new WaitForSeconds(dashTime);
            moveSpeed /= dashSpeed;
            myTrailRenderer.emitting = false;
            yield return new WaitForSeconds(dashCD);
            isDashing = false;
        }
        public void SetCheckpoint(Vector3 newCheckpointPosition)
        {
            checkpointPosition = newCheckpointPosition;
            Debug.Log("Checkpoint set to: " + checkpointPosition);
        }
        public void Respawn()
        {
            if (currentScene == 5)
            {
                transform.position = new Vector3(-26.71f, -15.9f, 0);
            }
            else
            {
                transform.position = checkpointPosition;
            }
            
            
            Debug.Log("Player respawned at: " + checkpointPosition);
        }



        protected override void Awake()

        {
            base.Awake();
            DontDestroyOnLoad(gameObject);
            playerControls = new PlayerControls();
            rb = GetComponent<Rigidbody2D>();
            myAnimator = GetComponent<Animator>();
            mySpriteRender = GetComponent<SpriteRenderer>();
            //hieu ung khi bi tan cong
            flash = GetComponent<Flash>();
            knockback = GetComponent<Knockback>();

            // Update stats based on the current level
            /*Level = 1;
            UpdateStatsForCurrentLevel();
            currentHealth = maxHealth;*/

            if (PlayerPrefs.HasKey("PlayerHealth"))
            {
                currentHealth = PlayerPrefs.GetInt("PlayerHealth");
            }

            //if (currentHealth == 0)
            //{
            //    currentHealth = maxHealth;
            //}
            //print("Current Health: " + currentHealth);

            if (healthSlider == null)
            {
                healthSlider = GameObject.Find("Health Slider").GetComponent<Slider>();
            }
            // UpdateExpSlider();
            UpdateHealthSlider();
        }


        private void OnEnable()
        {
            playerControls.Enable();
        }


        private void Update()
        {
            PlayerInput();
            HandleSpeedBoost();
            HandleTeleportCooldown();
            AdjustLookDirection();
            CheckLevelUp();
            if (currentLevel != Level)
            {
                UpdateHealthSlider();
                UpdateExpSlider();
                UpdateLevelText();
                Debug.Log($"Level Up: {Level}, HP: {maxHealth}, ATK: {Attack}, DEF: {Defence}, SPD: {Speed}");
                currentLevel = Level;
            }

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

        public Transform GetWeaponCollider()
        {
            return weaponCollider;
        }

        private void PlayerInput()
        {
            movement = playerControls.Movement.Move.ReadValue<Vector2>();

            if (Keyboard.current.spaceKey.wasPressedThisFrame)
            {
                ActivateSpeedBoost();
            }

            if (Keyboard.current.tKey.wasPressedThisFrame) // Assuming 'T' key for teleportation
            {
                Teleport();
            }

            myAnimator.SetFloat("moveX", movement.x);
            myAnimator.SetFloat("moveY", movement.y);
        }

        private void Move()
        {
            if (knockback != null && knockback.GettingKnockedBack) { return; }
            float currentMoveSpeed = isSpeedBoostActive ? moveSpeed * speedBoostMultiplier : moveSpeed;
            rb.MovePosition(rb.position + movement * (currentMoveSpeed * Time.fixedDeltaTime));
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
                INPCController npc = hit.collider.GetComponent<INPCController>();
                //Debug.Log("NPC: " + npc);
                if (npc != null)
                {
                    npc.DisplayDialog();
                }
            }
            else
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

        public int beAttacked(GameObject enemy, int atk)
        {
            if (canTakeDamage)
            {
                // New damage calculation formula
                int damage = Mathf.RoundToInt(atk * (38f / (35f + Mathf.Sqrt(Defence))));
                ChangeHealth(-damage);

                if (enemy == null)
                {
                    knockback.GetKnockedBack(this.gameObject.transform, knockBackThrustAmount);
                }
                else
                {
                    IEnemyController Ienemy = enemy.GetComponent<IEnemyController>();
                    knockback.GetKnockedBack(enemy.gameObject.transform, knockBackThrustAmount);
                }

                // StartCoroutine(flash.FlashRoutine());
            }
            return 0;
        }

        public void ActivateSpeedBoost()
        {
            if (!isSpeedBoostOnCooldown)
            {
                isSpeedBoostActive = true;
                isSpeedBoostOnCooldown = true;
                speedBoostTimer = speedBoostDuration;
                speedBoostCooldownTimer = speedBoostCooldown;
                Debug.Log("Speed boost activated!");
            }
        }

        private void HandleSpeedBoost()
        {
            if (isSpeedBoostActive)
            {
                speedBoostTimer -= Time.deltaTime;
                if (speedBoostTimer <= 0)
                {
                    isSpeedBoostActive = false;
                    Debug.Log("Speed boost ended.");
                }
            }

            if (isSpeedBoostOnCooldown)
            {
                speedBoostCooldownTimer -= Time.deltaTime;
                if (speedBoostCooldownTimer <= 0)
                {
                    isSpeedBoostOnCooldown = false;
                    Debug.Log("Speed boost cooldown ended.");
                }
            }
        }

        public void Teleport()
        {
            if (!isTeleportOnCooldown)
            {
                Vector2 teleportDirection = facingLeft ? Vector2.left : Vector2.right;
                RaycastHit2D hit = Physics2D.Raycast(transform.position, teleportDirection, teleportDistance);

                Vector3 targetPosition;
                if (hit.collider != null)
                {
                    // If an obstacle is detected, teleport right up to the obstacle
                    targetPosition = hit.point - teleportDirection * 0.1f;
                    Debug.Log("Obstacle detected. Teleporting to: " + targetPosition);
                }
                else
                {
                    // If no obstacle is detected, teleport the full distance
                    targetPosition = (Vector2)transform.position + teleportDirection * teleportDistance;
                    Debug.Log("No obstacle detected. Teleporting to: " + targetPosition);
                }

                Debug.Log("Current position: " + transform.position);
                transform.position = targetPosition;
                Debug.Log("New position: " + transform.position);

                isTeleportOnCooldown = true;
                teleportCooldownTimer = teleportCooldown;
                Debug.Log("Teleport cooldown started.");
            }
        }

        //public void PlaySound(AudioClip clip)
        //{
        //    audioSource.PlayOneShot(clip);
        //}

        private void HandleTeleportCooldown()
        {
            if (isTeleportOnCooldown)
            {
                teleportCooldownTimer -= Time.deltaTime;
                if (teleportCooldownTimer <= 0)
                {
                    isTeleportOnCooldown = false;
                    Debug.Log("Teleport cooldown ended.");
                }
            }
        }

        public void AddGold(int amount)
        {

            goldCounter += amount;
            //goldText.text = goldCounter.ToString();
            goldText.SetText(goldCounter.ToString());
            Debug.Log("Gold: " + goldCounter);
        }

        private IEnumerator DamageRecoveryRoutine()
        {
            yield return new WaitForSeconds(damageRecoveryTime);
            canTakeDamage = true;
        }

        public void ChangeHealth(int amount)
        {
            if (amount < 0)
            {
                if (!canTakeDamage) return;

                canTakeDamage = false;
                StartCoroutine(DamageRecoveryRoutine());
                if (flash != null)
                {
                    StartCoroutine(flash.FlashRoutine());
                }
                //PlaySound(hitSound);
                AudioManager.Instance.PlaySFX("p_hurt");

            }
            currentHealth = Mathf.Clamp(currentHealth + amount, 0, maxHealth);
            Debug.Log(currentHealth + "/" + maxHealth);
            if (currentHealth <= 0)
            {
                Respawn();
                currentHealth = maxHealth;
            }
            UpdateHealthSlider();
        }

        private void CheckLevelUp()
        {
            if (EXP >= expToNextLevel)
            {
                // LevelUp();
                EXP -= expToNextLevel;
                Level++;
                currentHealth = maxHealth;
                UpdateHealthSlider();
                UpdateExpSlider();
                UpdateLevelText();
                Debug.Log($"Level Up: {Level}, HP: {maxHealth}, ATK: {Attack}, DEF: {Defence}, SPD: {Speed}");
                currentLevel = Level;
            }
        }


        public void GainEXP(float amount)
        {
            EXP += amount;
            Debug.Log($"Gained {amount} EXP. Current EXP: {EXP}/{expToNextLevel}");
            UpdateExpSlider();
        }



        private void UpdateLevelText()
        {
            if (lvText == null)
            {
                lvText = GameObject.Find("LVCounter")?.GetComponent<TextMeshProUGUI>();
                if (lvText == null) return; // Exit if lvText is still null
            }
            lvText.SetText($"{Level}");
        }

        public int increaseHealth(int increasedHealth)
        {
            ChangeHealth(increasedHealth);
            return 0;
        }

        public int increaseDefense(int value)
        {
            defense += value;
            return 0;
        }



        public void DisableMovement(float duration)
        {
            if (isMovementDisabled) return;

            isMovementDisabled = true;

            StartCoroutine(EnableMovementAfterDelay(duration));
        }

        private IEnumerator EnableMovementAfterDelay(float delay)
        {
            // Vô hiệu hóa di chuyển
            float currentSpeed = moveSpeed;
            moveSpeed = 0f;

            // Đợi
            yield return new WaitForSeconds(delay);

            // Kích hoạt lại di chuyển
            moveSpeed = currentSpeed;
            isMovementDisabled = false;
        }

    }

}
