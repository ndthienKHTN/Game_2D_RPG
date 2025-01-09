using Common.Scripts.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Common.Scripts.UI {
    public class MouseFollower : MonoBehaviour
    {
        [SerializeField]
        private Canvas canvas;

        [SerializeField]
        private UIInventoryItem item;

        public void Awake()
        {
            canvas = transform.root.GetComponent<Canvas>();
            item = GetComponentInChildren<UIInventoryItem>();
        }

        public void SetData(Sprite sprite, int quantity)
        {
            item.SetData(sprite, quantity);
        }

        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {
            Vector2 position;

            RectTransformUtility.ScreenPointToLocalPointInRectangle(
                canvas.transform as RectTransform, 
                Input.mousePosition, 
                canvas.worldCamera, 
                out position
            );

            transform.position = canvas.transform.TransformPoint(position);
        }

        public void Toggle(bool value)
        {
            Debug.Log("Item Toggle: " + value);
            gameObject.SetActive(value);
        }
    }
}