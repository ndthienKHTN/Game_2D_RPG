using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Assets.Player.Scripts
{
    public class Flash : MonoBehaviour
    {
        [SerializeField] private Material whiteFlashMat;
        [SerializeField] private float restoreDefaultMatTime = .2f;

        private Material defaultMat;
        private SpriteRenderer spriteRenderer;
        private EnemyController enemyController;

        private void Awake() {
            enemyController = GetComponent<EnemyController>();
            spriteRenderer = GetComponent<SpriteRenderer>();
            defaultMat = spriteRenderer.material;
        }

        public IEnumerator FlashRoutine() {
            spriteRenderer.material = whiteFlashMat;
            yield return new WaitForSeconds(restoreDefaultMatTime);
            spriteRenderer.material = defaultMat;
            // enemyController.DetectDeath();
        }
    }
}