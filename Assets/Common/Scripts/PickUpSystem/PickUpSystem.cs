using Common.Scripts.UI.Model;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Common.Scripts.UI.PickUpSystem;

namespace Common.Scripts.UI.PickUpSystem
{
    public class PickUpSystem : MonoBehaviour
    {
        [SerializeField]
        private InventorySO inventoryData;

        private void OnTriggerEnter2D(Collider2D collision)
        {
            Item item = collision.GetComponent<Item>();

            if (item != null)
            {
                int reminder = inventoryData.AddItem(item.InventoryItem, item.Quantity);
                if (reminder == 0)
                {
                    item.DestroyItem();
                }
                else
                {
                    item.Quantity = reminder;
                }
                
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