using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EmojiAnswerChecker : MonoBehaviour
{
    private List<string> possibleAnswers;
    private List<EmojiInfo> possibleEmojis;
    private int m_matchedEmojiIndex = -1; // -1 indicates no match

    //private EmojiDataLoader dataLoader;

    public void SetPossibleAnswers(EmojiDataLoader dataLoader, int groupIndex)
    {
        EmojiDataWrapper emojiData = dataLoader.GetEmojiData();

        possibleAnswers = new List<string>();
        possibleEmojis = new List<EmojiInfo>();
        var group = emojiData.groups[groupIndex];
        //var group = dataLoader.GetEmojiData().groups[groupIndex];

        // Add descriptions of all emojis in the group except the selected ones
        foreach (var subgroup in group.subgroups)
        {
            foreach (var category in subgroup.categories)
            {
                foreach (var emoji in category.emojis)
                {
                    // Assuming emoji.description is the string you want to compare
                    if (dataLoader.IsEmojiInCurrentPlaythrough(emoji.description) == false)
                    {
                        possibleAnswers.Add(emoji.description);
                        possibleEmojis.Add(emoji);
                    }
                }
            }
        }
    }

    // Method to check the player's input against possible answers
    public bool CheckAnswer(string playerInput)
    {
        // Convert playerInput to lowercase for case-insensitive comparison
        string inputLower = playerInput.ToLower();

        for (int i = 0; i < possibleAnswers.Count; i++)
        {
            string answerLower = possibleAnswers[i].ToLower();
            if (inputLower == answerLower)
            {
                m_matchedEmojiIndex = i; // Store the index of the matched emoji
                return true; // Match found
            }
        }

        return false; // No match found
    }
    public EmojiInfo GetMatchedEmojiInfo()
    {
        if (m_matchedEmojiIndex >= 0 && m_matchedEmojiIndex < possibleEmojis.Count)
        {
            return possibleEmojis[m_matchedEmojiIndex];
        }

        return null; // No matched emoji or invalid index
    }
}
