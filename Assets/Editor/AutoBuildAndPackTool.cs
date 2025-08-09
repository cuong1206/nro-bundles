using System;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using UnityEditor;
using UnityEditor.AddressableAssets;
using UnityEditor.AddressableAssets.Settings;
using UnityEditor.Build;
using UnityEditor.Build.Reporting;
using UnityEngine;
using UnityEditor.AddressableAssets.Settings.GroupSchemas;
using System.Collections.Generic;
using UnityEngine.AddressableAssets;
public class AutoBuildAndPackTool
{
    [MenuItem("Tools/Build & Pack Bundle ➜ Windows")]
    public static void BuildAndPackWindows()
    {
        BuildAndPack(BuildTarget.StandaloneWindows64, "cuongdevPC");
    }
    [MenuItem("Tools/Build & Pack Bundle ➜ Android")]
public static void BuildAndPackAndroid()
    {
        BuildAndPack(BuildTarget.Android, "cuongdevAPK");
    }

    [MenuItem("Tools/Build & Pack Bundle ➜ iOS")]
    public static void BuildAndPackiOS()
    {
        BuildAndPack(BuildTarget.iOS, "cuongdevIOS");
    }

    private static void BuildAndPack(BuildTarget target, string outputFileName)
    {
        // 1. Đổi platform nếu cần
        if (EditorUserBuildSettings.activeBuildTarget != target)
        {
            EditorUserBuildSettings.SwitchActiveBuildTarget(BuildPipeline.GetBuildTargetGroup(target), target);
            Res.outz($"🔁 Đã chuyển platform sang {target}");
        }

        // 2. Build Addressables
        var modifiedGroups = EnableAllIncludeInBuildTemporarily();
        AddressableAssetSettings.BuildPlayerContent();
        RestoreIncludeInBuild(modifiedGroups);
        Res.outz($"✅ Đã build Addressables cho {target}");

        // 3. Thư mục bundle
        string bundleFolder = GetBundleFolder(target);
        if (!Directory.Exists(bundleFolder))
        {
            Res.err($"❌ Không tìm thấy thư mục bundle: {bundleFolder}");
            return;
        }
        // 2. Xử lý file video dựa trên platform
        string introVideoPath = Path.Combine(Application.dataPath, "BuildOutput/background.mp4");
        string introScenePath = Path.Combine(Application.dataPath, "Scenes/Intro/background.mp4");

        if (target == BuildTarget.iOS)
        {
            // Sao chép video vào Assets/Scenes/Intro cho iOS
            if (File.Exists(introVideoPath))
            {
                Directory.CreateDirectory(Path.Combine(Application.dataPath, "Scenes/Intro"));
                File.Copy(introVideoPath, introScenePath, true);
                Res.outz($"📥 Đã copy background.mp4 vào Assets/Scenes/Intro cho iOS.");

                // Gán Video Clip cho VideoPlayer trong Main Camera
                string assetPath = "Assets/Scenes/Intro/background.mp4";
                UnityEngine.Video.VideoClip videoClip = AssetDatabase.LoadAssetAtPath<UnityEngine.Video.VideoClip>(assetPath);
                if (videoClip != null)
                {
                    GameObject mainCamera = GameObject.Find("Main Camera");
                    if (mainCamera != null)
                    {
                        UnityEngine.Video.VideoPlayer videoPlayer = mainCamera.GetComponent<UnityEngine.Video.VideoPlayer>();
                        if (videoPlayer != null)
                        {
                            videoPlayer.source = UnityEngine.Video.VideoSource.VideoClip;
                            videoPlayer.clip = videoClip;
                            EditorUtility.SetDirty(videoPlayer);
                            AssetDatabase.SaveAssets();
                            Res.outz($"✅ Đã gán background.mp4 vào VideoPlayer của Main Camera.");
                        }
                    }
                }
            }
        }
        else if (target == BuildTarget.StandaloneWindows64 || target == BuildTarget.Android)
        {
            // Xóa video khỏi Assets/Scenes/Intro cho PC/Android
            if (File.Exists(introScenePath))
            {
                File.Delete(introScenePath);
                Res.outz($"🗑 Đã xóa background.mp4 khỏi Assets/Scenes/Intro cho {target}.");
            }
        }

        // 4. Pack
        string outputDirectory = Path.Combine(Application.dataPath, "BuildOutput");
        Directory.CreateDirectory(outputDirectory);
        string outputFilePath = Path.Combine(outputDirectory, outputFileName);
        string encryptionKey = "Cuongdevvippro123..";
        string[] bundleFiles = Directory.GetFiles(bundleFolder, "*.bundle", SearchOption.TopDirectoryOnly);

        using (BinaryWriter writer = new BinaryWriter(File.Open(outputFilePath, FileMode.Create)))
        {
            writer.Write(bundleFiles.Length);

            foreach (var file in bundleFiles)
            {
                byte[] raw = File.ReadAllBytes(file);
                byte[] encrypted = EncryptAES(raw, encryptionKey);

                string originalName = Path.GetFileNameWithoutExtension(file);
                string cuongdevName = originalName + ".cuongdev";

                writer.Write(cuongdevName);
                writer.Write(encrypted.Length);
                writer.Write(encrypted);
            }
        }

        Res.outz($"📦 Đã pack {bundleFiles.Length} bundle thành {outputFilePath}");
        UploadToGitHub(outputFilePath); // <-- gọi ở đây
        UploadToGitHub(Path.Combine(outputDirectory, "background.mp4")); // <-- gọi ở đây
                                                                         //  HandleIntroVideo(target);
        if (Directory.Exists(bundleFolder))
        {
            Directory.Delete(bundleFolder, true);
            Res.outz($"🗑 Đã xóa thư mục bundle: {bundleFolder}");
        }

    }

