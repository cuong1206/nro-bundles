#if UNITY_EDITOR
using UnityEngine;
using UnityEditor;
using System.IO;

public static class TextureToSpriteConverter
{
    [MenuItem("Assets/Convert All Textures To Sprite (Recursive)", false, 1000)]
    public static void ConvertSelectedFolder()
    {
        string folderPath = GetSelectedFolderPath();

        if (string.IsNullOrEmpty(folderPath))
        {
            Debug.LogError("❌ Vui lòng chọn một thư mục trong Project.");
            return;
        }

        string[] guids = AssetDatabase.FindAssets("t:Texture2D", new[] { folderPath });
        int convertedCount = 0;

        foreach (string guid in guids)
        {
            string assetPath = AssetDatabase.GUIDToAssetPath(guid);
            TextureImporter importer = AssetImporter.GetAtPath(assetPath) as TextureImporter;

            if (importer != null && importer.textureType != TextureImporterType.Sprite)
            {
                importer.textureType = TextureImporterType.Sprite;
                importer.spriteImportMode = SpriteImportMode.Single;
                importer.SaveAndReimport();

                Debug.Log($"✅ Đã chuyển: {assetPath}");
                convertedCount++;
            }
        }

        Debug.Log($"🎉 Đã chuyển {convertedCount} ảnh sang Sprite trong thư mục: {folderPath}");
    }

    [MenuItem("Assets/Convert All Textures To Sprite (Recursive)", true)]
    private static bool ValidateConvertSelectedFolder()
    {
        return !string.IsNullOrEmpty(GetSelectedFolderPath());
    }

    private static string GetSelectedFolderPath()
    {
        var obj = Selection.activeObject;
        if (obj == null) return null;

        string path = AssetDatabase.GetAssetPath(obj);
        if (!AssetDatabase.IsValidFolder(path))
            return null;

        return path;
    }
}
#endif
