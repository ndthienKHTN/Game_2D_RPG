using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Assets.Player.Scripts;

public class GainEXP : MonoBehaviour
{
    [SerializeField] private float expAmount = 10f;

    private void OnDestroy()
    {
        if (SceneManager.GetActiveScene().isLoaded)
        {
            PlayerController player = PlayerController.Instance;
            if (player != null)
            {
                player.GainEXP(expAmount);
            }
        }
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
}