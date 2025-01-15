using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Assets.Common.Scripts;
namespace Assets.Desert_Level.Scripts
{
    public class NPCController : MonoBehaviour, INPCController
    {
        public float displayTime = 4.0f;
        public GameObject dialogBox;
        float timerDisplay;
        // Start is called before the first frame update
        void Start()
        {
            dialogBox.SetActive(false);
            timerDisplay = -1.0f;
        }

        // Update is called once per frame
        void Update()
        {
            if (timerDisplay >= 0)
            {
                timerDisplay -= Time.deltaTime;
                if (timerDisplay < 0)
                {
                    dialogBox.SetActive(false);
                }
            }
        }

        public void DisplayDialog()
        {
            timerDisplay = displayTime;
            dialogBox.SetActive(true);
        }
    }
}