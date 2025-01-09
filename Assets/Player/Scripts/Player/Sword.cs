using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Assets.Player.Scripts;
using Assets.Common.Scripts;
using Common.Scripts.UI.Model;
public class Sword : MonoBehaviour, IPlayerController, IWeaponSystem
{
    [SerializeField] private GameObject slashAnimPrefab;
    [SerializeField] private Transform slashAnimSpawnPoint;
    [SerializeField] private Transform weaponCollider;
    
    private PlayerControls playerControls;
    private Animator myAnimator;
    private PlayerController playerController;
    private ActiveWeapon activeWeapon;

    private GameObject slashAnim;
    //---------arrow
    [SerializeField] private GameObject arrowPrefab;

    [SerializeField] private EquippableItemSO weaponItem;
    [SerializeField] private InventorySO inventoryData;
    [SerializeField] private List<ItemParameter> parametersToModify;
    [SerializeField] private List<ItemParameter> itemCurrentState;

    private void Awake() {
        playerController = GetComponentInParent<PlayerController>();
        activeWeapon = GetComponentInParent<ActiveWeapon>();
        myAnimator = GetComponent<Animator>();
        playerControls = new PlayerControls();
    }

    private void OnEnable() {
        playerControls.Enable();
    }

    void Start()
    {
        playerControls.Combat.Attack.started += _ => Attack();
        playerControls.Combat.Shoot.started += _ => ShootArrow();
    }

    private void Update() {
        MouseFollowWithOffset();
    }

    private void Attack() {
        //check  animater and weaponCollider
        if (myAnimator == null || weaponCollider == null){
            return;
        }
        myAnimator.SetTrigger("Attack");
        weaponCollider.gameObject.SetActive(true);
        if (slashAnimPrefab != null) {
            slashAnim = Instantiate(slashAnimPrefab, slashAnimSpawnPoint.position, Quaternion.identity);
            slashAnim.transform.parent = this.transform.parent;
        }
    }

    public void DoneAttackingAnimEvent() {
        weaponCollider.gameObject.SetActive(false);
    }


    public void SwingUpFlipAnimEvent() {
        slashAnim.gameObject.transform.rotation = Quaternion.Euler(-180, 0, 0);

        if (playerController.FacingLeft) { 
            slashAnim.GetComponent<SpriteRenderer>().flipX = true;
        }
    }

    public void SwingDownFlipAnimEvent() {
        slashAnim.gameObject.transform.rotation = Quaternion.Euler(0, 0, 0);

        if (playerController.FacingLeft)
        {
            slashAnim.GetComponent<SpriteRenderer>().flipX = true;
        }
    }

    private void MouseFollowWithOffset() {
        Vector3 mousePos = Input.mousePosition;
        Vector3 playerScreenPoint = Camera.main.WorldToScreenPoint(playerController.transform.position);
        if (activeWeapon == null || weaponCollider == null) {
            return;
        }


        float angle = Mathf.Atan2(mousePos.y, mousePos.x) * Mathf.Rad2Deg;

        if (mousePos.x < playerScreenPoint.x) {
            activeWeapon.transform.rotation = Quaternion.Euler(0, -180, angle);
            weaponCollider.transform.rotation = Quaternion.Euler(0, -180, 0);
        } else {
            activeWeapon.transform.rotation = Quaternion.Euler(0, 0, angle);
            weaponCollider.transform.rotation = Quaternion.Euler(0, 0, 0);
        }
    }
    private void ShootArrow() {
        Debug.Log(slashAnimSpawnPoint);
        if (slashAnimSpawnPoint == null) {
            return;
        }
        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector2 direction = (mousePosition - slashAnimSpawnPoint.position).normalized;

        GameObject arrow = Instantiate(arrowPrefab, slashAnimSpawnPoint.position, Quaternion.identity);
        arrow.transform.right = direction;
    }

    public int attack(GameObject enemy, int atk)
    {
        IEnemyController enemyController = enemy.GetComponent<IEnemyController>();
        if (enemyController != null)
        {
            return enemyController.beAttacked(atk);
        }
        return 0;
    }

    public int beAttacked(GameObject enemy, int atk)
    {
        return 0;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        attack(collision.gameObject, 1);
    }

    public int increaseHealth(int increasedHealth)
    {
        return 0;
    }

    public void SetWeapon(EquippableItemSO weaponItemSO, List<ItemParameter> itemState)
    {
        if (weaponItemSO != null)
        {
            inventoryData.AddItem(weaponItemSO, 1, itemCurrentState);
        }

        this.weaponItem = weaponItemSO;
        this.itemCurrentState = new List<ItemParameter>(itemState);
        ModifyParameters();
    }

    private void ModifyParameters()
    {

        foreach (ItemParameter parameter in parametersToModify)
        {
            if (itemCurrentState.Contains(parameter))
            {
                int index = itemCurrentState.IndexOf(parameter);
                int newValue = itemCurrentState[index].value + parameter.value;
                itemCurrentState[index] = new ItemParameter
                {
                    itemParameter = parameter.itemParameter,
                    value = newValue
                };
            }
        }
    }
}
