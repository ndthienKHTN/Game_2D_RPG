using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Assets.Common.Scripts;
public class WeaponCollider: MonoBehaviour, IPlayerController
{
    [SerializeField] private int atkAmount = 1;

    public int attack(GameObject enemy, int atk)
    {
        /*EnemyControllerLong controller = enemy.GetComponent<EnemyControllerLong>();
        if (controller != null)
        {
            controller.beAttacked(atk);
        }
        return 0;*/

        IEnemyController controller = enemy.GetComponent<IEnemyController>();
        if (controller != null)
        {
            return controller.beAttacked(atk);
        }
        return 0;
    }

    public int beAttacked(int atk)
    {
        return 0;
    }

    private void OnTriggerEnter2D(Collider2D other) {
        /*if (other.gameObject.GetComponent<EnemyControllerLong>()) {
            attack(other.gameObject, atkAmount);
        }*/
        if(other.gameObject != null)
        {
            attack(other.gameObject, atkAmount);
        }
        
        /*if (other.gameObject.GetComponent<IEnemyController>()) {
           
        }*/
    }
}

