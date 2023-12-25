using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class WordJumbleSolver : MonoBehaviour
{
    public TMP_InputField guessInputField;
    public TextMeshProUGUI feedbackText;

    private string actualWord; // Word to guess


    void Start()
    {
        // Hide input field and feedback text initially
        guessInputField.gameObject.SetActive(false);
        feedbackText.gameObject.SetActive(false);
    }

    // Call this method when all letters are collected
    public void ShowInputField(string wordToGuess)
    {
        actualWord = wordToGuess;
        guessInputField.gameObject.SetActive(true);
        feedbackText.gameObject.SetActive(false);
        guessInputField.text = ""; // Clear previous input
        guessInputField.ActivateInputField(); // Focus on the input field
    }

    // Method to be called when player submits their guess
    public void CheckFinalWordGuess()
    {
        Debug.Log($"actualWord: {actualWord}");
        if (guessInputField.text.Equals(actualWord, System.StringComparison.OrdinalIgnoreCase))
        {
            feedbackText.text = "Success!";
            Debug.Log($"inputField.text right: {guessInputField.text}");
        }
        else
        {
            feedbackText.text = "Try again";
            Debug.Log($"inputField.text wrong: {guessInputField.text}");

        }
        feedbackText.gameObject.SetActive(true);
    }
}
