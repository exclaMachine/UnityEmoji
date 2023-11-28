using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.UI;

public class EmojiGameController : MonoBehaviour
{
    public Tilemap emojiTilemap; // Assign in the inspector
    public Tile blankTile; // Assign in the inspector
    public InputField playerInputField; // Assign in the inspector
    private EmojiDataLoader emojiDataLoader;

    private EmojiSpriteMapper emojiSpriteMapper;

    void Start()
    {

        emojiDataLoader = FindObjectOfType<EmojiDataLoader>();
        GameObject emojiManager = GameObject.Find("EmojiManager");
        if (emojiManager != null)
        {
            emojiSpriteMapper = emojiManager.GetComponent<EmojiSpriteMapper>();
        }

        PlaceRandomEmojisOnMap();
    }


    void PlaceRandomEmojisOnMap()
    {
        EmojiInfo[] emojis = emojiDataLoader.GetRandomEmojisFromSameCategory();
        if (emojis != null && emojis.Length == 2)
        {
            Vector3Int position1 = new Vector3Int(0, 0, 0); // Your placement logic
            Vector3Int position2 = new Vector3Int(1, 0, 0); // Your placement logic



            // Get Sprites for emojis
            Sprite sprite1 = emojiSpriteMapper.GetSprite(emojis[0].code);
            Sprite sprite2 = emojiSpriteMapper.GetSprite(emojis[1].code);

            Debug.Log($"sprite 1: {sprite2}");

            // Place emojis using the sprites
            // You might use GameObjects with SpriteRenderer to display these sprites
            // Example:
            CreateEmojiGameObject(position1, sprite1);
            CreateEmojiGameObject(position2, sprite2);
        }
    }

    void CreateEmojiGameObject(Vector3Int position, Sprite sprite)
    {
        GameObject emojiObj = new GameObject("Emoji");
        emojiObj.transform.position = position;
        SpriteRenderer renderer = emojiObj.AddComponent<SpriteRenderer>();
        renderer.sprite = sprite;
    }

    // ...


    // Tile GetEmojiTile(EmojiInfo emoji)
    // {
    //     // Logic to get the corresponding Tile for the given emoji
    // }

    public void OnPlayerInputSubmit()
    {
        string playerInput = playerInputField.text;
        // Compare player input with correct answer and provide feedback
    }
}
