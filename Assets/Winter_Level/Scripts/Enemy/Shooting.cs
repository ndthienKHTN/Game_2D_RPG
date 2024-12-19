using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Winter_Level.Scripts.Enemy
{
    public class Shooting : MonoBehaviour
    {
        [SerializeField] private GameObject bulletPrefab; // Prefab của viên đạn
        [SerializeField] private Transform firePoint;     // Vị trí bắn đạn
        [SerializeField] private float bulletSpeed = 10f; // Tốc độ đạn
        [SerializeField] private float shootCooldown = 2f; // Thời gian chờ giữa các phát bắn

        private bool canShoot = true;

        private void Update()
        {
            // Kiểm tra nếu có thể bắn, bạn có thể thêm điều kiện nếu cần.
            if (canShoot)
            {
                StartCoroutine(Shoot());
            }
        }

        //private IEnumerator Shoot()
        //{
        //    canShoot = false;

        //    // Tạo viên đạn tại firePoint
        //    GameObject bullet = Instantiate(bulletPrefab, firePoint.position, Quaternion.identity);
        //    Rigidbody2D rb = bullet.GetComponent<Rigidbody2D>();

        //    // Đặt hướng bay của viên đạn
        //    Vector2 targetDir = (PlayerPosition() - (Vector2)firePoint.position).normalized;
        //    rb.velocity = targetDir * bulletSpeed;

        //    yield return new WaitForSeconds(shootCooldown);
        //    canShoot = true;
        //}

        private IEnumerator Shoot()
        {
            canShoot = false;

            if (bulletPrefab == null || firePoint == null)
            {
                Debug.LogError("BulletPrefab hoặc FirePoint chưa được gán!");
                yield break;
            }

            // Tính hướng và góc quay của viên đạn
            Vector2 direction = (PlayerPosition() - (Vector2)firePoint.position).normalized;
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

            // Tạo viên đạn và xoay nó
            GameObject bullet = Instantiate(bulletPrefab, firePoint.position, Quaternion.Euler(0, 0, angle));

            // Bỏ qua va chạm giữa đạn và Bat
            Collider2D batCollider = GetComponent<Collider2D>();
            Collider2D bulletCollider = bullet.GetComponent<Collider2D>();
            if (batCollider != null && bulletCollider != null)
            {
                Physics2D.IgnoreCollision(batCollider, bulletCollider);
            }

            // Gán vận tốc cho viên đạn
            Rigidbody2D rb = bullet.GetComponent<Rigidbody2D>();
            if (rb != null)
            {
                rb.velocity = direction * bulletSpeed;
            }

            yield return new WaitForSeconds(shootCooldown);
            canShoot = true;
        }


        private Vector2 PlayerPosition()
        {
            // Tìm đối tượng Player, thêm logic tìm kiếm nếu cần
            GameObject player = GameObject.FindGameObjectWithTag("Player");
            return player != null ? (Vector2)player.transform.position : Vector2.zero;
        }
    }
}
