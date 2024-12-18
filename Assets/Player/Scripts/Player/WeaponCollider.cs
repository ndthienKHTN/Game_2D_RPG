using System.Collections;
using System.Collections.Generic;
using Assets.Common.Scripts;
using UnityEngine;

public class WeaponCollider: MonoBehaviour, IPlayerController
{
    [SerializeField] private int atkAmount = 1;

    public int attack(GameObject enemy, int atk)
    {
        if (enemy.GetComponent<EnemyController>()) {
            EnemyController enemyController = enemy.GetComponent<EnemyController>();
            // enemyController.beAttacked(atk);
        }
        
        return 0;
    }

    public int beAttacked(int atk)
    {
        throw new System.NotImplementedException();
    }

    private void OnTriggerEnter2D(Collider2D other) {
        if (other.gameObject.GetComponent<EnemyController>()) {
            attack(other.gameObject, atkAmount);
        }
    }
}

