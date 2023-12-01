using UnityEngine;
using UnityEngine.Tilemaps;

public class EmojiTileReplacer : MonoBehaviour
{
    public Tilemap emojiTilemap; // Assign this in the Unity Editor
    public Tile placeholderTile; // Assign this in the Unity Editor
    public EmojiGameController emojiGameController; // Assign this in the Unity Editor
    public EmojiSpriteMapper emojiSpriteMapper; // Assign this in the Unity Editor
    private EmojiDataLoader emojiDataLoader;
    private EmojiDataWrapper emojiData;

    void Start()
    {
        emojiDataLoader = FindObjectOfType<EmojiDataLoader>();

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
        //ReplaceMarkersWithEmojis();
    }



    private bool IsAdjacentTilePlaceholder(Vector3Int position)
    {
        // Check adjacent tiles (left, right, up, down)
        Vector3Int[] adjacentPositions = new Vector3Int[]
        {
            position + Vector3Int.left,
            position + Vector3Int.right,
            position + Vector3Int.up,
            position + Vector3Int.down
        };

        foreach (var adjacentPosition in adjacentPositions)
        {
            if (emojiTilemap.GetTile(adjacentPosition) == placeholderTile)
            {
                return true;
            }
        }

        return false;
    }

    private void ReplaceTile(Vector3Int position, Sprite sprite)
    {
        Tile tile = CreateTileFromSprite(sprite);
        emojiTilemap.SetTile(position, tile);
    }

    private void ReplaceAdjacentTile(Vector3Int position, Sprite sprite)
    {
        Vector3Int[] adjacentPositions = new Vector3Int[]
        {
            position + Vector3Int.left,
            position + Vector3Int.right,
            position + Vector3Int.up,
            position + Vector3Int.down
        };

        foreach (var adjacentPosition in adjacentPositions)
        {
            if (emojiTilemap.GetTile(adjacentPosition) == placeholderTile)
            {
                ReplaceTile(adjacentPosition, sprite);
                return; // Replace only one adjacent tile and exit
            }
        }
    }

    // Method to create a tile from a given sprite
    private Tile CreateTileFromSprite(Sprite sprite)
    {
        Tile tile = ScriptableObject.CreateInstance<Tile>();
        tile.sprite = sprite;
        return tile;
    }
}
