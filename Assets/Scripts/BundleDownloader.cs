using System.Collections;
using System.IO;
using UnityEngine;
using UnityEngine.Networking;

public class BundleDownloader : MonoBehaviour
{
    [SerializeField] string fileName = "cuongdevPC"; // Hoặc cuongdevAPK
    string downloadUrl => $"https://github.com/cuong1206/nro-bundles/releases/latest/download/{fileName}";
    string localPath => Path.Combine(Application.persistentDataPath, fileName);

    void Start()
    {
        StartCoroutine(DownloadBundle());
    }

    IEnumerator DownloadBundle()
    {
        Res.outz($"📥 Đang tải {fileName} từ GitHub...");

        UnityWebRequest request = UnityWebRequest.Get(downloadUrl);
        request.downloadHandler = new DownloadHandlerFile(localPath);
        yield return request.SendWebRequest();

        if (request.result != UnityWebRequest.Result.Success)
        {
            Res.err($"❌ Tải thất bại: {request.error}");
        }
        else
        {
            Res.outz($"✅ Tải thành công: {localPath}");
            // Nếu muốn load luôn bundle:
            // StartCoroutine(LoadBundleFromFile(localPath));
        }
    }

    // Nếu bạn muốn load luôn sau khi tải
    IEnumerator LoadBundleFromFile(string path)
    {
        var bundleRequest = AssetBundle.LoadFromFileAsync(path);
        yield return bundleRequest;

        AssetBundle bundle = bundleRequest.assetBundle;
        if (bundle == null)
        {
            Res.err("❌ Không load được AssetBundle.");
            yield break;
        }

        Res.outz("✅ AssetBundle đã được load.");
        // Load assets từ bundle nếu muốn
    }
}
