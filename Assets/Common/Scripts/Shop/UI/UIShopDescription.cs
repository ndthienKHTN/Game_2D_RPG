using Common.Scripts.Shop.Model;
using Common.Scripts.UI.Model;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;


namespace Common.Scripts.Shop.UI
{
    public class UIShopDescription : MonoBehaviour
    {

        [SerializeField]
        private Image itemImage;

        [SerializeField]
        private TMP_Text title;

        [SerializeField]
        private TMP_Text description;

        [SerializeField]
        private TMP_Text price;

        [SerializeField]
        private Button buyButton;

        [SerializeField]
        private TMP_Text currentGold;

        private int currentItemIndex = -1;

        [SerializeField]
        public ShopSO shopData;

        [SerializeField]
        private InventorySO inventoryData;

        public void Awake()
        {
            ResetDescription();
        }

        public void ResetDescription()
        {
            itemImage.gameObject.SetActive(false);
            title.text = "";
            description.text = "";
            price.text = "";
            currentGold.text = "";
            buyButton.gameObject.SetActive(false);
            currentItemIndex = -1;
        }

        public void SetDescription(Sprite sprite, string itemName,
            string itemDescription, double price, double currentGold, int itemIndex)
        {
            itemImage.gameObject.SetActive(true);
            itemImage.sprite = sprite;
            title.text = itemName;
            description.text = itemDescription;
            this.price.text = "Price: " + price.ToString();
            this.currentGold.text = "Current gold: " +  currentGold.ToString();
            currentItemIndex = itemIndex;
            buyButton.gameObject.SetActive(true);
        }

        // Start is called before the first frame update
        void Start()
        {

            buyButton.onClick.AddListener(
                () => onClickAction()
            );
        }

        // Update is called once per frame
        void Update()
        {

        }

        public void onClickAction()
        {
            if (currentItemIndex == -1)
            {
                return;
            }

            // if (inventoryData.Gold < 0)
            // {
            //     return;
            // }

            //inventoryData.Gold -= 0;
            ShopItem item = shopData.GetItemAt(currentItemIndex);
            inventoryData.AddItem(item);
            //ResetDescription();
        }
    }

}