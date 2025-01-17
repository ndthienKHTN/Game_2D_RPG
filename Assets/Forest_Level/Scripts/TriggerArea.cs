using Assets.Forest_Level.Scripts;
using UnityEngine;
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
            // Find all game objects with the tag "TreeEntrance" and activate them
            GameObject[] treeEntrances = GameObject.FindGameObjectsWithTag("TreeEntrance");
            foreach (GameObject treeEntrance in treeEntrances)
            {
                treeEntrance.SetActive(true);
            }
        }
    }
}