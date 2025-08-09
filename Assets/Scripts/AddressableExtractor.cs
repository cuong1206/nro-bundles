using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using UnityEngine;

public class AddressableExtractor : MonoBehaviour
{
    public static IEnumerator ExtractAndDecrypt(string bundlePath, string extractFolder, string password, Action onDone = null)
    {
        byte[] fullData = null;

#if UNITY_ANDROID && !UNITY_EDITOR
using (UnityEngine.Networking.UnityWebRequest uwr = UnityEngine.Networking.UnityWebRequest.Get(bundlePath))
{
yield return uwr.SendWebRequest();

    if (uwr.result != UnityEngine.Networking.UnityWebRequest.Result.Success)
    {
        Res.err("❌ Không đọc được cuongdev: " + uwr.error);
        yield break;
    }

    fullData = uwr.downloadHandler.data;
}
#else
        if (!File.Exists(bundlePath))
        {
            Res.err("❌ Không tìm thấy file: " + bundlePath);
            yield break;
        }
        fullData = File.ReadAllBytes(bundlePath);
#endif
try
        {
            using (MemoryStream stream = new MemoryStream(fullData))
            using (BinaryReader reader = new BinaryReader(stream))
            {
                int fileCount = reader.ReadInt32();
                Res.outz($"📦 Tổng số file trong bundle: {fileCount}");

                if (!Directory.Exists(extractFolder))
                    Directory.CreateDirectory(extractFolder);

                StringBuilder extractedList = new StringBuilder();

                for (int i = 0; i < fileCount; i++)
                {
                    string fileName = reader.ReadString();
                    int length = reader.ReadInt32();
                    byte[] encryptedData = reader.ReadBytes(length);
                    string outputPath = Path.Combine(extractFolder, fileName); // ✅ sửa ở đây
                    File.WriteAllBytes(outputPath, encryptedData);

                    extractedList.AppendLine($" - {fileName}");
                }

                Res.outz($"✅ Cac file da duoc giai nen:\n{extractedList}");
                Res.outz("🎉 Đã giải mã & giải nén xong, bạn có thể tiếp tục load asset ở bước sau.");
                onDone?.Invoke();
            }
        }
        catch (Exception ex)
        {
            Res.err("❌ Loi gia nen: " + ex.Message);
        }
    }
    private static byte[] DecryptAES(byte[] encryptedData, string password)
    {
        using (Aes aes = Aes.Create())
        {
            var key = new Rfc2898DeriveBytes(password, Encoding.UTF8.GetBytes("SALT1234"), 1000);
            aes.Key = key.GetBytes(32);
            aes.IV = key.GetBytes(16);

            using (MemoryStream msDecrypt = new MemoryStream())
            {
                using (CryptoStream cs = new CryptoStream(msDecrypt, aes.CreateDecryptor(), CryptoStreamMode.Write))
                {
                    cs.Write(encryptedData, 0, encryptedData.Length);
                    cs.FlushFinalBlock();
                }
                return msDecrypt.ToArray();
            }
        }
    }
    public static IEnumerator ExtractFromBytes(byte[] fullData, string extractFolder, string password, Action onDone = null)
    {
        List<(string name, byte[] data)> entries = new();
// Đọc toàn bộ dữ liệu vào bộ nhớ trước (dùng try-catch)
    try
        {
            using var stream = new MemoryStream(fullData);
            using var reader = new BinaryReader(stream);

            int fileCount = reader.ReadInt32();
            if (!Directory.Exists(extractFolder))
                Directory.CreateDirectory(extractFolder);

            for (int i = 0; i < fileCount; i++)
            {
                string fileName = reader.ReadString();
                int length = reader.ReadInt32();
                byte[] encryptedData = reader.ReadBytes(length);

                entries.Add((fileName, encryptedData));
            }
        }
        catch (Exception ex)
        {
            Res.err("❌ ExtractFromBytes: Loi giai nen - " + ex.Message);
            yield break;
        }

        // Ghi từng file ra đĩa và yield ngoài try-catch
        foreach (var entry in entries)
        {
            string outPath = Path.Combine(extractFolder, entry.name);
            File.WriteAllBytes(outPath, entry.data);
            yield return null;
        }

        Res.outz("✅ ExtractFromBytes: Da ghi toan bo bundle tu stream.");
        onDone?.Invoke();
    }
    private static byte[] EncryptAES(byte[] data, string password)
    {
        byte[] salt = Encoding.UTF8.GetBytes("SALT1234");
        using (var aes = Aes.Create())
        {
            var key = new Rfc2898DeriveBytes(password, salt, 1000);
            aes.Key = key.GetBytes(32); // 256-bit key
            aes.IV = key.GetBytes(16); // 128-bit IV

            using (var msEncrypt = new MemoryStream())
            using (var cs = new CryptoStream(msEncrypt, aes.CreateEncryptor(), CryptoStreamMode.Write))
            {
                cs.Write(data, 0, data.Length);
                cs.FlushFinalBlock();
                return msEncrypt.ToArray();
            }
        }
    }
    }