using UnityEditor;
using UnityEngine;
using System.IO;
using System.Collections.Generic;

public class BundleAssetExplorer : EditorWindow
{
    private string folderPath = "";
    private Vector2 scroll;
    private Dictionary<string, List<string>> bundleAssets = new Dictionary<string, List<string>>();

    [MenuItem("Tools/Bundle Asset Explorer")]
    public static void Open()
    {
        GetWindow<BundleAssetExplorer>("Bundle Asset Explorer");
    }

    private void OnGUI()
    {
        GUILayout.Label("📦 Bundle Asset Explorer", EditorStyles.boldLabel);
        GUILayout.Space(5);

        EditorGUILayout.BeginHorizontal();
        folderPath = EditorGUILayout.TextField("Thư mục chứa .bundle:", folderPath);

        if (GUILayout.Button("📁", GUILayout.Width(30)))
        {
            string selected = EditorUtility.OpenFolderPanel("Chọn thư mục chứa các file .bundle", "", "");
            if (!string.IsNullOrEmpty(selected))
                folderPath = selected;
        }
        EditorGUILayout.EndHorizontal();

        GUILayout.Space(5);

        if (GUILayout.Button("📂 Quét các file .bundle và liệt kê asset"))
        {
            ScanBundles(folderPath);
        }

        GUILayout.Space(10);

        scroll = EditorGUILayout.BeginScrollView(scroll);

        foreach (var kvp in bundleAssets)
        {
            EditorGUILayout.LabelField($"📦 {Path.GetFileName(kvp.Key)}", EditorStyles.boldLabel);
            foreach (var asset in kvp.Value)
            {
                EditorGUILayout.LabelField("   🔹 " + asset);
            }
            EditorGUILayout.Space();
        }

        EditorGUILayout.EndScrollView();
    }

    private void ScanBundles(string path)
    {
        bundleAssets.Clear();

        if (!Directory.Exists(path))
        {
            Debug.LogError("❌ Thư mục không tồn tại: " + path);
            return;
        }

        string[] files = Directory.GetFiles(path, "*.bundle", SearchOption.AllDirectories);

        foreach (var file in files)
        {
            try
            {
                var bundle = AssetBundle.LoadFromFile(file);
                if (bundle == null)
                {
                    Debug.LogError("❌ Không thể load bundle: " + file);
                    continue;
                }

                var names = bundle.GetAllAssetNames();
                bundleAssets[file] = new List<string>(names);

                Debug.Log($"✅ {Path.GetFileName(file)} chứa {names.Length} asset:");
                foreach (var n in names)
                    Debug.Log("   ↳ " + n);

                bundle.Unload(false);
            }
            catch (System.Exception ex)
            {
                Debug.LogError("❌ Lỗi khi đọc bundle: " + file + "\n" + ex.Message);
            }
        }

        Debug.Log("🎉 Quét bundle xong. Tổng cộng: " + bundleAssets.Count + " file.");
    }
}
