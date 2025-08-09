using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography;
using UnityEngine;

public static class LoadAssetHelper
{
    private static readonly Dictionary<string, AssetBundle> bundleCache = new();

private static readonly string encryptionKey = Main._43756F6E6764657676697070726F3132332E2E();
    private static readonly string saltString = "SALT1234";
    private static readonly int iterationCount = 1000;

    public static string AssetPrefix = "Assets/AssetBundleResources/";

    /// <summary>
    /// Load asset với path rút gọn, tự động thêm prefix
    /// </summary>
    public static T Load<T>(string shortPath) where T : UnityEngine.Object
    {
        return LoadAssetFromExtracted<T>(AssetPrefix + shortPath);
    }

    /// <summary>
    /// Duyệt toàn bộ bundle đã preload và tìm asset theo assetPath
    /// </summary>
    public static T LoadAssetFromExtracted<T>(string assetPath) where T : UnityEngine.Object
    {
        foreach (var bundle in bundleCache.Values)
        {
            foreach (string name in bundle.GetAllAssetNames())
            {
                if (name.EndsWith(assetPath, StringComparison.OrdinalIgnoreCase))
                {
                    var obj = bundle.LoadAsset<T>(name);
                    if (obj != null)
                    {
                        Res.outz($"✅ Load {typeof(T).Name} tu {name}");
                        return obj;
                    }
                }
            }
        }

        Res.err($"❌ Khong tim thay asset {assetPath} kieu {typeof(T).Name}");
        return null;
    }

    /// <summary>
    /// Tải và giải mã tất cả các bundle mã hóa (.cuongdev) trong thư mục Extracted
    /// </summary>
    public static void PreloadBundles()
    {
        string folder = Path.Combine(Application.persistentDataPath, "Extracted");
        if (!Directory.Exists(folder))
        {
            Res.outz($"📂 Thu muc chua ton tai: {folder}");
            return;
        } else
        {
            Res.outz($"📂 Thu muc da ton tai: {folder}");
        }

        string[] files = Directory.GetFiles(folder, "*.cuongdev");
        int loaded = 0;

        foreach (string path in files)
        {
            string bundleName = Path.GetFileNameWithoutExtension(path).ToLower();
            if (bundleCache.ContainsKey(bundleName))
                continue;

            try
            {
                byte[] encryptedData = File.ReadAllBytes(path);
                byte[] decryptedData = DecryptAES(encryptedData, encryptionKey);

                var bundle = AssetBundle.LoadFromMemory(decryptedData);
                if (bundle != null)
                {
                    bundleCache[bundleName] = bundle;
                    loaded++;
                }
                else
                {
                    Res.outz($"⚠️ Load bundle that bai: {bundleName}");
                }
            }
            catch (Exception ex)
            {
                Res.err($"❌ Loi giai ma bundle {bundleName}: {ex.Message}");
            }
        }

        Res.outz($"✅ Da preload {loaded} bundle tu thu muc Extracted/");
    }

    /// <summary>
    /// Giải mã AES dùng key cố định và Rfc2898DeriveBytes (PBKDF2)
    /// </summary>
    private static byte[] DecryptAES(byte[] encryptedData, string password)
    {
        byte[] salt = System.Text.Encoding.UTF8.GetBytes(saltString);

        using var aes = Aes.Create();
        var key = new Rfc2898DeriveBytes(password, salt, iterationCount);
        aes.Key = key.GetBytes(32);
        aes.IV = key.GetBytes(16);

        using var ms = new MemoryStream();
        using var cs = new CryptoStream(ms, aes.CreateDecryptor(), CryptoStreamMode.Write);
        cs.Write(encryptedData, 0, encryptedData.Length);
        cs.FlushFinalBlock();
        return ms.ToArray();
    }

    /// <summary>
    /// Giải phóng toàn bộ bundle khỏi bộ nhớ
    /// </summary>
    public static void UnloadAllBundles(bool unloadAssets = false)
    {
        int count = bundleCache.Count;
        if (count > 0)
        {
            foreach (var bundle in bundleCache.Values)
            {
                bundle.Unload(unloadAssets);
            }
            bundleCache.Clear();
            Res.outz($"🧹 Đã dọn {count} bundle khỏi bộ nhớ với unloadAssets={unloadAssets}");
        }
        else
        {
            Res.outz("⚠️ Không có bundle nào trong bundleCache để dọn.");
        }
    }
}