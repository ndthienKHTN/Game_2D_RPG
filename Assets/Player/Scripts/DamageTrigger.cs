using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Assets.Player.Scripts;
public class DamageTrigger : MonoBehaviour
{
    void OnTriggerStay2D(Collider2D other)
    {
        PlayerController player = other.GetComponent<PlayerController>();
        if (player != null)
        {
            player.ChangeHealth(-5);
        }
        // Debug.Log("Collision with DamageZone");
    }
}
