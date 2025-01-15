using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Assets.Winter_Level.Scripts;
using Assets.Player.Scripts;

public class ActiveWeapon : Singleton<ActiveWeapon>
{
    public MonoBehaviour CurrentActiveWeapon { get; private set; }

    private PlayerControls playerControls;
    private float timeBetweenAttacks;

    private bool attackButtonDown, isAttacking = false;

    protected override void Awake() {
        base.Awake();

        playerControls = new PlayerControls();
    }

    private void OnEnable()
    {
        playerControls.Enable();
    }

    private void Start()
    {
        playerControls.Combat.Attack.started += _ => StartAttacking();
        playerControls.Combat.Attack.canceled += _ => StopAttacking();

        AttackCooldown();
    }

    private void Update() {
        Attack();
    }

    public void NewWeapon(MonoBehaviour newWeapon) {
        CurrentActiveWeapon = newWeapon;

        AttackCooldown();
        timeBetweenAttacks = (CurrentActiveWeapon as IWeapon).GetWeaponInfo().weaponCooldown;
    }

    public void WeaponNull() {
        CurrentActiveWeapon = null;
    }


    private IEnumerator TimeBetweenAttacksRoutine() {
        yield return new WaitForSeconds(timeBetweenAttacks);
        isAttacking = false;
    }

    private void AttackCooldown() {
        isAttacking = true;
        StopAllCoroutines();
        StartCoroutine(TimeBetweenAttacksRoutine());
    }

    private void StartAttacking()
    {
        attackButtonDown = true;
    }

    private void StopAttacking()
    {
        attackButtonDown = false;
    }

    private void Attack() {
        if (attackButtonDown && !isAttacking && CurrentActiveWeapon) {
            AttackCooldown();
            (CurrentActiveWeapon as IWeapon).Attack();
        }
    }


    private float CalculateActualCooldown(float baseCooldown) {
        float playerSpeed = PlayerController.Instance.Speed;
        return baseCooldown * (28 / (25 + Mathf.Sqrt(playerSpeed)));
    }
}