using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Assets.Forest_Level.Scripts;


namespace Assets.Forest_Level.Scripts
{
    public class SceneManagement : Singleton<SceneManagement>
    {
        public string SceneTransitionName { get; private set; }

        public void SetTransitionName(string sceneTransitionName)
        {
            this.SceneTransitionName = sceneTransitionName;
        }
    }
}