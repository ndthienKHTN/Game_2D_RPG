using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Assets.Common.Scripts;

namespace Common.Scripts.UI.Model.ItemModifiers
{
    [CreateAssetMenu]
    public class PlayerStatHealthModifierSO : PlayerStatModifierSO
    {
        public override void AffectCharacter(IPlayerStatController character, int value)
        {
            if (character != null)
            {
                character.increaseHealth(value);
            }

        }
    }

}