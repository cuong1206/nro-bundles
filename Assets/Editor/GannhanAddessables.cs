using UnityEditor;
using UnityEditor.AddressableAssets;
using UnityEditor.AddressableAssets.Settings;
using UnityEditor.AddressableAssets.Settings.GroupSchemas;
using UnityEngine;
using System.IO;

public static class MarkAllAssetsAsAddressable
{
    [MenuItem("Assets/Đặt tất cả asset là Addressable theo Group thư mục", false, 100)]
    private static void MarkAssets()
    {
        string folderPath = GetSelectedFolderPath();
        if (string.IsNullOrEmpty(folderPath))
        {
            Debug.LogWarning("❌ Không tìm thấy thư mục.");
            return;
        }

        string[] guids = AssetDatabase.FindAssets("", new[] { folderPath }); // lấy mọi loại asset

        if (guids.Length == 0)
        {
            Debug.Log("📂 Không tìm thấy asset nào trong thư mục.");
            return;
        }

        var settings = AddressableAssetSettingsDefaultObject.Settings;
        if (settings == null)
        {
            Debug.LogError("❌ Không tìm thấy AddressableAssetSettings.");
            return;
        }

        string groupName = Path.GetFileName(folderPath.TrimEnd('/')).Trim();

        AddressableAssetGroup group = settings.FindGroup(groupName);
        if (group == null)
        {
            group = settings.CreateGroup(groupName, false, false, true, null,
                typeof(BundledAssetGroupSchema), typeof(ContentUpdateGroupSchema));
            var schema = group.GetSchema<BundledAssetGroupSchema>();
            schema.BuildPath.SetVariableByName(settings, AddressableAssetSettings.kLocalBuildPath);
            schema.LoadPath.SetVariableByName(settings, AddressableAssetSettings.kLocalLoadPath);
        }

        int count = 0;

        foreach (string guid in guids)
        {
            string assetPath = AssetDatabase.GUIDToAssetPath(guid);

            if (string.IsNullOrEmpty(assetPath)) continue;

            if (Directory.Exists(assetPath)) continue; // bỏ qua folder

            // Bỏ qua meta, cs, dll, asmdef, json, shader nếu muốn
            string extension = Path.GetExtension(assetPath).ToLower();
            if (extension == ".cs" || extension == ".dll" || extension == ".meta" || extension == ".asmdef")
                continue;

            AddressableAssetEntry entry = settings.CreateOrMoveEntry(guid, group);
            if (entry != null)
            {
                entry.address = assetPath;
                count++;
            }
        }

        if (count > 0)
        {
            settings.SetDirty(AddressableAssetSettings.ModificationEvent.EntryMoved, null, true);
            AssetDatabase.SaveAssets();
            Debug.Log($"✅ Đã đánh dấu {count} asset là Addressable vào group \"{groupName}\"");
        }
        else
        {
            Debug.Log("📁 Không có asset nào được đánh dấu.");
        }
    }

    [MenuItem("Assets/Đặt tất cả asset là Addressable theo Group thư mục", true)]
    private static bool ValidateMarkAssets()
    {
        string path = GetSelectedFolderPath();
        return !string.IsNullOrEmpty(path) && Directory.Exists(path);
    }

    private static string GetSelectedFolderPath()
    {
        Object obj = Selection.activeObject;
        if (obj == null) return null;

        string path = AssetDatabase.GetAssetPath(obj);
        if (string.IsNullOrEmpty(path)) return null;

        if (Directory.Exists(path)) return path;
        return Path.GetDirectoryName(path);
    }
}
