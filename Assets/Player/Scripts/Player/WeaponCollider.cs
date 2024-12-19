using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Assets.Common.Scripts;

public class WeaponCollider: MonoBehaviour
{
    [SerializeField] private int atkAmount = 50;

    private void OnTriggerEnter2D(Collider2D other) {
        IEnemyController enemyController = other.GetComponent<IEnemyController>();
        if (enemyController != null)
        {
            Debug.Log("Player attacking Enemy");
            enemyController.beAttacked(atkAmount);
        }
    }
}

