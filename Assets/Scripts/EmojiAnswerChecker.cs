using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EmojiAnswerChecker : MonoBehaviour
{
    private List<string> possibleAnswers;

    //private EmojiDataLoader dataLoader;

    public void SetPossibleAnswers(EmojiDataLoader dataLoader, int groupIndex)
    {
        EmojiDataWrapper emojiData = dataLoader.GetEmojiData();

        possibleAnswers = new List<string>();
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

        // Iterate through the possible answers
        foreach (string answer in possibleAnswers)
        {
            // Convert the answer to lowercase
            string answerLower = answer.ToLower();

            // Check if player input matches the answer
            if (inputLower == answerLower)
            {
                return true; // Match found
            }
        }

        return false; // No match found
    }
}
