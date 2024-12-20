using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FrameController : MonoBehaviour
{
    public GameObject actionFrame;
    private Button button;

    // Start is called before the first frame update
    void Start()
    {
        button = GetComponent<Button>();
        button.onClick.AddListener(OpenActionFrame);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnDestroy()
    {
        button.onClick.RemoveListener(OpenActionFrame);
    }

    public void OpenActionFrame()
    {
        actionFrame.SetActive(true);
        //Time.timeScale = 0;
        //UICanvasController.gameIsPaused = true;
    }
}
