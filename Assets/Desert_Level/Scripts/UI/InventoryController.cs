using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryController : Singleton<InventoryController>
{
    public List<GameObject> slots = new List<GameObject>();
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void addEquipment(GameObject equipment)
    {
        /*for (int i = 0; i < slots.Count; i++)
        {
            if (slots[i].transform.childCount == 0)
            {
                GameObject equip = Instantiate(equipment, slots[i].transform);
                equip.transform.localPosition = Vector3.zero;
                equip.transform.localScale = new Vector3(1, 1, 1);
                break;
            }
        }*/
        
    }
}
