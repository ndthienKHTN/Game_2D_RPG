using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Assets.Common.Scripts;


namespace Assets.Winter_Level.Scripts.Enemy
{
    public class Fire : MonoBehaviour
    {
        [SerializeField] private int attackPower = 10; // Sát thương của viên đạn
        [SerializeField] private GameObject damageZonePrefab;
        private Animator animator;

        private void Awake()
        {
            animator = GetComponent<Animator>();
        }

        private void OnCollisionEnter2D(Collision2D collision)
        {
            if (collision.collider.CompareTag("Player"))
            {
                // Gọi hàm beAttacked của Player
                IPlayerController playerController = collision.collider.GetComponent<IPlayerController>();
                if (playerController != null)
                {
                    playerController.beAttacked(null, attackPower);
                }
            }

            if (animator != null)
            {
                Debug.Log("There have animator");
                // Kiểm tra xem trigger "Hit" có trong animator không
                if (HasTrigger(animator, "Hit"))
                {
                    Debug.Log("There have Hit");
                    animator.SetTrigger("Hit"); // Kích hoạt trigger "Hit"
                    StartCoroutine(DestroyAfterAnimation()); // Đợi animation kết thúc trước khi hủy
                }
                else
                {
                    CreateDamageZone();
                    Destroy(gameObject);
                }
            }
            else
            {
                CreateDamageZone();
                Destroy(gameObject);
            }
        }

        private bool HasTrigger(Animator anim, string triggerName)
        {
            foreach (AnimatorControllerParameter param in anim.parameters)
            {
                if (param.type == AnimatorControllerParameterType.Trigger && param.name == triggerName)
                {
                    return true;
                }
            }
            return false;
        }

        private IEnumerator DestroyAfterAnimation()
        {
            yield return new WaitForSeconds(animator.GetCurrentAnimatorStateInfo(0).length);
            CreateDamageZone();
            Destroy(gameObject);
        }

        private void CreateDamageZone()
        {
            if (damageZonePrefab != null)
            {
                Instantiate(damageZonePrefab, transform.position, Quaternion.identity);
            }
            else
            {
                Debug.LogError("DamageZone Prefab is not assigned!");
            }
        }
    }
}
