using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Assets.Common.Scripts;

namespace Assets.Forest_Level.Scripts
{
    public class Flash : MonoBehaviour
    {
        [SerializeField] private Material whiteFlashMat;
        [SerializeField] private float restoreDefaultMatTime = .1f;

        private Material defaultMat;
        private SpriteRenderer spriteRenderer;
        private IEnemyController enemyController;

        private void Awake()
        {
            enemyController = GetComponent<IEnemyController>();
            spriteRenderer = GetComponent<SpriteRenderer>();
            defaultMat = spriteRenderer.material;
        }

        public IEnumerator FlashRoutine()
        {
            spriteRenderer.material = whiteFlashMat;
            yield return new WaitForSeconds(restoreDefaultMatTime);
            spriteRenderer.material = defaultMat;
            enemyController.DetectDeath();
        }
    }
}