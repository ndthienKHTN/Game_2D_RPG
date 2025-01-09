using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Assets.Common.Scripts;
using Assets.Player.Scripts; // Add this line to include the correct namespace

public class DamageSource: MonoBehaviour, IPlayerController
{
    private int atkAmount;

    private void Start() {
        MonoBehaviour currenActiveWeapon = ActiveWeapon.Instance.CurrentActiveWeapon;
        atkAmount = (currenActiveWeapon as IWeapon).GetWeaponInfo().weaponDamage;
    }

    public int attack(GameObject enemy, int atk)
    {
        IEnemyController controller = enemy.GetComponent<IEnemyController>();
        if (controller != null)
        {
            // Include the player's attack stat
            int playerAttack = PlayerController.Instance.Attack;
            return controller.beAttacked(atk + playerAttack);
        }
        return 0;
    }

    public int beAttacked(GameObject enemy, int atk)
    {
        return 0;
    }

    private void OnTriggerEnter2D(Collider2D other) {
        if(other.gameObject != null)
        {
            attack(other.gameObject, atkAmount);
        }
    }

     public int increaseHealth(int increasedHealth)
    {
        return 0;
    }

    public void DisableMovement(float duration)
    {
        return;
    }
}

