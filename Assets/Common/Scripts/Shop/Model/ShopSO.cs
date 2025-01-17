using Common.Scripts.UI.Model;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using UnityEngine;


namespace Common.Scripts.Shop.Model
{
    [CreateAssetMenu]
    public class ShopSO : ScriptableObject
    {
        [SerializeField]
        private List<ShopItem> shopItems = new List<ShopItem>();

        [SerializeField]
        public int Size { get; private set; } = 20;

        public event Action<Dictionary<int, ShopItem>> OnShopUpdated;

        public void Initialize()
        {
            shopItems = new List<ShopItem>();
            for (int i = 0; i < Size; i++)
            {
                shopItems.Add(ShopItem.GetEmptyItem());
            }
        }

        public void AddItem(ItemSO item, double price, List<ItemParameter> itemState)
        {
            if (item.isStackable == false)
            {
                for (int i = 0; i < shopItems.Count; i++)
                {
                    while (IsShopFull() == false)
                    {
                        AddNonStackableItem(item, price, itemState);
                    }

                    InformAboutChange();
                    return;
                }
            }
            AddStackableItem(item, price, itemState);
            InformAboutChange();
            return;
        }

        private void AddStackableItem(ItemSO item, double price,
            List<ItemParameter> itemState = null)
        {
            ShopItem newItem = new ShopItem
            {
                item = item,
                price = price,
                itemState = new List<ItemParameter>
                    (itemState == null ? item.DefaultParametersList : itemState)
            };

            for (int i = 0; i < shopItems.Count; i++)
            {
                if (shopItems[i].IsEmpty)
                {
                    shopItems[i] = newItem;
                    return;
                }
            }
        }

        private void AddNonStackableItem(ItemSO item, double price,
            List<ItemParameter> itemState = null)
        {
            ShopItem newItem = new ShopItem
            {
                item = item,
                price = price,
                itemState = new List<ItemParameter>
                    (itemState == null ? item.DefaultParametersList : itemState)
            };

            for (int i = 0; i < shopItems.Count; i++)
            {
                if (shopItems[i].IsEmpty)
                {
                    shopItems[i] = newItem;
                    return;
                }
            }
        }

        private bool IsShopFull()
        {
            return shopItems.Where(item => item.IsEmpty).Any() == false;
        }

        public Dictionary<int, ShopItem> GetCurrentShopState()
        {
            Dictionary<int, ShopItem> shopState =
                new Dictionary<int, ShopItem>();

            for (int i = 0; i < shopItems.Count; i++)
            {
                if (shopItems[i].IsEmpty == false)
                {
                    continue;
                }

                shopState[i] = shopItems[i];
            }

            return shopState;
        }

        public ShopItem GetItemAt(int itemIndex)
        {
            return shopItems[itemIndex];
        }

        public void AddItem(ShopItem item)
        {
            AddItem(item.item, item.price, item.itemState);
        }

        public void SwapItem(int itemIndex1, int itemIndex2)
        {
            ShopItem temp = shopItems[itemIndex1];
            shopItems[itemIndex1] = shopItems[itemIndex2];
            shopItems[itemIndex2] = temp;
            InformAboutChange();
        }

        private void InformAboutChange()
        {
            OnShopUpdated?.Invoke(GetCurrentShopState());
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

    [Serializable]
    public struct ShopItem
    {
        public ItemSO item;
        public double price;
        public List<ItemParameter> itemState;

        public bool IsEmpty => item == null;

        public void Clear()
        {
            item = null;
            price = 0;
        }

        public static ShopItem GetEmptyItem()
        {
            return new ShopItem
            {
                item = null,
                price = 0,
                itemState = new List<ItemParameter>()
            };
        }
    }
}