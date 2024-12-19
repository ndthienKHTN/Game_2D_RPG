using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class QuitButtonBehavior : MonoBehaviour
{
    public GameObject inventoryWindow;
    private Button button;
    // Start is called before the first frame update
    void Start()
    {
        button = GetComponent<Button>();
        button.onClick.AddListener(CloseWindow);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnDestroy()
    {
        button.onClick.RemoveListener(CloseWindow);
    }

    public void CloseWindow()
    {

        inventoryWindow.SetActive(false);
        Time.timeScale = 1;
        UICanvasController.gameIsPaused = false;
    }

}
