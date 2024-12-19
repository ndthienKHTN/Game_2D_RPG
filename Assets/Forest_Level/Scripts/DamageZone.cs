using System.Collections;
using System.Collections.Generic;
using Assets.Desert_Level.Scripts;
using UnityEngine;

public class DamageZone : MonoBehaviour
{
    public AudioClip collectSound;
    private void OnTriggerStay2D(Collider2D collision)
    {
        PlayerController player = collision.GetComponent<PlayerController>();
        if (player != null)
        {
            player.ChangeHealth(-20);
            player.PlaySound(collectSound);
        }
    }
    
    //private void OnTriggerEnter2D(Collider2D collision)
    //{
    //    PlayerController player = collision.GetComponent<PlayerController>();
    //    if (player != null)
    //    {
    //        player.ChangeHealth(20);
    //        Destroy(gameObject);
    //        player.PlaySound(collectSound);
    //    }
    //}
}
