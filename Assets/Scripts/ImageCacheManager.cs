using UnityEngine;
using System.IO;
using System;

public class ImageCacheManager
{
    public static readonly string CacheDir = Path.Combine(Application.persistentDataPath, "ImageCache");

    /// <summary>
    /// Lưu ảnh từ đường dẫn AssetBundle hoặc byte[] vào persistentDataPath.
    /// </summary>
    public static void SaveImage(string assetPath, string cacheFileName)
    {
        if (!Directory.Exists(CacheDir))
        {
            Directory.CreateDirectory(CacheDir);
        }

        string fullPath = Path.Combine(CacheDir, cacheFileName + ".png");
        try
        {
            Texture2D texture = LoadAssetHelper.Load<Texture2D>(assetPath);
            if (texture != null)
            {
                byte[] bytes = texture.EncodeToPNG();
                File.WriteAllBytes(fullPath, bytes);
                Debug.Log($"✅ Đã lưu ảnh {assetPath} vào {fullPath}");
                UnityEngine.Object.Destroy(texture); // Giải phóng texture ngay
            }
            else
            {
                Debug.LogError($"❌ Không thể nạp {assetPath} để lưu.");
            }
        }
        catch (Exception ex)
        {
            Debug.LogError($"❌ Lỗi lưu ảnh {assetPath}: {ex.Message}");
        }
    }

    /// <summary>
    /// Nạp ảnh từ persistentDataPath vào Texture2D tạm thời.
    /// </summary>
    public static Texture2D LoadImageFromCache(string cacheFileName)
    {
        string fullPath = Path.Combine(CacheDir, cacheFileName + ".png");
        if (File.Exists(fullPath))
        {
            byte[] bytes = File.ReadAllBytes(fullPath);
            Texture2D texture = new Texture2D(2, 2); // Kích thước tạm, sẽ resize
            if (texture.LoadImage(bytes))
            {
                Debug.Log($"✅ Đã nạp ảnh từ {fullPath}");
                return texture;
            }
            UnityEngine.Object.Destroy(texture);
        }
        Debug.LogError($"❌ Không tìm thấy file {fullPath}");
        return null;
    }

    /// <summary>
    /// Giải phóng ảnh sau khi dùng (gọi thủ công).
    /// </summary>
    public static void ReleaseImage(Texture2D texture)
    {
        if (texture != null)
        {
            UnityEngine.Object.Destroy(texture);
            Resources.UnloadUnusedAssets();
            Debug.Log("🧹 Đã giải phóng Texture2D.");
        }
    }
}