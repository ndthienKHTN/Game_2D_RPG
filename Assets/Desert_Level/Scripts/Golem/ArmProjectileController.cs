using Assets.Common.Scripts;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArmProjectileController : MonoBehaviour
{
    Rigidbody2D rigidbody2d;
    int atk = 1;
    // Start is called before the first frame update
    void Awake()
    {
        rigidbody2d = GetComponent<Rigidbody2D>();
        if (rigidbody2d == null)
        {
            Debug.LogError("Rigidbody2D component not found in awake");
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (transform.position.magnitude > 15.0f)
        {
            Destroy(gameObject);
        }
    }

    public void Launch(Vector2 direction, float force, int atk)
    {
        rigidbody2d.AddForce(direction * force);
        this.atk = atk;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {

        if (collision.collider.tag == "Player")
        {
            attack(collision.gameObject, atk);
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            attack(collision.gameObject, atk);
        }
    }

    public int attack(GameObject player, int atk)
    {
        IPlayerController playerController = player.GetComponent<IPlayerController>();
        if (playerController != null)
        {
            Destroy(gameObject);
            Debug.Log("attacking player");
            return playerController.beAttacked(null, atk);
        }
        return 0;
    }


}
