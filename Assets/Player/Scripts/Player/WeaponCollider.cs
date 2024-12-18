using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponCollider: MonoBehaviour
{
    [SerializeField] private int atkAmount = 1;

    private void OnTriggerEnter2D(Collider2D other) {
        if (other.gameObject.GetComponent<EnemyControllerLong>()) {
            EnemyControllerLong controller = other.gameObject.GetComponent<EnemyControllerLong>();
            controller.beAttacked(atkAmount);
        }
    }
}

