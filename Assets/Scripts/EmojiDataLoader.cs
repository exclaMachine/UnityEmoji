using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class EmojiDataWrapper
{
    public List<EmojiGroup> groups;
}

[Serializable]
public class EmojiGroup
{
    public string name;
    public string interjection;
    public List<EmojiSubgroup> subgroups;
}

[Serializable]
public class EmojiSubgroup
{
    public string name;
    public List<EmojiCategory> categories;
}

[Serializable]
public class EmojiCategory
{
    public string name;
    public string interjection;
    public List<EmojiInfo> emojis;
}

[Serializable]
public class EmojiInfo
{
    public string code; // Unicode representation
    public string character; // Emoji character
    public string description;
    public List<string> emoticons; // List of emoticons
}

[System.Serializable]
public class WordList
{
    public List<string> words;
}



public class EmojiDataLoader : MonoBehaviour
{
    public TextAsset emojiJson; // Assign this in the Unity Editor
    public TextAsset wordJson; // Assign this in the Unity Editor
    public TextAsset fontsJson; // Assign this in Unity Editor

    private FontCollection fontsCollection;

    public string m_sCurWord;

    //private EmojiData emojiData;
    private EmojiDataWrapper emojiData;

    private List<string> currentPlaythroughEmojiDescriptions = new List<string>();
    private List<EmojiInfo> currentPlaythroughEmojis = new List<EmojiInfo>();

    private int m_emojiGroupIndex;
    void Start()
    {
        LoadEmojiData();
        // Get the current word index
        m_sCurWord = GetCurrentWord();

        CreateFontData();

        Debug.Log($"curWord{m_sCurWord}");

        //TestRandomEmojiSelection();
        // GetRandomEmojisFromSameCategory();
    }

    private void LoadEmojiData()
    {
        if (emojiJson != null)
        {
            emojiData = JsonUtility.FromJson<EmojiDataWrapper>(emojiJson.text);
            Debug.Log("Emoji data loaded successfully.");
        }
        else
        {
            Debug.LogError("Emoji JSON file is not assigned.");
        }
    }

    private string GetCurrentWord()
    {
        List<string> words = LoadWordList();
        int totalWords = words.Count;
        int currentWordIndex = PlayerPrefs.GetInt("WordIndex", 0);

        // Make sure the index is within the range of available words
        if (currentWordIndex < totalWords)
        {
            return words[currentWordIndex];
        }

        return null; // or handle this situation as needed
    }

    private List<string> LoadWordList()
    {
        // Deserialize the JSON string into the WordList object
        WordList wordList = JsonUtility.FromJson<WordList>(wordJson.text);
        return wordList.words;
    }

    private void CreateFontData()
    {
        if (fontsJson != null)
        {
            fontsCollection = JsonUtility.FromJson<FontCollection>(fontsJson.text);
            Debug.Log("Fonts data loaded successfully.");

            // Example of accessing data
            // foreach (var font in fontsCollection.fonts)
            // {
            //     Debug.Log($"Font: {font.name}, Rarity: {font.rarity}");
            //     foreach (var character in font.characters)
            //     {
            //         Debug.Log($"Character: {character.character}, Collected: {character.collected}");
            //     }
            // }
        }
        else
        {
            Debug.LogError("Fonts JSON file is not assigned.");
        }
    }


    private void UpdateWordIndex()
    {
        int currentWordIndex = PlayerPrefs.GetInt("WordIndex", 0);
        currentWordIndex = (currentWordIndex + 1) % LoadWordList().Count;

        PlayerPrefs.SetInt("WordIndex", currentWordIndex);
        PlayerPrefs.Save();
    }

    // Public method to get emojiData
    public EmojiDataWrapper GetEmojiData()
    {
        return emojiData;
    }

    private void TestRandomEmojiSelection()
    {
        EmojiInfo[] randomEmojis = GetRandomEmojisFromSameCategory();
        if (randomEmojis == null || randomEmojis.Length < 2)
        {
            Debug.Log("No random emojis were selected.");
            return;
        }

        Debug.Log($"Random Emoji 1: {randomEmojis[0].character} - {randomEmojis[0].description}");
        Debug.Log($"Random Emoji 2: {randomEmojis[1].character} - {randomEmojis[1].description}");
    }

