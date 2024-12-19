using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Assets.Desert_Level.Scripts;

namespace Assets.Desert_Level.Scripts
{
    public class EnemyUIHealthBar : MonoBehaviour
    {
        public Image mask;

        float originalSize;
        // Start is called before the first frame update
        void Start()
        {
            originalSize = mask.rectTransform.rect.width;
        }

        // Update is called once per frame
        void Update()
        {

        }
        public void SetValue(float value)
        {
            mask.rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, originalSize * value);
        }
    }
}