    private static byte[] EncryptAES(byte[] data, string password)
    {
        using (Aes aes = Aes.Create())
        {
            var pdb = new Rfc2898DeriveBytes(password, Encoding.UTF8.GetBytes("SALT1234"), 1000);
            aes.Key = pdb.GetBytes(32);
            aes.IV = pdb.GetBytes(16);
            using (MemoryStream ms = new MemoryStream())
            using (CryptoStream cs = new CryptoStream(ms, aes.CreateEncryptor(), CryptoStreamMode.Write))
            {
                cs.Write(data, 0, data.Length);
                cs.Close();
                return ms.ToArray();
            }
        }
    }

    private static string GetBundleFolder(BuildTarget target)
    {
        string baseFolder = "Library/com.unity.addressables/aa/";
        switch (target)
        {
            case BuildTarget.Android:
                return Path.Combine(baseFolder, "Android/Android/");
            case BuildTarget.iOS:
                return Path.Combine(baseFolder, "iOS/iOS/");
            case BuildTarget.StandaloneWindows64:
                return Path.Combine(baseFolder, "Windows/StandaloneWindows64/");
            default:
                Res.err("❌ Chưa hỗ trợ platform này.");
                return "";
        }
    }


    private class GroupBuildState
    {
        public AddressableAssetGroup group;
        public bool wasIncluded;
    }

    // Trả về danh sách group đã được bật để sau đó khôi phục
    private static List<GroupBuildState> EnableAllIncludeInBuildTemporarily()
    {
        var settings = AddressableAssetSettingsDefaultObject.Settings;
        var modified = new List<GroupBuildState>();
    foreach (var group in settings.groups)
        {
            if (group == null) continue;
            var schema = group.GetSchema<BundledAssetGroupSchema>();
            if (schema != null)
            {
                if (!schema.IncludeInBuild)
                {
                    modified.Add(new GroupBuildState { group = group, wasIncluded = false });
                    schema.IncludeInBuild = true;
                    EditorUtility.SetDirty(schema);
                }
            }
        }

        AssetDatabase.SaveAssets();
        return modified;
    }

    private static void RestoreIncludeInBuild(List<GroupBuildState> modifiedGroups)
    {
        foreach (var state in modifiedGroups)
        {
            var schema = state.group.GetSchema<BundledAssetGroupSchema>();
            if (schema != null && !state.wasIncluded)
            {
                schema.IncludeInBuild = false;
                EditorUtility.SetDirty(schema);
            }
        }
    AssetDatabase.SaveAssets();
    }
    static void UploadToGitHub(string filePath)
    {
        string fileName = Path.GetFileName(filePath);
        string ghPath = @"C:\Program Files\GitHub CLI\gh.exe";
        string command = $"gh release upload latest \"{filePath}\" --clobber";

        System.Diagnostics.ProcessStartInfo psi = new()
        {
            FileName = ghPath,
            Arguments = $"release upload latest \"{filePath}\" --clobber",
            RedirectStandardOutput = true,
            RedirectStandardError = true,
            UseShellExecute = false,
            CreateNoWindow = true,
        };

        var process = System.Diagnostics.Process.Start(psi);
        string output = process.StandardOutput.ReadToEnd();
        string error = process.StandardError.ReadToEnd();
        process.WaitForExit();

        if (process.ExitCode == 0)
            Debug.Log($"✅ Đã upload {fileName} lên GitHub Releases: cuong1206/nro-bundles (latest)");
        else
            Debug.LogError($"❌ Lỗi upload {fileName}:\n{error}");
    }
    private static void HandleIntroVideo(BuildTarget target)
    {
        string streamingAssetsPath = Path.Combine(Application.dataPath, "Scenes/Intro");
        string sourceVideoPath = Path.Combine(Application.dataPath, "BuildOutput/background.mp4");
        string destVideoPath = Path.Combine(streamingAssetsPath, "background.mp4");

        // Đảm bảo thư mục StreamingAssets tồn tại
        if (!Directory.Exists(streamingAssetsPath))
            Directory.CreateDirectory(streamingAssetsPath);

        if (target == BuildTarget.iOS)
        {
            // Copy video vào StreamingAssets cho iOS
            if (File.Exists(sourceVideoPath))
            {
                File.Copy(sourceVideoPath, destVideoPath, true);
                Debug.Log("📥 Đã copy background.mp4 vào Scene/Intro (iOS).");
            }
            else
            {
                Debug.LogWarning("⚠ Không tìm thấy background.mp4 để copy cho iOS!");
            }
        }
        else
        {
            // Xóa video nếu có để PC/Android tải từ GitHub
            if (File.Exists(destVideoPath))
            {
                File.Delete(destVideoPath);
                Debug.Log("🗑 Đã xóa background.mp4 khỏi StreamingAssets (PC/Android).");
            }
        }
    }
}