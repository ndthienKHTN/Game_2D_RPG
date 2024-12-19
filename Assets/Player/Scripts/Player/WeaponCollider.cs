using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Assets.Common.Scripts;

public class WeaponCollider: MonoBehaviour, IPlayerController
{
    [SerializeField] private int atkAmount = 50;

    public int attack(GameObject enemy, int atk)
    {
        IEnemyController _enemy = enemy.GetComponent<IEnemyController>();
        if (_enemy != null)
        {
            _enemy.beAttacked(atk);
        }
        return 0;
    }

    public int beAttacked(GameObject enemy, int atk)
    {
        return 0;
    }

    private void OnTriggerEnter2D(Collider2D other) {
        if (other != null) {
            attack(other.gameObject, atkAmount);
        }
    }
}

