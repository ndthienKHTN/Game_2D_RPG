using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Assets.Common.Scripts;

namespace Common.Scripts.UI.Model
{
    [CreateAssetMenu]
    public class EquippableItemSO : ItemSO, IDestroyableItem, IItemAction
    {
        [SerializeField]
        private List<ModifierData> modifiersData = new List<ModifierData>();

        //  public List<ModifierData> ModifiersData => modifiersData;

        public string ActionName => "Equip";

        [SerializeField]
        public AudioClip actionSFX { get; private set;}

        //public void Equip(GameObject character)
        //{
        //    foreach (ModifierData modifierData in modifiersData)
        //    {
        //        modifierData.statModifier.AffectCharacter(character.GetComponent<IPlayerController>(), modifierData.value);
        //    }
        //}

        public bool PerformAction(GameObject character, List<ItemParameter> itemState = null)
        {
            IWeaponSystem weaponSystem = character.GetComponentInChildren<IWeaponSystem>();

            if (weaponSystem != null)
            {
                weaponSystem.SetWeapon(this, itemState == null ?
                    DefaultParametersList : itemState);
            }

            IPlayerStatController playerStatController = character.GetComponent<IPlayerStatController>();

            Debug.Log("PlayerStatController: " + playerStatController);
            if (playerStatController != null)
            {
                foreach (var modifierData in modifiersData)
                {
                    modifierData.statModifier.AffectCharacter(playerStatController, modifierData.value);
                }

            }
            

            return true;
        }

        //public void Unequip(GameObject character)
        //{
        //    foreach (ModifierData modifierData in modifiersData)
        //    {
        //       // modifierData.statModifier.RemoveEffect(character.GetComponent<IPlayerController>(), modifierData.value);
        //    }
        //}
    }
}