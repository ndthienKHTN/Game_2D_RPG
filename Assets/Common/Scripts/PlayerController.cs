using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Common.Scripts
{
    public interface PlayerController
    {
        public int attack(int atk);
        public int beAttacked(int atk);
    }
}