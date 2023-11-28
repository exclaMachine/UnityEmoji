using UnityEngine;
using System.Collections.Generic;

public class EmojiSpriteMapper : MonoBehaviour
{
    private Dictionary<string, Sprite> emojiSprites;

    void Awake()
    {
        emojiSprites = new Dictionary<string, Sprite>();

        // Assuming you have a folder named "EmojiImages" in your Assets directory
        Sprite[] sprites = Resources.LoadAll<Sprite>("Emoji_Images");
        foreach (Sprite sprite in sprites)
        {
            string code = GetCodeFromFilename(sprite.name);
            Debug.Log($"code {code}");
            if (!string.IsNullOrEmpty(code))
            {
                emojiSprites[code] = sprite;
            }
        }
    }

    public Sprite GetSprite(string code)
    {
        if (emojiSprites.ContainsKey(code))
        {
            return emojiSprites[code];
        }
        return null;
    }

    private string GetCodeFromFilename(string filename)
    {
        Debug.Log($"filename {filename}");
        const string prefix = "emoji_u";

        if (filename.StartsWith(prefix))
        {
            int startIndex = prefix.Length;
            int endIndex = filename.Length;

            // Check if filename contains ".png", adjust endIndex if so
            if (filename.EndsWith(".png"))
            {
                endIndex = filename.LastIndexOf(".png");
            }

            if (endIndex > startIndex)
            {
                string code = filename.Substring(startIndex, endIndex - startIndex).ToUpper();
                return code;
            }
        }
        return null;
    }

}
