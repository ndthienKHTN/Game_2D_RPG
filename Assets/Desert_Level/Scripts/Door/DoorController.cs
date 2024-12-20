using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorController : MonoBehaviour
{
    bool isOpen = true;
    public GameObject door;
    public GameObject bringerOfDeath;
    //public GameObject key;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player") && isOpen)
        {
            door.SetActive(true);
            isOpen = false;
            bringerOfDeath.SetActive(true);
            gameObject.SetActive(false);
        }
    }


}
