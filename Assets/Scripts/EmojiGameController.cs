using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.UI;
using System.Linq;
using System.Collections.Generic;
using TMPro;


public class EmojiGameController : MonoBehaviour
{
    public Tilemap emojiTilemap; // Assign in the inspector
    public Tile placeholderTile; // Assign in the inspector
    public Sprite blankEmojiSprite;
    public TMP_InputField tmpInputField;
    private EmojiDataLoader emojiDataLoader;
    private List<string> possibleAnswers;

    private EmojiSpriteMapper emojiSpriteMapper;
    private EmojiDataWrapper emojiData;

    public TextMeshProUGUI emojiNameText; // Assign this in the Unity Editor

    public EmojiAnswerChecker m_answerChecker;


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
        const int groupSize = 3; // Number of tiles in each group
        const int totalGroups = 5; // Total number of groups
        int placedGroups = 0;

        for (int i = 0; i < placeholderPositions.Count && placedGroups < totalGroups; i += groupSize)
        {
            // Check if there are enough placeholders for the current group
            if (i + groupSize <= placeholderPositions.Count)
            {
                PlaceRandomEmojiGroup(placeholderPositions.GetRange(i, groupSize));
                Debug.Log("Invalid number of positions for emoji group");
                placedGroups++;
            }
        }
    }

    void PlaceRandomEmojiGroup(List<Vector3Int> positions)
    {
        if (positions.Count != 3)
        {
            Debug.LogError("Invalid number of positions for emoji group");
            return;
        }

        // Place two random emojis
        EmojiInfo[] emojis = emojiDataLoader.GetRandomEmojisFromSameCategory();
        if (emojis != null && emojis.Length == 2)
        {
            Debug.Log($"Emoji Name1: {emojis[0].description}, Emoji Name2: {emojis[1].description}");

            CreateEmojiGameObject(positions[0], emojiSpriteMapper.GetSprite(emojis[0].code), emojis[0].description);
            CreateEmojiGameObject(positions[1], emojiSpriteMapper.GetSprite(emojis[1].code), emojis[1].description);
        }

        // Place a blank tile at the third position
        m_answerChecker = CreateBlankEmojiGameObject(positions[2]);
    }

    void CreateEmojiGameObject(Vector3Int gridPosition, Sprite sprite, string emojiName)
    {
        GameObject emojiObj = new GameObject("Emoji");
        // Calculate world position from grid position
        Vector3 worldPosition = emojiTilemap.CellToWorld(gridPosition);

        // Adjust the position by 1 pixel up and to the right
        // Assuming your tilemap is using 8px cells, and Unity units correspond to pixels
        float adjustment = 1.0f / 24.0f; // 1 pixel in terms of Unity units
        worldPosition.x += adjustment;
        worldPosition.y += adjustment;

        emojiObj.transform.position = worldPosition;
        SpriteRenderer renderer = emojiObj.AddComponent<SpriteRenderer>();
        renderer.sprite = sprite;

        // Set the sorting layer and order
        renderer.sortingLayerName = "Foreground";
        renderer.sortingOrder = 1;

        BoxCollider2D collider = emojiObj.AddComponent<BoxCollider2D>();
        collider.isTrigger = true;

        // Increase the size of the collider
        collider.size = new Vector2(collider.size.x * 1.2f, collider.size.y * 1.2f); // Adjust the multiplier as needed


        EmojiInteraction interaction = emojiObj.AddComponent<EmojiInteraction>();
        interaction.onPlayerEnter += () => ShowEmojiName(emojiName, worldPosition);
        interaction.onPlayerExit += HideEmojiName;

    }


    EmojiAnswerChecker CreateBlankEmojiGameObject(Vector3Int gridPosition)
    {
        // Create a GameObject for the blank emoji
        GameObject blankEmojiObj = new GameObject("BlankEmoji");
        blankEmojiObj.transform.position = gridPosition;
        SpriteRenderer renderer = blankEmojiObj.AddComponent<SpriteRenderer>();

        // Calculate world position from grid position
        Vector3 worldPosition = emojiTilemap.CellToWorld(gridPosition);

        // Adjust the position by 1 pixel up and to the right
        // Assuming your tilemap is using 8px cells, and Unity units correspond to pixels
        float adjustment = 1.0f / 24.0f; // 1 pixel in terms of Unity units
        worldPosition.x += adjustment;
        worldPosition.y += adjustment;

        blankEmojiObj.transform.position = worldPosition;

        // Assign the blank sprite or color to renderer.sprite
        // Example: renderer.sprite = blankEmojiSprite;
        renderer.sprite = blankEmojiSprite;
        //renderer.color = Color.blue;

        // Set sorting layer and order
        renderer.sortingLayerName = "Foreground";
        renderer.sortingOrder = 1;

        //Add Collider and Interaction
        BoxCollider2D collider = blankEmojiObj.AddComponent<BoxCollider2D>();
        collider.isTrigger = true;
        collider.size = new Vector2(collider.size.x * 1.5f, collider.size.y * 1.5f); // Adjust the multiplier as needed

        // Attach answer checker and set possible answers
        EmojiAnswerChecker answerChecker = blankEmojiObj.AddComponent<EmojiAnswerChecker>();
        int groupIndex = emojiDataLoader.GetCurrentEmojiGroupIndex();
        answerChecker.SetPossibleAnswers(emojiDataLoader, groupIndex);

        EmojiInteraction interaction = blankEmojiObj.AddComponent<EmojiInteraction>();
        interaction.onPlayerEnter += () => ShowInputFieldAtPosition(worldPosition);
        //interaction.onPlayerExit += HideInputField;

        return answerChecker;
    }

    public void ShowEmojiName(string name, Vector3 emojiPosition)
    {
        emojiNameText.text = name;

        // Position the text object above the emoji
        Vector3 textPosition = Camera.main.WorldToScreenPoint(emojiPosition + new Vector3(0, 0.5f, 0)); // Adjust the offset as needed
        emojiNameText.transform.position = textPosition;

        emojiNameText.gameObject.SetActive(true);
    }

    public void HideEmojiName()
    {
        emojiNameText.gameObject.SetActive(false);
    }

    private void ShowInputFieldAtPosition(Vector3 position)
    {
        // Position the input field above the blank emoji
        Vector3 inputFieldPosition = Camera.main.WorldToScreenPoint(position + new Vector3(0, 0.5f, 0)); // Adjust the offset as needed
        tmpInputField.transform.position = inputFieldPosition;
        tmpInputField.gameObject.SetActive(true);
    }

    private void HideInputField()
    {
        tmpInputField.gameObject.SetActive(false);
    }


    public void CheckInput(string input)
    {
        if (m_answerChecker != null && m_answerChecker.CheckAnswer(input))
        {
            Debug.Log("Correct Answer!");
            // Implement logic for correct answer, e.g., make emojis disappear
        }
        else
        {
            Debug.Log("Incorrect Answer");
            // Implement logic for incorrect answer
        }
    }

    private bool IsCloseEnough(string input, string description)
    {
        // Implement fuzzy matching logic here
        // For now, it's a simple substring check
        return description.Contains(input);
    }

    private void OnCorrectAnswer()
    {
        // Handle correct answer logic
        Debug.Log("Correct Answer!");
        // TODO: Implement what happens on correct answer, e.g., reveal emoji, award points
    }

    private void OnIncorrectAnswer()
    {
        // Handle incorrect answer logic
        Debug.Log("Incorrect Answer!");
        // TODO: Implement what happens on incorrect answer
    }

}
