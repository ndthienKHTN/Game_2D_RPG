using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Assets.Desert_Level.Scripts;

namespace Assets.Desert_Level.Scripts
{
    public class GoldManager : Singleton<GoldManager>
    {
        public GameObject prefab;
         
        List<GameObject> goldList;
        // Start is called before the first frame update
        void Start()
        {
            goldList = new List<GameObject>();
        }

        // Update is called once per frame
        void Update()
        {
            //Debug.Log("Gold Count: " + goldList.Count);
            if (goldList.Count < 3)
            {
                spawnGold();
            }
        }

        private void spawnGold()
        {
            int goldCreated = 3 - goldList.Count;
            for (int i = 0; i < goldCreated; i++)
            {
                
                int randomX = Random.Range(0, 10);
                int randomY = Random.Range(0, 10);
                int randomZ = Random.Range(0, 10);
                Vector3 spawnPosition = new Vector3(randomX, randomY, randomZ);
                GameObject newObject = Instantiate(prefab, spawnPosition, Quaternion.identity);

                goldList.Add(newObject);
                Debug.Log("Gold added");
            }
            
        }

        public void RemoveGold(GameObject gold)
        {
            if (goldList.Contains(gold))
            {
                Debug.Log("Gold Removed");
                goldList.Remove(gold);
            }
        }
    }
}