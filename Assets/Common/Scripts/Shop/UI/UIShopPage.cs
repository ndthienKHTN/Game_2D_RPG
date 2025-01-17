using Common.Scripts.UI;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Common.Scripts.Shop.UI
{
    public class UIShopPage : MonoBehaviour
    {
        [SerializeField]
        private UIShopItem itemPrefab;

        [SerializeField]
        private Transform contentPanel;

        [SerializeField]
        private UIShopDescription itemDescription;

        [SerializeField]
        private ShopMouseFollower mouseFollower;

        //[SerializeField]
        private List<UIShopItem> listItems = new List<UIShopItem>();

        private int currentlyDraggedItemIndex = -1;

        public event Action<int> OnDescriptionRequested,
            OnItemActionRequested,
            OnStartDragging,
            OnBuyRequested;

        public event Action<int, int> OnSwapItems;

        [SerializeField]
        private ShopItemActionPanel actionPanel;

        private void Awake()
        {
            Hide();
            mouseFollower.Toggle(false);
            itemDescription.ResetDescription();
        }

        public void InitializeShopUI(int inventorySize)
        {
            Debug.Log("Initializing shop UI");
            for (int i = 0; i < inventorySize; i++)
            {
                UIShopItem item =
                    Instantiate(itemPrefab,
                        Vector3.zero,
                        Quaternion.identity);

                item.transform.SetParent(contentPanel);
                item.OnItemClicked += HandleItemSelection;
                item.OnItemDroppedOn += HandleSwap;
                item.OnItemBeginDrag += HandleBeginDrag;
                item.OnItemEndDrag += HandleEndDrag;
                item.OnRightMouseButtonClick += HandleShowItemActions;
                listItems.Add(item);
            }
        }

        public void UpdateData(int itemIndex, Sprite itemImage,
            int itemQuantity, double price)
        {
            
            if (listItems.Count > itemIndex)
            {
                listItems[itemIndex].SetData(itemImage, itemQuantity, price);
            }
        }

        private void HandleShowItemActions(UIShopItem item)
        {
            int index = listItems.IndexOf(item);

            if (index == -1)
            {
                return;
            }

            OnItemActionRequested?.Invoke(index);
        }

        private void HandleEndDrag(UIShopItem item)
        {
            ResetDraggedItem();
        }

        private void HandleBeginDrag(UIShopItem item)
        {
            int index = listItems.IndexOf(item);

            if (index == -1)
            {
                return;
            }

            currentlyDraggedItemIndex = index;
            HandleItemSelection(item);
            OnStartDragging?.Invoke(index);
        }

        public void CreateDraggedItem(Sprite image, int quantity, double price)
        {
            mouseFollower.Toggle(true);
            mouseFollower.SetData(image, quantity, price);
        }
        private void HandleSwap(UIShopItem item)
        {
            int index = listItems.IndexOf(item);

            if (index == -1)
            {
                //ResetDraggedItem();
                return;
            }

            OnSwapItems?.Invoke(currentlyDraggedItemIndex, index);
            HandleItemSelection(item);
        }

        private void ResetDraggedItem()
        {
            mouseFollower.Toggle(false);
            currentlyDraggedItemIndex = -1;
        }

        private void HandleItemSelection(UIShopItem item)
        {
            int index = listItems.IndexOf(item);

            if (index == -1)
            {
                //ResetDraggedItem();
                return;
            }

            OnDescriptionRequested?.Invoke(index);
        }

        public void Show()
        {
            gameObject.SetActive(true);

            ResetSelection();
        }

        public void ResetSelection()
        {
            itemDescription.ResetDescription();
            DeselectAllItems();
        }

        public void AddAction(string actionName, Action performAction)
        {
            actionPanel.AddButton(actionName, performAction);
        }

        public void ShowItemAction(int itemIndex)
        {
            actionPanel.Toggle(true);
            actionPanel.transform.position = listItems[itemIndex].transform.position;
        }

        private void DeselectAllItems()
        {
            foreach (var item in listItems)
            {
                item.Deselect();
            }
            actionPanel.Toggle(false);
        }

        public void Hide()
        {
            actionPanel.Toggle(false);
            gameObject.SetActive(false);
            ResetDraggedItem();
        }

        public void UpdateDescription(int itemIndex, Sprite itemImage, 
            string name, string description, double price, double currentGold)
        {
            itemDescription.SetDescription(itemImage, name, description, price, currentGold, itemIndex);
            DeselectAllItems();
            listItems[itemIndex].Select();
        }

        public void ResetAllItems()
        {
            foreach (var item in listItems)
            {
                item.ResetData();
                item.Deselect();
            }
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