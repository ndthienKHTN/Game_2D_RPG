using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;


namespace Common.Scripts.Shop.UI
{
    public class ShopItemActionPanel : MonoBehaviour
    {
        [SerializeField]
        private GameObject buttonPrefab;

        public void AddButton(string name, Action onClickAction)
        {
            GameObject button = Instantiate(buttonPrefab, transform);
            button.GetComponent<Button>().onClick.AddListener(
                () => onClickAction()
            );

            button.GetComponentInChildren<TMP_Text>().text = name;
        }

        public void Toggle(bool value)
        {
            if (value == true)
            {
                RemoveOldButtons();

            }
            gameObject.SetActive(value);
        }

        private void RemoveOldButtons()
        {
            foreach (Transform childObj in transform)
            {
                Destroy(childObj.gameObject);
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