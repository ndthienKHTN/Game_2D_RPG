using Common.Scripts.UI.Model;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Common.Scripts.UI.PickUpSystem
{
    public class Item : MonoBehaviour
    {
        [field: SerializeField]
        public ItemSO InventoryItem { get; private set; }

        [field: SerializeField]
        public int Quantity { get; set; } = 1;

        [field: SerializeField]
        private AudioSource audioSource;

        [field: SerializeField]
        private float duration = 0.3f;



        // Start is called before the first frame update
        void Start()
        {
            GetComponent<SpriteRenderer>().sprite = InventoryItem.ItemImage;
        }

        // Update is called once per frame
        void Update()
        {

        }

        public void DestroyItem()
        {
            GetComponent<Collider2D>().enabled = false;
            StartCoroutine(AnimateItemPickup());
        }

        private IEnumerator AnimateItemPickup()
        {
            audioSource.Play();
            Vector3 startScale = transform.localScale;
            Vector3 endScale = Vector3.zero;
            float currentTime = 0;

            while (currentTime < duration)
            {
                currentTime += Time.deltaTime;
                transform.localScale = 
                    Vector3.Lerp(startScale, endScale, currentTime / duration);
                yield return null;
            }

            transform.localScale = endScale;
            Destroy(gameObject);

        }
    }
}