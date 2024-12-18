using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Common.Scripts
{
    public interface IPlayerController
    {
        public int attack(GameObject enemy, int atk);
        public int beAttacked(int atk);
    }
}