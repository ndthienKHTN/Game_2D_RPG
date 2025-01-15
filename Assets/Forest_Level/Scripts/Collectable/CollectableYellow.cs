using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Assets.Player.Scripts;
using Assets.Forest_Level.Scripts;
public class CollectableYellow : MonoBehaviour
{
    public int HP { get; private set; } = 30;
    public AudioClip collectSound;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        PlayerController player = collision.GetComponent<PlayerController>();
        if (player != null)
        {
            player.ChangeHealth(HP);
            Destroy(gameObject);
            //player.PlaySound(collectSound);
        }
    }
}
