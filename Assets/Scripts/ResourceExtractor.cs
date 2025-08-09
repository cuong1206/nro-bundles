using System;
using System.Collections;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using ICSharpCode.SharpZipLib.Zip;
using UnityEngine;

// Token: 0x020000B0 RID: 176
public class ResourceExtractor : MonoBehaviour
{
    // Token: 0x0600070B RID: 1803 RVA: 0x0007DB8B File Offset: 0x0007BD8B
    private void Start()
    {
        base.StartCoroutine(this.ExtractIfFirstTime());
    }

    // Token: 0x0600070C RID: 1804 RVA: 0x0007DB9A File Offset: 0x0007BD9A
    private IEnumerator ExtractIfFirstTime()
    {
        string extractRoot = Path.Combine(Application.persistentDataPath, "res");
        string flagFile = Path.Combine(extractRoot, "done.flag");
        if (File.Exists(flagFile))
        {
            Debug.Log("✔ Đã giải nén từ trước, không cần giải lại.");
            yield break;
        }
        string path = Path.Combine(Application.streamingAssetsPath, "resource.pak");
        byte[] encryptedData = File.ReadAllBytes(path);
        yield return null;
        using (MemoryStream memoryStream = new MemoryStream(this.DecryptAES(encryptedData)))
        {
            using (ZipInputStream zipInputStream = new ZipInputStream(memoryStream))
            {
                ZipEntry nextEntry;
                while ((nextEntry = zipInputStream.GetNextEntry()) != null)
                {
                    string path2 = Path.Combine(extractRoot, nextEntry.Name);
                    string directoryName = Path.GetDirectoryName(path2);
                    if (!Directory.Exists(directoryName))
                    {
                        Directory.CreateDirectory(directoryName);
                    }
                    using (FileStream fileStream = File.Create(path2))
                    {
                        byte[] array = new byte[4096];
                        int count;
                        while ((count = zipInputStream.Read(array, 0, array.Length)) > 0)
                        {
                            fileStream.Write(array, 0, count);
                        }
                    }
                }
                File.WriteAllText(flagFile, "ok");
                Debug.Log("✅ Giải nén resource.pak hoàn tất tại: " + extractRoot);
                yield break;
            }
        }
        yield break;
    }

    // Token: 0x0600070D RID: 1805 RVA: 0x0007DBAC File Offset: 0x0007BDAC
    private byte[] DecryptAES(byte[] data)
    {
        byte[] result;
        using (Aes aes = Aes.Create())
        {
            aes.Key = Encoding.UTF8.GetBytes("1234567890abcdef");
            aes.IV = Encoding.UTF8.GetBytes("abcdef1234567890");
            using (MemoryStream memoryStream = new MemoryStream())
            {
                using (CryptoStream cryptoStream = new CryptoStream(memoryStream, aes.CreateDecryptor(), CryptoStreamMode.Write))
                {
                    cryptoStream.Write(data, 0, data.Length);
                    cryptoStream.FlushFinalBlock();
                    result = memoryStream.ToArray();
                }
            }
        }
        return result;
    }

    // Token: 0x0600070E RID: 1806 RVA: 0x00003664 File Offset: 0x00001864
    public ResourceExtractor()
    {
    }

    // Token: 0x04000F7B RID: 3963
    private const string AES_KEY = "1234567890abcdef";

    // Token: 0x04000F7C RID: 3964
    private const string AES_IV = "abcdef1234567890";
}
