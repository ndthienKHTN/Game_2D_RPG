using Assets.Common.Scripts;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathSpellController : MonoBehaviour
{
    private bool playerInCollision = false;
    bool isActivated = false;
    public int damage;
    public float initTime = 0.3f;
    public float damageTime = 0.2f;
    public float destructionTime = 0.2f;
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(ChangeActivatedState());
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        IPlayerController playerController = collision.GetComponent<IPlayerController>();

        if (playerController != null)
        {
            //Debug.Log("FireTrap is collision with player");
            playerInCollision = true;
            if (!isActivated)
            {
                return;
            }

            playerController.beAttacked(damage);
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (isActivated && playerInCollision)
        {
            IPlayerController playerController = collision.GetComponent<IPlayerController>();
            if (playerController != null)
            {
                playerController.beAttacked(damage);
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        IPlayerController playerController = collision.GetComponent<IPlayerController>();
        if (playerController != null)
        {
            //Debug.Log("FireTrap is exit collision with player");
            playerInCollision = false;
        }
    }

    private IEnumerator ChangeActivatedState()
    {
        while (true)
        {
            isActivated = false;
            //Debug.Log("FireTrap is unactivated");
            yield return new WaitForSeconds(initTime);

            isActivated = true;
            //Debug.Log("FireTrap is activated");
            yield return new WaitForSeconds(damageTime);

            isActivated = false;
            yield return new WaitForSeconds(destructionTime);

        }
    }
}
