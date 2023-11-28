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


public class EmojiDataLoader : MonoBehaviour
{
    public TextAsset emojiJson; // Assign this in the Unity Editor

    //private EmojiData emojiData;
    private EmojiDataWrapper emojiData;
    void Start()
    {
        LoadEmojiData();
        TestRandomEmojiSelection();
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
        if (emojiData == null || emojiData.groups == null || emojiData.groups.Count == 0)
        {
            Debug.LogError("Emoji data is not loaded or has no groups");
            return null;
        }

        // Select a random group
        var randomGroup = emojiData.groups[UnityEngine.Random.Range(0, emojiData.groups.Count)];
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

        return new EmojiInfo[] { firstEmoji, secondEmoji };
    }


}
