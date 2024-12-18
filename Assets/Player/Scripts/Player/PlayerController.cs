using System.Collections;
using System.Collections.Generic;
using Assets.Common.Scripts;
using UnityEngine;
using UnityEngine.UI;


// public class PlayerController : Singleton<PlayerController>
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
    
    private void UpdateHealthSlider() {
        if (healthSlider == null) {
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
        UpdateHealthSlider();
    }
    
    
    // -----------------------------------------------------


    // protected override void Awake()
    private void Awake() {
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


    private void AdjustPlayerFacingDirection() {
        Vector3 mousePos = Input.mousePosition;
        Vector3 playerScreenPoint = Camera.main.WorldToScreenPoint(transform.position);

        if (mousePos.x < playerScreenPoint.x) {
            mySpriteRender.flipX = true;
            FacingLeft = true;
        } else {
            mySpriteRender.flipX = false;
            FacingLeft = false;
        }
    }

    public int attack(GameObject enemy, int atk)
    {
        //enemy.GetComponent<IEnemyController>().beAttacked(atk);
        return 0;
    }

    public int beAttacked(int atk)
    {
        ChangeHealth(-atk);
        return 0;
    }
}
