using Assets.Forest_Level.Scripts;
using UnityEngine;
using Assets.Forest_Level.Scripts;
public class TriggerArea : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            EnemyBoss enemyBoss = FindObjectOfType<EnemyBoss>();
            if (enemyBoss != null)
            {
                enemyBoss.StartFollowingPlayer();
            }
        }
    }
}