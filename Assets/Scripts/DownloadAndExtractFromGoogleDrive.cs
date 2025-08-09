using System.Collections;
using System.IO;
using UnityEngine;
using UnityEngine.Networking;

public class DownloadAndExtractFromGoogleDrive : MonoBehaviour
{

private string encryptionKey = Main._43756F6E6764657676697070726F3132332E2E();
private string extractFolder => Path.Combine(Application.persistentDataPath, "Extracted");

IEnumerator Start()
{
    yield return StartCoroutine(DownloadAndExtract(GameCanvas.getLinkBundle()));
  //  LoadAssetHelper.PreloadBundles();
}

IEnumerator DownloadAndExtract(string url)
{
    Res.outz($"🌐 Tải bundle từ: {url}");

    UnityWebRequest uwr = UnityWebRequest.Get(url);
    uwr.timeout = 30;
    yield return uwr.SendWebRequest();

    if (uwr.result != UnityWebRequest.Result.Success)
    {
        Res.err("❌ Lỗi tải file: " + uwr.error);
        yield break;
    }

    byte[] data = uwr.downloadHandler.data;

    yield return StartCoroutine(AddressableExtractor.ExtractFromBytes(data, extractFolder, encryptionKey, () =>
    {
        Res.outz("✅ Đã tải và extract xong từ Google Drive");
    }));
}
}