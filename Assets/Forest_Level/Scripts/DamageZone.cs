using System.Collections;
using System.Collections.Generic;
using Assets.Player.Scripts;
using UnityEngine;

public class DamageZone : MonoBehaviour
{
    public int Damage { get; private set; } = 20;
    public AudioClip collectSound;
    private void OnTriggerStay2D(Collider2D collision)
    {
        PlayerController player = collision.GetComponent<PlayerController>();
        if (player != null)
        {
            player.ChangeHealth(-Damage);
            player.PlaySound(collectSound);
        }
    }
}
