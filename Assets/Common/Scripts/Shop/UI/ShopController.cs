using Common.Scripts.Shop.Model;
using Common.Scripts.UI;
using Common.Scripts.UI.Model;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace Common.Scripts.Shop.UI
{
    public class ShopController : MonoBehaviour
    {
        [SerializeField]
        public UIShopPage shopUI;

        [SerializeField]
        public ShopSO shopData;

        public List<ShopItem> shopItems = new List<ShopItem>();

        [SerializeField]
        private AudioClip buyClip;

        [SerializeField]
        private AudioSource audioSource;


        // Start is called before the first frame update
        void Start()
        {
            PrepareUI();
            PrepareShopData();
        }

        private void PrepareShopData()
        {
            shopData.Initialize();
            shopData.OnShopUpdated += UpdateShopUI;
            foreach (ShopItem item in shopItems)
            {
                if (item.IsEmpty)
                {
                    continue;
                }

                shopData.AddItem(item);
            }
        }



        private void UpdateShopUI(Dictionary<int, ShopItem> dictionary)
        {
            shopUI.ResetAllItems();

            foreach (var item in dictionary)
            {
                shopUI.UpdateData(
                    item.Key,
                    item.Value.item.ItemImage,
                    1,
                    item.Value.price
                );
            }
        }

        private void PrepareUI()
        {
            shopUI.InitializeShopUI(shopData.Size);
            shopUI.OnDescriptionRequested += HandleDescriptionRequested;
            shopUI.OnBuyRequested += HandleBuyRequested;
            shopUI.OnSwapItems += HandleSwapItems;
            shopUI.OnStartDragging += HandleStartDragging;
            shopUI.OnItemActionRequested += HandleItemActionRequested;
        }

        private void HandleItemActionRequested(int itemIndex)
        {
            ShopItem shopItem = shopData.GetItemAt(itemIndex);

            if (shopItem.IsEmpty)
            {
                return;
            }

            shopUI.AddAction("Buy", 
                () => PerformAction(itemIndex)
            );
        }

        private void PerformAction(int itemIndex)
        {
            throw new NotImplementedException();
        }

        private void HandleStartDragging(int itemIndex)
        {
            ShopItem shopItem = shopData.GetItemAt(itemIndex);

            if (shopItem.IsEmpty)
            {
                return;
            }

            shopUI.CreateDraggedItem(
                shopItem.item.ItemImage,
                1,
                shopItem.price
            );
        }

        private void HandleSwapItems(int itemIndex1, int itemIndex2)
        {
            shopData.SwapItem(itemIndex1, itemIndex2);  
        }

        private void HandleBuyRequested(int itemIndex)
        {
            ShopItem shopItem = shopData.GetItemAt(itemIndex);

            if (shopItem.IsEmpty)
            {
                return;
            }

            //TODO
        }

        private void HandleDescriptionRequested(int itemIndex)
        {
            ShopItem shopItem = shopData.GetItemAt(itemIndex);

            if (shopItem.IsEmpty)
            {
                shopUI.ResetSelection();
                return;
            }

            ItemSO item = shopItem.item;

            string description = PrepareDescription(shopItem);


            //TODO

            shopUI.UpdateDescription(
                itemIndex, 
                item.ItemImage,
                item.Name,
                description,
                shopItem.price,
                shopItem.price);

        }

        private string PrepareDescription(ShopItem shopItem)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(shopItem.item.Description);
            sb.AppendLine();

            for (int i = 0; i < shopItem.itemState.Count; i++)
            {
                sb.Append($"{shopItem.itemState[i].itemParameter.ParameterName}" +
                $": {shopItem.itemState[i].value} / {shopItem.item.DefaultParametersList[i].value}");
                sb.AppendLine();
            }

            return sb.ToString();
        }

        // Update is called once per frame
        void Update()
        {
            if (Input.GetKeyDown(KeyCode.L))
            {
                if (shopUI.isActiveAndEnabled == false)
                {
                    shopUI.Show();

                    foreach (var item in shopData.GetCurrentShopState())
                    {
                        shopUI.UpdateData(
                            item.Key,
                            item.Value.item.ItemImage,
                            1,
                            item.Value.price
                        );
                    }
                }
                else
                {
                    shopUI.Hide();
                }
            }
        }
    }
}