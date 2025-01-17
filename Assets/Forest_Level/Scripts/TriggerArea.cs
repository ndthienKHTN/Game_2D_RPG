using Assets.Forest_Level.Scripts;
using UnityEngine;
public class TriggerArea : MonoBehaviour
{
    [SerializeField]
    public GameObject[] treeEntrances2;

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

            foreach (GameObject treeEntrance2 in treeEntrances2)
            {
                treeEntrance2.SetActive(true);
            }
        }
    }
}