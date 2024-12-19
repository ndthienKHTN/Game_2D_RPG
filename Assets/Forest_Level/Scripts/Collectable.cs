using System.Collections;
using System.Collections.Generic;
using Assets.Forest_Level.Scripts;
using UnityEngine;
public class Collectable : MonoBehaviour
{
    public AudioClip collectSound;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        PlayerController player = collision.GetComponent<PlayerController>();
        if (player != null)
        {
            player.ChangeHealth(1);
            Destroy(gameObject);
            player.PlaySound(collectSound);
        }
    }
}
