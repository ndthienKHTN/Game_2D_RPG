using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Common.Scripts.UI.Model.ItemModifiers;
using System;
using Assets.Common.Scripts;

namespace Common.Scripts.UI.Model
{
    [CreateAssetMenu]
    public class EdibleItemSO : ItemSO, IDestroyableItem, IItemAction
    {
        [SerializeField]
        private List<ModifierData> modifiersData = new List<ModifierData>();

        public string ActionName => "Consume";

        [SerializeField]
        public AudioClip actionSFX { get; private set;}

        public bool PerformAction(GameObject character, List<ItemParameter> itemState = null)
        {
            foreach (ModifierData modifierData in modifiersData)
            {
                modifierData.statModifier.AffectCharacter(character.GetComponent<IPlayerStatController>(), modifierData.value);
            }

            return true;
        }
    }

    public interface IDestroyableItem
    {

    }

    public interface IItemAction
    {
        public string ActionName { get; }
        public AudioClip actionSFX { get; }
        bool PerformAction(GameObject character, List<ItemParameter> itemState);
    }

    [Serializable]
    public class ModifierData
    {
        public PlayerStatModifierSO statModifier;
        public int value;
    }
}