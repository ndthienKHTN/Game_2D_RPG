using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Assets.Common.Scripts;

namespace Common.Scripts.UI.Model.ItemModifiers
{
    public abstract class PlayerStatModifierSO : ScriptableObject
    {
        public abstract void AffectCharacter(IPlayerStatController character, int value);
    }
}