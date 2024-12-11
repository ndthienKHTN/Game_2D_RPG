using System.Collections;
using System.Collections.Generic;
using Assets.Desert_Level.Scripts;
using UnityEngine;

public class Sword : Singleton<Sword>
{
    private PlayerControls playerControls;
    private Animator myAnimator;
    private PlayerController playerController;
    private ActiveWeapon activeWeapon;

    private void Awake() {
        playerController = GetComponentInParent<PlayerController>();
        activeWeapon = GetComponentInParent<ActiveWeapon>();
        myAnimator = GetComponent<Animator>();
        playerControls = new PlayerControls();
    }
    private void OnEnable(){
        playerControls.Enable();
    }
    void Start(){
        playerControls.Combat.Attack.started += _ => Attack();
    }

    private void Update() {
        MouseFollowWithOffset();
    }

    private void Attack(){
        myAnimator.SetTrigger ("Attack");
    }

    private void MouseFollowWithOffset() {
        if (playerController == null ) {
            Debug.LogError("playerController  is not assigned.");
        }
        if ( activeWeapon == null) {
            Debug.LogError(" activeWeapon is not assigned.");
        }
        Vector3 mousePos = Input.mousePosition;
        Vector3 playerScreenPoint = Camera.main.WorldToScreenPoint(playerController.transform.position);

        float angle = Mathf.Atan2(mousePos.y, mousePos.x) * Mathf.Rad2Deg;

        if (mousePos.x < playerScreenPoint.x) {
            activeWeapon.transform.rotation = Quaternion.Euler(0, -180, angle);
        } else {
            activeWeapon.transform.rotation = Quaternion.Euler(0, 0, angle);
        }
    }
}
