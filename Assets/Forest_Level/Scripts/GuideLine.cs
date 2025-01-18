using System.Collections;
using UnityEngine;
using TMPro; // Import TextMeshPro namespace
using UnityEngine.UI; // Import UI namespace

public class GuideLine : MonoBehaviour
{
    public TextMeshProUGUI instructionText; // Reference to the TextMeshProUGUI element
    public Image backgroundImage; // Reference to the background Image element
    private float displayTime = 2f; // Time to display the instructions

    // Start is called before the first frame update
    void Start()
    {
        // Set the instruction text
        instructionText.text = "Press 1, 2, 3 to change weapons\nMove with A, S, D, W\nE and R to open and close NPC";

        StartCoroutine(HideInstructions());
    }

    // Coroutine to hide the instructions after a delay
    IEnumerator HideInstructions()
    {
        yield return new WaitForSeconds(displayTime);
        //check null
        if (instructionText != null)
            instructionText.gameObject.SetActive(false);
        if (backgroundImage != null)
            backgroundImage.gameObject.SetActive(false); // Hide the background image as well
    }
}