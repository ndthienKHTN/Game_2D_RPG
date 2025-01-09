using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Common.Scripts.UI.Model.ItemParameters;
using System;

namespace Common.Scripts.UI.Model
{
    [CreateAssetMenu]
    public abstract class ItemSO : ScriptableObject
    {
        [field: SerializeField]
        public bool isStackable { get; set; }

        public int ID => GetInstanceID();

        [field: SerializeField]
        public int MaxStackSize { get; set; } = 1;

        [field: SerializeField]
        public string Name { get; set; }

        [field: SerializeField]
        [field: TextArea]
        public string Description { get; set; }

        [field: SerializeField]
        public Sprite ItemImage { get; set; }

        [field: SerializeField]
        public List<ItemParameter> DefaultParametersList { get; set; }

    }

    [Serializable]
    public struct ItemParameter: IEquatable<ItemParameter>
    {
        public ItemParameterSO itemParameter;
        public int value;

        public bool Equals(ItemParameter other)
        {
            return itemParameter == other.itemParameter;
        }
    }
}