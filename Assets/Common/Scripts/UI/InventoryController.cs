using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Common.Scripts.UI.Model;
using System.Text;

namespace Common.Scripts.UI
{

    public class InventoryController : MonoBehaviour
    {
        [SerializeField]
        public UIInventoryPage inventoryUI;

        [SerializeField]
        public InventorySO inventoryData;

        public List<InventoryItem> inventoryItems = new List<InventoryItem>();

        [SerializeField]
        private AudioClip dropClip;

        [SerializeField]
        private AudioSource audioSource;


        public void Start()
        {
            PrepareUI();
            PrepareInventoryData();
        }

        private void PrepareInventoryData()
        {
            inventoryData.Initialize();
            inventoryData.OnInventoryUpdated += UpdateInventoryUI;
            foreach (InventoryItem item in inventoryItems)
            {
                if (item.IsEmpty)
                {
                    continue;
                }

                inventoryData.AddItem(item);
            }
        }

        private void UpdateInventoryUI(Dictionary<int, InventoryItem> inventoryState)
        {
            inventoryUI.ResetAllItems();   

            foreach (var item in inventoryState)
            {
                inventoryUI.UpdateData(
                    item.Key,
                    item.Value.item.ItemImage,
                    item.Value.quantity
                    );
            }
        }

        private void PrepareUI()
        {
            inventoryUI.InitializeInventoryUI(inventoryData.Size);
            inventoryUI.OnDescriptionRequested += HandleDescriptionRequested;
            inventoryUI.OnSwapItems += HandleSwapItems;
            inventoryUI.OnStartDragging += HandleStartDragging;
            inventoryUI.OnItemActionRequested += HandleItemActionRequested;
        }

        private void HandleItemActionRequested(int itemIndex)
        {
            InventoryItem inventoryItem = inventoryData.GetItemAt(itemIndex);

            if (inventoryItem.IsEmpty)
            {
                return;
            }

            

            IItemAction itemAction = inventoryItem.item as IItemAction;

            if (itemAction != null)
            {
                // itemAction.PerformAction(gameObject, inventoryItem.itemState);
                
                inventoryUI.ShowItemAction(itemIndex);
                inventoryUI.AddAction(
                    itemAction.ActionName,
                    () => PerformAction(itemIndex)
                );

                
            }

            IDestroyableItem destroyableItem = inventoryItem.item as IDestroyableItem;

            if (destroyableItem != null)
            {
                //inventoryData.RemoveItem(itemIndex, 1);
                inventoryUI.AddAction("Drop",
                    () => DropItem(itemIndex, inventoryItem.quantity));
            }
        }

        private void DropItem(int itemIndex, int quantity)
        {
            inventoryData.RemoveItem(itemIndex, quantity);
            inventoryUI.ResetSelection();
            audioSource.PlayOneShot(dropClip);
        }

        public void PerformAction(int itemIndex)
        {
            InventoryItem inventoryItem = inventoryData.GetItemAt(itemIndex);

            if (inventoryItem.IsEmpty)
            {
                return;
            }

            IDestroyableItem destroyableItem = inventoryItem.item as IDestroyableItem;

            if (destroyableItem != null)
            {
                Debug.Log("Remove item");
                inventoryData.RemoveItem(itemIndex, 1);
                Debug.Log("Remove item 2");
                inventoryUI.ResetSelection();
            }

            IItemAction itemAction = inventoryItem.item as IItemAction;

            if (itemAction != null)
            {
                itemAction.PerformAction(gameObject, inventoryItem.itemState);

                if (itemAction.actionSFX != null)
                {
                    audioSource.PlayOneShot(itemAction.actionSFX);
                }

                //// Remove the item if it is an EquippableItemSO
                //if (inventoryItem.item is EquippableItemSO)
                //{
                //    inventoryData.RemoveItem(itemIndex, 1);
                //}
                

                if (inventoryData.GetItemAt(itemIndex).IsEmpty)
                {
                    inventoryUI.ResetSelection();
                }
            }
        }

        private void HandleStartDragging(int itemIndex)
        {
            InventoryItem inventoryItem = inventoryData.GetItemAt(itemIndex);

            if (inventoryItem.IsEmpty)
            {
                return;
            }

            inventoryUI.CreateDraggedItem(
                inventoryItem.item.ItemImage,
                inventoryItem.quantity
                );
        }

        private void HandleSwapItems(int itemIndex1, int itemIndex2)
        {
            inventoryData.SwapItems(itemIndex1, itemIndex2);
        }   

        private void HandleDescriptionRequested(int itemIndex)
        {
            InventoryItem inventoryItem = inventoryData.GetItemAt(itemIndex);

            if (inventoryItem.IsEmpty)
            {
                inventoryUI.ResetSelection();
                return;
            }

            ItemSO item = inventoryItem.item;
            string description = PrepareDescription(inventoryItem); 

            inventoryUI.UpdateDescription(
                itemIndex,
                item.ItemImage,
                item.Name,
                description);
        }

        private string PrepareDescription(InventoryItem inventoryItem)
        {
            StringBuilder sb= new StringBuilder();
            sb.Append(inventoryItem.item.Description);
            sb.AppendLine();

            for (int i = 0; i < inventoryItem.itemState.Count; i++)
            {
                sb.Append($"{inventoryItem.itemState[i].itemParameter.ParameterName}" +
                    $": {inventoryItem.itemState[i].value} / {inventoryItem.item.DefaultParametersList[i].value}");
                sb.AppendLine();
            }

            return sb.ToString();
        }

        public void Update()
        {
            if (Input.GetKeyDown(KeyCode.I))
            {
                if (inventoryUI.isActiveAndEnabled == false)
                {
                    inventoryUI.Show();

                    foreach (var item in 
                        inventoryData.GetCurrentInventoryState())
                    {
                        inventoryUI.UpdateData(
                            item.Key,
                            item.Value.item.ItemImage,
                            item.Value.quantity);
                    }
                }
                else
                {
                    inventoryUI.Hide();
                }
                
            }
        }
    }
}