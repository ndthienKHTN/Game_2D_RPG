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
        }

        public void SetDescription(Sprite sprite, string itemName,
            string itemDescription, double price, double currentGold)
        {
            itemImage.gameObject.SetActive(true);
            itemImage.sprite = sprite;
            title.text = itemName;
            description.text = itemDescription;
            this.price.text = "Price: " + price.ToString();
            this.currentGold.text = "Current gold: " +  currentGold.ToString();
            buyButton.gameObject.SetActive(true);
        }

        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }
    }

}