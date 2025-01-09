using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Common.Scripts
{
    public interface IPlayerStatController
    {
        // Start is called before the first frame update

        public int increaseHealth(int increasedHealth);

        public int increaseDefense(int value);
    }
}