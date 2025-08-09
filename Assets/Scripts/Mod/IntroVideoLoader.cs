using System.Collections;
using System.IO;
using UnityEditor;
using UnityEngine;
using UnityEngine.Networking;

public class IntroVideoLoader : MonoBehaviour
{
    public static string FileName = "videos/background.mp4";
    public static string DownloadUrl = "https://github.com/cuong1206/nro-bundles/releases/download/latest/background.mp4";

    public static bool IsVideoReady => File.Exists(VideoPath());
    public static string VideoPath()
    {
#if UNITY_IOS
        // iOS: lấy video từ StreamingAssets
        return Path.Combine(Application.streamingAssetsPath, FileName);
#else
        // Android & PC: lấy video đã tải (persistentDataPath)
        return Path.Combine(Application.persistentDataPath, FileName);
#endif
    }

    public static IEnumerator DownloadIntroVideo(System.Action onComplete)
    {
        string videoPath = VideoPath();
        string directoryPath = Path.GetDirectoryName(videoPath); // Lấy đường dẫn thư mục (ví dụ: persistentDataPath/videos)

        if (IsVideoReady)
        {
            Debug.Log("✅ Da co san video intro tại: " + videoPath);
            onComplete?.Invoke();
            yield break;
        }

        // Tạo thư mục nếu chưa tồn tại
        if (!string.IsNullOrEmpty(directoryPath) && !Directory.Exists(directoryPath))
        {
            Directory.CreateDirectory(directoryPath);
            Debug.Log("📂 Da tao thu muc: " + directoryPath);
        }

        Debug.Log("📥 Bat dau tai video intro tu GitHub...");

        UnityWebRequest uwr = UnityWebRequest.Get(DownloadUrl);
        uwr.downloadHandler = new DownloadHandlerFile(videoPath);
        yield return uwr.SendWebRequest();

        if (uwr.result == UnityWebRequest.Result.Success)
        {
            Debug.Log("✅ Tai video intro thanh cong tại: " + videoPath);
            onComplete?.Invoke();
        }
        else
        {
            Debug.LogError("❌ Loi tai video intro: " + uwr.error);
            if (File.Exists(videoPath))
            {
                Debug.LogWarning("File tai that bai, xoa file hong: " + videoPath);
                File.Delete(videoPath); // Xóa file hỏng nếu có
            }
        }
    }
}