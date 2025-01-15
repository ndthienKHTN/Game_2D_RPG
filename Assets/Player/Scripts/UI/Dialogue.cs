using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Dialogue : MonoBehaviour
{
    public TMP_Text dialogueText;
    private Queue<string> lines;
    
    void Awake()
    {
        gameObject.SetActive(true);
        if (dialogueText == null)
        {
            dialogueText = GameObject.Find("DialogueText").GetComponent<TMP_Text>();
        }

        lines = new Queue<string>();
        // gameObject.SetActive(false);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            DisplayNextLine();
        }
        else if (Input.GetKeyDown(KeyCode.R))
        {
            EndDialogue();
        }
    }

    public void StartDialogue(string[] dialogueLines)
    {
        // Debug.Log("Starting dialogue");
        gameObject.SetActive(true);
        // Debug.Log("DialogueBox set to active");
        lines.Clear();

        foreach (string line in dialogueLines)
        {
            lines.Enqueue(line);
        }

        DisplayNextLine();
    }

    public bool IsDialogueActive()
    {
        return gameObject.activeSelf;
    }

    public void DisplayNextLine()
    {
        if (lines.Count == 0)
        {
            EndDialogue();
            return;
        }

        string line = lines.Dequeue();
        Debug.Log("Displaying line: " + line);
        dialogueText.text = line;
    }

    public void EndDialogue()
    {
        // Debug.Log("Ending dialogue");
        gameObject.SetActive(false);
    }
}
