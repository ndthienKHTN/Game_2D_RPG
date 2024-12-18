using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Common.Scripts
{
    public interface IEnemyController
    {
        public int attack(GameObject player, int atk);
        public int beAttacked(int atk);
    }
}