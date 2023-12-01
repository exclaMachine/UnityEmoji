using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.UI;
using System.Linq;
using System.Collections.Generic;

public class EmojiGameController : MonoBehaviour
{
    public Tilemap emojiTilemap; // Assign in the inspector
    public Tile placeholderTile; // Assign in the inspector
    //public Tile placeholderTile;
    public InputField playerInputField; // Assign in the inspector
    private EmojiDataLoader emojiDataLoader;

    private EmojiSpriteMapper emojiSpriteMapper;
    private EmojiDataWrapper emojiData;

    //EmojiDataLoader dataLoader = ...;


    void Start()
    {

        emojiDataLoader = FindObjectOfType<EmojiDataLoader>();
        GameObject emojiManager = GameObject.Find("EmojiManager");
        if (emojiManager != null)
        {
            emojiSpriteMapper = emojiManager.GetComponent<EmojiSpriteMapper>();
        }

        // Access emoji data
        if (emojiDataLoader != null)
        {
            emojiData = emojiDataLoader.GetEmojiData();
            if (emojiData != null && emojiData.groups.Count > 0)
            {
                int randomGroupIndex = UnityEngine.Random.Range(0, emojiData.groups.Count);
                EmojiGroup randomGroup = emojiData.groups[randomGroupIndex];

                // Use randomGroup for further logic
            }
            else
            {
                Debug.LogError("No groups found in emoji data.");
            }
        }
        else
        {
            Debug.LogError("EmojiDataLoader component not found in the scene.");
        }
        PlaceRandomEmojisOnMap();
    }

    List<Vector3Int> FindPlaceholderTilePositions(Tilemap tilemap, Tile placeholderTile)
    {
        List<Vector3Int> placeholderPositions = new List<Vector3Int>();
        BoundsInt bounds = tilemap.cellBounds;
        TileBase tile;

        for (int x = bounds.xMin; x < bounds.xMax; x++)
        {
            for (int y = bounds.yMin; y < bounds.yMax; y++)
            {
                Vector3Int position = new Vector3Int(x, y, 0);
                tile = tilemap.GetTile(position);
                if (tile == placeholderTile)
                {
                    placeholderPositions.Add(position);
                }
            }
        }

        return placeholderPositions;
    }


    void PlaceRandomEmojisOnMap()
    {
        List<Vector3Int> placeholderPositions = FindPlaceholderTilePositions(emojiTilemap, placeholderTile);

        // Make sure you have at least two placeholder positions
        if (placeholderPositions.Count >= 2)
        {
            EmojiInfo[] emojis = emojiDataLoader.GetRandomEmojisFromSameCategory();

            if (emojis != null && emojis.Length == 2)
            {
                // Assuming you take the first two placeholder positions
                Vector3Int position1 = placeholderPositions[0];
                Vector3Int position2 = placeholderPositions[1];
                Debug.Log($"pos 1: {position1}");
                Debug.Log($"pos 2: {position2}");
                Sprite sprite1 = emojiSpriteMapper.GetSprite(emojis[0].code);
                Sprite sprite2 = emojiSpriteMapper.GetSprite(emojis[1].code);
                Debug.Log($"sprite 2: {sprite2}");
                Debug.Log($"sprite 1: {sprite1}");
                CreateEmojiGameObject(position1, sprite1);
                CreateEmojiGameObject(position2, sprite2);
            }
        }
    }

    void CreateEmojiGameObject(Vector3Int gridPosition, Sprite sprite)
    {
        GameObject emojiObj = new GameObject("Emoji");
        Debug.Log($"obj 1: {emojiObj}");

        Vector3 worldPosition = emojiTilemap.CellToWorld(gridPosition);
        emojiObj.transform.position = worldPosition;

        SpriteRenderer renderer = emojiObj.AddComponent<SpriteRenderer>();
        renderer.sprite = sprite;

        // Set the sorting layer and order
        renderer.sortingLayerName = "Foreground"; // Name of the sorting layer
        renderer.sortingOrder = 1; // Higher number means it will be rendered on top
    }



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
