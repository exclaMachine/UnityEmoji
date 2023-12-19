using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FontCollection
{
    public Dictionary<string, FontData> fonts;

    [System.Serializable]
    public class FontData
    {
        public string rarity;
        public Dictionary<char, bool> characters;
    }
}

private FontCollection fontsCollection;
private string currentWord;
private List<char> collectedLetters = new List<char>();

private void SelectRandomLetter()
{
    // Ensure the current word has more than one letter
    if (currentWord.Length <= 1) return;

    // Exclude the first letter and select a random letter
    int randomIndex = UnityEngine.Random.Range(1, currentWord.Length);
    char selectedLetter = currentWord[randomIndex];

    // Check and select a font for the letter
    foreach (var font in fontsCollection.fonts)
    {
        if (font.Value.characters.ContainsKey(selectedLetter) && !font.Value.characters[selectedLetter])
        {
            font.Value.characters[selectedLetter] = true; // Mark as collected
            collectedLetters.Add(selectedLetter);
            PlayerPrefs.SetString("CollectedLetters", new string(collectedLetters.ToArray())); // Store in PlayerPrefs
            UpdateUIWithCollectedLetter(selectedLetter, font.Key); // Update UI
            break;
        }
    }
}

private void UpdateUIWithCollectedLetter(char letter, string fontName)
{
    // Implement UI update logic to display the letter in the chosen font
}
