using System.Collections;
using System.Collections.Generic;
using Assets.Forest_Level.Scripts;
using UnityEngine;
public class Collectable : MonoBehaviour
{
    public int HP { get; private set; } = 20;
    public AudioClip collectSound;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        PlayerController player = collision.GetComponent<PlayerController>();
        if (player != null)
        {
            player.ChangeHealth(HP);
            Destroy(gameObject);
            player.PlaySound(collectSound);
        }
    }
}
