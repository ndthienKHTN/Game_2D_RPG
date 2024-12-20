using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Assets.Player.Scripts;
public class Checkpoint : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        PlayerController player = collision.GetComponent<PlayerController>();
        if (player != null)
        {
            player.SetCheckpoint(transform.position);
        }
    }
}
