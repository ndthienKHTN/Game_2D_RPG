using System.Collections;
using System.Collections.Generic;
using UnityEngine;
// namespace Assets.Player.Scripts

public class Knockback : MonoBehaviour
{
    public bool GettingKnockedBack { get; private set; }

    [SerializeField] private float knockBackTime = .2f;

    private Rigidbody2D rb;

    private void Awake() {
        rb = GetComponent<Rigidbody2D>();
    }

    public void GetKnockedBack(Transform weaponCollider, float knockBackThrust) {
        GettingKnockedBack = true;
        Vector2 difference = (transform.position - weaponCollider.position).normalized * knockBackThrust * rb.mass;
        rb.AddForce(difference, ForceMode2D.Impulse); 
        StartCoroutine(KnockRoutine());
    }

    private IEnumerator KnockRoutine() {
        yield return new WaitForSeconds(knockBackTime);
        rb.velocity = Vector2.zero;
        GettingKnockedBack = false;
    }
}
