using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Dialogue : MonoBehaviour
{
    public GameObject window; // The dialogue window
    public GameObject indicator; // The indicator to show when dialogue is available
    public TMP_Text dialogueText; // The text component for displaying dialogue
    public TMP_Text nameTag; // The text component for displaying the name of the speaker
    public List<string> dialogues; // List of dialogues
    public float writingSpeed; // Speed at which text is written out
    private int index; // Current dialogue index
    private int charIndex; // Current character index in the dialogue
    private bool started; // Whether the dialogue has started
    private bool waitForNext; // Whether to wait for the next dialogue

    private void Awake()
    {
        ToggleIndicator(false); // Hide the indicator initially
        ToggleWindow(false); // Hide the dialogue window initially
    }

    // Toggle the visibility of the dialogue window
    private void ToggleWindow(bool show)
    {
        window.SetActive(show);
    }

    // Toggle the visibility of the indicator
    public void ToggleIndicator(bool show)
    {
        indicator.SetActive(show);
    }

    // Start the dialogue
    public void StartDialogue()
    {
        if (started)
            return;

        started = true; // Mark dialogue as started
        ToggleWindow(true); // Show the dialogue window
        ToggleIndicator(false); // Hide the indicator
        nameTag.text = "Villager"; // Set the name tag to Villager
        GetDialogue(0); // Start with the first dialogue
    }

    // Get the dialogue at the specified index
    private void GetDialogue(int i)
    {
        index = i; // Set the current dialogue index
        charIndex = 0; // Reset the character index
        dialogueText.text = string.Empty; // Clear the dialogue text
        StartCoroutine(Writing()); // Start writing the dialogue
    }

    // End the dialogue
    public void EndDialogue()
    {
        started = false; // Mark dialogue as not started
        waitForNext = false; // Stop waiting for the next dialogue
        StopAllCoroutines(); // Stop all coroutines
        ToggleWindow(false); // Hide the dialogue window
    }

    // Coroutine to write the dialogue text character by character
    IEnumerator Writing()
    {
        yield return new WaitForSeconds(writingSpeed);

        string currentDialogue = dialogues[index];
        dialogueText.text += currentDialogue[charIndex]; // Add the next character to the dialogue text
        charIndex++; // Increment the character index

        if (charIndex < currentDialogue.Length) // If there are more characters to write
        {
            yield return new WaitForSeconds(writingSpeed); // Wait for the specified writing speed
            StartCoroutine(Writing()); // Continue writing
        }
        else
        {
            waitForNext = true; // Wait for the next dialogue
        }
    }

    private void Update()
    {
        if (!started)
            return;

        if (waitForNext && Input.GetKeyDown(KeyCode.E)) // If waiting for the next dialogue and the player presses E
        {
            waitForNext = false;
            index++;

            if (index < dialogues.Count) // If there are more dialogues
            {
                GetDialogue(index); // Get the next dialogue
            }
            else
            {
                nameTag.text = "Bryan"; // Change the name tag to Bryan
                ToggleIndicator(true); // Show the indicator
                EndDialogue(); // End the dialogue
            }
        }
    }
}