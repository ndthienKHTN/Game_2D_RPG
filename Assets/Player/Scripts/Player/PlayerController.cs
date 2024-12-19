using System.Collections;
using System.Collections.Generic;
using Assets.Common.Scripts;
using Assets.Player.Scripts;
using UnityEngine;
using UnityEngine.UI;



public class PlayerController : Singleton<PlayerController>,IPlayerController
// public class PlayerController : MonoBehaviour, IPlayerController
{
    public bool FacingLeft { get { return facingLeft; } set { facingLeft = value; } }
    public static PlayerController Instance;

    // --------------------------Movement--------------------------
    [SerializeField] private float moveSpeed = 4f;

    private PlayerControls playerControls;
    private Vector2 movement;
    private Rigidbody2D rb;
    private Animator myAnimator;
    private SpriteRenderer mySpriteRender;

    private bool facingLeft = false;

    //--------------------------Health--------------------------
    
    public int maxHealth = 100;
    [SerializeField] private float knockBackThrustAmount = 10f;
    [SerializeField] private float damageRecoveryTime = 1f;

    private int currentHealth;
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

    private Slider healthSlider;
    // --health-bar-
    
    private void UpdateHealthSlider() {
        if (healthSlider == null) {
            healthSlider = GameObject.Find("Health Slider").GetComponent<Slider>();
        }

        healthSlider.maxValue = maxHealth;
        healthSlider.value = currentHealth;
    }

    void Start()
    {
        currentHealth = maxHealth;
        UpdateHealthSlider();
    }  
    
    // -----------------------------------------------------


    protected override void Awake() {
    // private void Awake() {
        base.Awake();
        Instance = this;
        playerControls = new PlayerControls();
        rb = GetComponent<Rigidbody2D>();
        myAnimator = GetComponent<Animator>();
        mySpriteRender = GetComponent<SpriteRenderer>();
        //hieu ung khi bi tan cong
        flash = GetComponent<Flash>();
        knockback = GetComponent<Knockback>();
    }

    private void OnEnable()
    {
        playerControls.Enable();
    }

    private void Update()
    {
        PlayerInput();
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
        if (knockback.GettingKnockedBack) { return; }

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

    public int beAttacked(GameObject enemy,int atk)
    {   
        if ((canTakeDamage)) {
            ChangeHealth(-atk);
            if (enemy == null) {
                knockback.GetKnockedBack(this.gameObject.transform, knockBackThrustAmount);
            } else{
                IEnemyController Ienemy = enemy.GetComponent<IEnemyController>();
                knockback.GetKnockedBack(enemy.gameObject.transform, knockBackThrustAmount);
            }
            // StartCoroutine(flash.FlashRoutine());
            
        }       
        return 0;        
    }

    public void ChangeHealth(int amount)
    {
        if (amount < 0)
        {
            if (!canTakeDamage) return;

            canTakeDamage = false;
            StartCoroutine(DamageRecoveryRoutine());
            StartCoroutine(flash.FlashRoutine());
        }
        currentHealth = Mathf.Clamp(currentHealth + amount, 0, maxHealth);
        Debug.Log(currentHealth + "/" + maxHealth);
        UpdateHealthSlider();
    }
      
    private IEnumerator DamageRecoveryRoutine() {
        yield return new WaitForSeconds(damageRecoveryTime);
        canTakeDamage = true;
    }
}
