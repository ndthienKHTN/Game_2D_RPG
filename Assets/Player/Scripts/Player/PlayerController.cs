using System.Collections;
using System.Collections.Generic;
using Assets.Common.Scripts;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour, IPlayerController, ICheckpoint
{
    
    public static PlayerController Instance;

    [SerializeField] private float moveSpeed = 4f;
    [SerializeField] private float speedBoostMultiplier = 2f;
    [SerializeField] private float speedBoostDuration = 5f;
    [SerializeField] private float speedBoostCooldown = 10f;
    [SerializeField] private float teleportDistance = 5f;
    [SerializeField] private float teleportCooldown = 10f;
    [SerializeField] private float dashSpeed = 10f;
    [SerializeField] private TrailRenderer myTrailRenderer;

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

    //--------------------------Health--------------------------

    public int maxHealth = 100;
    int currentHealth;
    public int health
    {
        get { return currentHealth; }
        set { currentHealth = value; }
    }
    private Slider healthSlider;
    // --health-bar-

    private void UpdateHealthSlider()
    {
        if (healthSlider == null)
        {
            healthSlider = GameObject.Find("Health Slider").GetComponent<Slider>();
        }

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
        checkpointPosition = transform.position;
        UpdateHealthSlider();
        playerControls.Combat.Dash.performed += _ => Dash();
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
        transform.position = checkpointPosition;
        Debug.Log("Player respawned at: " + checkpointPosition);
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
        UpdateHealthSlider();
        if (currentHealth == 0)
        {
            Respawn();
        }
    }

    private void Awake()
    {
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
        HandleSpeedBoost();
        HandleTeleportCooldown();

        if (isInvincible)
        {
            invincibleTimer -= Time.deltaTime;
            if (invincibleTimer < 0)
            {
                isInvincible = false;
            }
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

    public void PlaySound(AudioClip clip)
    {
        audioSource.PlayOneShot(clip);
    }

    public int attack(GameObject enemy, int atk)
    {
        enemy.GetComponent<IEnemyController>().beAttacked(atk);
        return 0;
    }

    public int beAttacked(int atk)
    {
        ChangeHealth(-atk);
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
}
