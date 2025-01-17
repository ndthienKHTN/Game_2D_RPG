using Common.Scripts.UI;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;


namespace Common.Scripts.Shop.UI
{
    public class UIShopItem : MonoBehaviour, IPointerClickHandler,
            IBeginDragHandler, IDropHandler, IEndDragHandler, IDragHandler
    {
        [SerializeField]
        private Image itemImage;


        //[SerializeField]
        //private TMP_Text quantityTxt;

        [SerializeField]
        private Image borderImage;

        [SerializeField]
        private double price;

        public event Action<UIShopItem> OnItemClicked,
            OnItemDroppedOn, OnItemBeginDrag, OnItemEndDrag,
            OnRightMouseButtonClick;

        private bool empty = true;

        private void Awake()
        {
            ResetData();
            Deselect();
        }

        public void ResetData()
        {
            this.itemImage.gameObject.SetActive(false);
            empty = true;
        }

        public void Deselect()
        {
            borderImage.enabled = false;
        }

        public void SetData(Sprite sprite, int quantity, double price)
        {
            this.itemImage.gameObject.SetActive(true);
            this.itemImage.sprite = sprite;
            //this.quantityTxt.text = quantity.ToString();
            this.price = price;
            empty = false;
        }

        public void Select()
        {
            borderImage.enabled = true;
        }

        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }

        public void OnBeginDrag(PointerEventData eventData)
        {
            if (empty)
            {
                return;
            }

            OnItemBeginDrag?.Invoke(this);
        }

        public void OnDrag(PointerEventData eventData)
        {
            
        }

        public void OnDrop(PointerEventData eventData)
        {
            OnItemDroppedOn?.Invoke(this);
        }

        public void OnEndDrag(PointerEventData eventData)
        {
            OnItemEndDrag?.Invoke(this);
        }

        public void OnPointerClick(PointerEventData pointerEventData)
        {
            if (pointerEventData.button == PointerEventData.InputButton.Right)
            {
                OnRightMouseButtonClick?.Invoke(this);
            }
            else
            {
                OnItemClicked?.Invoke(this);
            }
        }
    }
}