using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoldCollectable : MonoBehaviour
{
    public int goldValue = 100;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            //other.GetComponent<PlayerController>().ChangeGold(goldValue);
            Destroy(gameObject);
        }
    }
}
