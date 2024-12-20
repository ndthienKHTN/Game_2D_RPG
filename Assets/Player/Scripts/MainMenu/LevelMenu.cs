using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Assets.Player.Scripts 
{
    public class LevelMenu : MonoBehaviour
    {
        public void Button_Level(string Name){
            UnityEngine.SceneManagement.SceneManager.LoadScene(Name);
        }
        
    }
}