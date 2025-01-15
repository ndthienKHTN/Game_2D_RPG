using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Common.Scripts.UI
{
    public class UIInventoryPage : MonoBehaviour
    {
        [SerializeField]
        private UIInventoryItem itemPrefab;

        [SerializeField]
        private Transform contentPanel;

        [SerializeField]   
        private UIInventoryDescription itemDescription;

        [SerializeField]
        private MouseFollower mouseFollower;

        List<UIInventoryItem> listItems = new List<UIInventoryItem>();

        /*public Sprite image;
        public Sprite image2;
        public int quantity;
        public string title;
        public string description;*/

        private int currentlyDraggedItemIndex = -1;

        public event Action<int> OnDescriptionRequested,
            OnItemActionRequested,
            OnStartDragging;

        public event Action<int, int> OnSwapItems;

        [SerializeField]
        private ItemActionPanel actionPanel;


        private void Awake()
        {
            Hide();
            mouseFollower.Toggle(false);
            itemDescription.ResetDescription();
        }

        public void InitializeInventoryUI(int inventorySize)
        {
            for (int i = 0; i < inventorySize; i++)
            {
                UIInventoryItem item =
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
            int itemQuantity)
        {
            if (listItems.Count > itemIndex)
            {
                listItems[itemIndex].SetData(itemImage, itemQuantity);
            }
        }

        private void HandleShowItemActions(UIInventoryItem item)
        {
            int index = listItems.IndexOf(item);

            if (index == -1)
            {
                return;
            }

            OnItemActionRequested?.Invoke(index);
        }

        private void HandleEndDrag(UIInventoryItem item)
        {
            ResetDraggedItem();
        }

        private void HandleBeginDrag(UIInventoryItem item)
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

        public void CreateDraggedItem(Sprite image, int quantity)
        {
            mouseFollower.Toggle(true);
            mouseFollower.SetData(image, quantity);
        }
        private void HandleSwap(UIInventoryItem item)
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

        private void HandleItemSelection(UIInventoryItem item)
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

        public void UpdateDescription(int itemIndex, Sprite itemImage, string name, string description)
        {
            itemDescription.SetDescription(itemImage, name, description);
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
       
    }
}