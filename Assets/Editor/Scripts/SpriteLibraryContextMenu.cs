using UnityEditor;
using UnityEngine;
using System;
using System.Reflection;
using UnityEditor.Sprites;
using UnityEditorInternal;
using UnityEditor.U2D;

public static class SpriteLibraryContextMenu
{
    [MenuItem("Assets/Custom Actions/Process Image", true, 0)]
    private static bool ValidateProcessImage()
    {
        return Selection.activeObject is Texture2D;
    }

    [MenuItem("Assets/Custom Actions/Process Image", false, 0)]
    private static void ProcessImage()
    {
        var assetPath = AssetDatabase.GetAssetPath(Selection.activeObject);
        if (assetPath != null)
        {
            // Open the built-in sprite editor using reflection
            Type spriteEditorWindowType = typeof(EditorWindow).Assembly.GetType("UnityEditor.SpriteUtilityWindow");
            MethodInfo getWindowMethod = spriteEditorWindowType.GetMethod("ShowSpriteEditorWindow", BindingFlags.NonPublic | BindingFlags.Static, null, new Type[] { typeof(Texture2D) }, null);
            getWindowMethod.Invoke(spriteEditorWindowType, new object[] { Selection.activeObject as Texture2D });

            // Open the custom window
            ImageProcessingWindow.ShowWindow(Selection.activeObject as Texture2D);
        }
    }
}
