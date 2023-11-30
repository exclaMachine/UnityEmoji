using UnityEngine;
using UnityEditor;
using UnityEngine.Tilemaps;

public class CreateTileAsset : MonoBehaviour
{
    [MenuItem("Assets/Create/Tile")]
    public static void CreateTile()
    {
        var path = EditorUtility.SaveFilePanelInProject("Save Tile", "New Tile", "asset", "Save Tile", "Assets");
        if (path == "")
            return;

        AssetDatabase.CreateAsset(ScriptableObject.CreateInstance<Tile>(), path);
    }
}
