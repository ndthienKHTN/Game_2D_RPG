using Assets.Common.Scripts;
using Common.Scripts.UI.Model.ItemModifiers;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Common.Scripts.UI.Model.ItemModifiers
{
    [CreateAssetMenu]
    public class PlayerDefenseModifierSO : PlayerStatModifierSO
    {
        public override void AffectCharacter(IPlayerStatController character, int value)
        {
            if (character != null)
            {
                character.increaseDefense(value);
            }

        }
    }
}