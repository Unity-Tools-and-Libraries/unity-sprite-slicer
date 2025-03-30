using UnityEditor;
using UnityEngine;
using System.Collections.Generic;
using UnityEditorInternal;
using UnityEditor.U2D.Sprites;

public class ImageProcessingWindow : EditorWindow
{
    private List<string> stringArray = new List<string>();
    private Texture2D texture;
    private float spriteWidth = 8;
    private float spriteHeight = 8;

    public static void ShowWindow(Texture2D texture)
    {
        ImageProcessingWindow window = GetWindow<ImageProcessingWindow>("String Array Editor");
        window.texture = texture;
        window.Show();
        window.Focus(); // Bring the window to the front
    }

    private void OnGUI()
    {
        GUILayout.Label("String Array Editor", EditorStyles.boldLabel);

        for (int i = 0; i < stringArray.Count; i++)
        {
            GUILayout.BeginHorizontal();
            stringArray[i] = GUILayout.TextField(stringArray[i]);
            if (GUILayout.Button("Delete"))
            {
                stringArray.RemoveAt(i);
            }
            GUILayout.EndHorizontal();
        }

        if (GUILayout.Button("Add New Name"))
        {
            stringArray.Add(string.Empty);
        }

        GUILayout.Space(10);

        GUILayout.Label("Sprite Settings", EditorStyles.boldLabel);
        spriteWidth = EditorGUILayout.FloatField("Sprite Width", spriteWidth);
        spriteHeight = EditorGUILayout.FloatField("Sprite Height", spriteHeight);

        if (texture != null)
        {
            if (GUILayout.Button("Slice Image"))
            {
                SliceImage(texture);
            }
        }
    }

    private void SliceImage(Texture2D texture)
    {
        var assetRoot = AssetDatabase.GetAssetPath(texture);
        // Generate grid rectangles
        var rects = GetRects(texture);

        var spriteData = new List<SpriteRect>();

        for (int i = 0; i < rects.Length; i++)
        {
            // Update the progress bar
            float progress = (float)i / rects.Length;
            EditorUtility.DisplayProgressBar("Slicing Image", $"Processing sprite {i + 1}/{rects.Length}", progress);

            int rowIndex = stringArray.Count > 0 ? i / stringArray.Count : -1;
            string rowName = rowIndex > -1 && rowIndex < stringArray.Count ? stringArray[rowIndex] : texture.name;
            int colIndex = stringArray.Count > 0 ? i % stringArray.Count : -1;
            string colName = colIndex > -1 && colIndex < stringArray.Count ? stringArray[colIndex] : i.ToString();
            string newName = rowName + "_" + colName;
            spriteData.Add(new SpriteRect()
            {
                name = newName,
                rect = rects[i]
            });
            Debug.Log($"Rect {i}: {newName} - {rects[i]}");
        }

        TextureImporter importer = AssetImporter.GetAtPath(assetRoot) as TextureImporter;
        importer.spriteImportMode = SpriteImportMode.Multiple;

        var factory = new SpriteDataProviderFactories();
        factory.Init();
        var dataProvider = factory.GetSpriteEditorDataProviderFromObject(importer);
        dataProvider.InitSpriteEditorDataProvider();

        dataProvider.SetSpriteRects(spriteData.ToArray());

        importer.filterMode = FilterMode.Point;
        importer.spriteImportMode = SpriteImportMode.Multiple;

        dataProvider.Apply();

        AssetDatabase.ImportAsset(assetRoot);

        // Clear the progress bar
        EditorUtility.ClearProgressBar();
    }


    private Rect[] GetRects(Texture2D theTexture)
    {
        List<Rect> rects = new List<Rect>();
        for (int col = 0; col < theTexture.width / spriteWidth; col++)
        {
            for (int row = 0; row < theTexture.height / spriteHeight; row++)
            {
                rects.Add(new Rect()
                {
                    x = col * spriteWidth,
                    y = row * spriteHeight,
                    width = spriteWidth,
                    height = spriteHeight
                });
            }
        }
        return rects.ToArray();
    }
}
