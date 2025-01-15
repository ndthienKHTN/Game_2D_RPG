using Common.Scripts.UI.Model;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Common.Scripts
{
    public interface IWeaponSystem
    {
        public void SetWeapon(EquippableItemSO weaponItemSO, List<ItemParameter> itemState);

    }
}