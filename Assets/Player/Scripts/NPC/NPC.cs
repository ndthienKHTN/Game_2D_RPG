using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.IO;

public class NPC : MonoBehaviour
{
    public string dialogueName;
    private Dialogue dialogueSystem;
    private bool playerInRange;

    void Awake()
    {
        // Debug.Log("NPC Awake method called");

        // Find the UICanvas GameObject by name
        GameObject uiCanvas = GameObject.Find("UICanvas");
        if (uiCanvas != null)
        {
            // Debug.Log("UICanvas GameObject found");

            // Find the DialogueBox GameObject within the UICanvas
            GameObject dialogueBox = uiCanvas.transform.Find("DialogueBox").gameObject;
            if (dialogueBox != null)
            {
                // Debug.Log("DialogueBox GameObject found");
                dialogueSystem = dialogueBox.GetComponent<Dialogue>();
                if (dialogueSystem != null)
                {
                    // Debug.Log("Dialogue component found");
                }
                else
                {
                    Debug.LogError("Dialogue component not found on DialogueBox");
                }
            }
            else
            {
                // Debug.LogError("DialogueBox GameObject not found within UICanvas");
            }
        }
        else
        {
            Debug.LogError("UICanvas GameObject not found");
        }

        playerInRange = false;
    }

    void Update()
    {
        if (playerInRange && Input.GetKeyDown(KeyCode.E))
        {
            if (dialogueSystem.IsDialogueActive())
            {
                dialogueSystem.DisplayNextLine();
            }
            else
            {
                StartDialogue();
            }
        }
        else if (playerInRange && Input.GetKeyDown(KeyCode.R))
        {
            dialogueSystem.EndDialogue();
        }
    }

    void StartDialogue()
    {
        if (dialogueSystem == null)
        {
            Debug.LogError("Dialogue system is not assigned");
            return;
        }

        // Debug.Log("Attempting to start dialogue");
        string path = Path.Combine(Application.dataPath, "Player/dialogue", dialogueName + ".txt");
        if (File.Exists(path))
        {
            string[] lines = File.ReadAllLines(path);
            dialogueSystem.StartDialogue(lines);
        }
        else
        {
            Debug.LogError("Dialogue file not found: " + path);
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            // Debug.Log("Player entered trigger");
            playerInRange = true;
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            // Debug.Log("Player exited trigger");
            playerInRange = false;
        }
    }
}
