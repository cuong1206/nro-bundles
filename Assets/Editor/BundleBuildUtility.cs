using UnityEditor;
using UnityEditor.AddressableAssets;
using UnityEditor.AddressableAssets.Settings;
using System.IO;
using UnityEngine;

public class BundleBuildUtility
{
[MenuItem("Tools/Pack & Clean Addressables")]
public static void PackAndClean()
{
// 1. Build Addressables
AddressableAssetSettings.BuildPlayerContent();
    // 2. Gọi tool bạn đã viết để pack thành data.bytes
    // => TODO: gọi hàm của bạn ở đây

    // 3. Clean catalog để build không crash
    AddressableAssetSettings.CleanPlayerContent();

    // 4. (Tuỳ chọn) Xoá thư mục build
    string path = "Library/com.unity.addressables/aa";
    if (Directory.Exists(path)) Directory.Delete(path, true);

    Debug.Log("✅ Đã pack bundle, xoá catalog và dọn thư mục. Sẵn sàng build.");
}
}