using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//using Assets.Desert_Level.Scripts;
using System.Threading;
using Assets.Player.Scripts;
using Assets.Common.Scripts;

namespace Assets.Desert_Level.Scripts
{
    public class BearTrapController : MonoBehaviour
    {
        Animator animator;
        private bool playerInCollision = false;
        float restartTrapTimer;
        public float restartTrapDelay = 1f;

        // Start is called before the first frame update
        void Start()
        {
            animator = GetComponent<Animator>();
            restartTrapTimer = -1;
        }

        // Update is called once per frame
        void Update()
        {
            if (restartTrapTimer > 0)
            {
                restartTrapTimer -= Time.deltaTime;
            }
        }
        private void OnTriggerStay2D(Collider2D collision)
        {
            if (restartTrapTimer > 0)
            {
                return;
            }

            IPlayerController playerController = collision.GetComponent<IPlayerController>();

            if (playerController != null)
            {
                //playerController.ChangeHealth(-1);
                //animator.SetTrigger("Hit");
                animator.SetBool("Hit", true);
                playerInCollision = true;
                StartCoroutine(CheckPlayerCollisionAfterDelay(playerController));
                StartCoroutine(ResetHitTrigger());
            }
        }
        /*private void OnTriggerEnter2D(Collider2D other)
        {
            //if (restartTrapTimer > 0)
            //{
            //    return;
            //}

            PlayerController playerController = other.GetComponent<PlayerController>();
            if (playerController != null)
            {
                //playerController.ChangeHealth(-1);
                //animator.SetTrigger("Hit");
                animator.SetBool("Hit", true);
                playerInCollision = true;
                StartCoroutine(CheckPlayerCollisionAfterDelay(playerController));
                StartCoroutine(ResetHitTrigger());
            }
        }*/

        private void OnTriggerExit2D(Collider2D other)
        {
            PlayerController playerController = other.GetComponent<PlayerController>();
            if (playerController != null)
            {
                playerInCollision = false;
            }
        }

        private IEnumerator ResetHitTrigger()
        {
            yield return new WaitForSeconds(0.45f);
           // animator.ResetTrigger("Hit");
            animator.SetBool("Hit", false);
            restartTrapTimer = restartTrapDelay;
        }

        private IEnumerator CheckPlayerCollisionAfterDelay(IPlayerController playerController)
        {
            yield return new WaitForSeconds(0.15f);
            if (playerInCollision)
            {
                Debug.Log("Player is still in collision with bear trap");
                playerController.beAttacked(1);
                // Cause damage to the player
                //playerController.ChangeHealth(-1);
            }
        }
    }
}