    public EmojiInfo[] GetRandomEmojisFromSameCategory()
    {
        // // Clear the current descriptions list
        // currentPlaythroughEmojiDescriptions.Clear();

        if (emojiData == null || emojiData.groups == null || emojiData.groups.Count == 0)
        {
            Debug.LogError("Emoji data is not loaded or has no groups");
            return null;
        }

        int randomGroupIndex = UnityEngine.Random.Range(0, emojiData.groups.Count);
        m_emojiGroupIndex = randomGroupIndex;

        // Select a random group
        var randomGroup = emojiData.groups[randomGroupIndex];
        Debug.Log($"Random Group Index: {randomGroupIndex}, Name: {randomGroup.name}, Number of Subgroups: {randomGroup.subgroups.Count}");

        if (randomGroup.subgroups == null || randomGroup.subgroups.Count == 0)
        {
            Debug.LogError("Selected group has no subgroups");
            return null;
        }

        // Select a random subgroup
        var randomSubgroup = randomGroup.subgroups[UnityEngine.Random.Range(0, randomGroup.subgroups.Count)];
        if (randomSubgroup.categories == null || randomSubgroup.categories.Count == 0)
        {
            Debug.LogError("Selected subgroup has no categories");
            return null;
        }

        // Select a random category
        var randomCategory = randomSubgroup.categories[UnityEngine.Random.Range(0, randomSubgroup.categories.Count)];
        if (randomCategory.emojis == null || randomCategory.emojis.Count < 2)
        {
            Debug.LogError("Selected category has insufficient emojis");
            return null;
        }

        // Select two different random emojis from the same category
        EmojiInfo firstEmoji = randomCategory.emojis[UnityEngine.Random.Range(0, randomCategory.emojis.Count)];
        EmojiInfo secondEmoji;

        do
        {
            secondEmoji = randomCategory.emojis[UnityEngine.Random.Range(0, randomCategory.emojis.Count)];
        } while (secondEmoji == firstEmoji);

        currentPlaythroughEmojiDescriptions.Add(firstEmoji.description);
        currentPlaythroughEmojiDescriptions.Add(secondEmoji.description);

        currentPlaythroughEmojis.Add(firstEmoji);
        currentPlaythroughEmojis.Add(secondEmoji);


        return new EmojiInfo[] { firstEmoji, secondEmoji };
    }

    public int GetCurrentEmojiGroupIndex()
    {
        return m_emojiGroupIndex;
    }

    public bool IsEmojiInCurrentPlaythrough(string description)
    {
        return currentPlaythroughEmojiDescriptions.Contains(description);
    }

    [System.Serializable]
    public class FontData
    {
        public string name;
        public string rarity;
        public List<CharacterData> characters;
    }

    [System.Serializable]
    public class CharacterData
    {
        public string character;
        public bool collected;
    }

    [System.Serializable]
    public class FontCollection
    {
        public List<FontData> fonts;
    }

    public List<char> collectedLetters = new List<char>();


    public void SelectRandomLetter()
    {
        // Ensure the current word has more than one letter
        if (m_sCurWord.Length <= 1) return;

        // Exclude the first letter and select a random letter
        int randomIndex = UnityEngine.Random.Range(1, m_sCurWord.Length);
        char selectedLetter = m_sCurWord[randomIndex];

        // Convert the selected letter to a string
        string selectedLetterStr = selectedLetter.ToString();

        if (fontsCollection != null && fontsCollection.fonts != null && fontsCollection.fonts.Count > 0)
        {
            foreach (var font in fontsCollection.fonts)
            {
                Debug.Log($"Font2: {font.name}, Rarity2: {font.rarity}");
                // foreach (KeyValuePair<char, bool> character in font.characters)
                // {
                //     Debug.Log($"Character: {character.Key}, Collected: {character.Value}");
                // }
            }
        }

        // Check and select a font for the letter
        // foreach (var font in fontsCollection.fonts)
        // {
        //     if (font.characters.ContainsKey(selectedLetterStr) && !font.characters[selectedLetterStr])
        //     {
        //         font.characters[selectedLetterStr] = true; // Mark as collected
        //         collectedLetters.Add(selectedLetter);
        //         PlayerPrefs.SetString("CollectedLetters", new string(collectedLetters.ToArray())); // Store in PlayerPrefs
        //                                                                                            // UpdateUIWithCollectedLetter(selectedLetter, font.name); // Update UI
        //         break;
        //     }
        // }
    }

    // Implement UpdateUIWithCollectedLetter as needed for UI updates



}
