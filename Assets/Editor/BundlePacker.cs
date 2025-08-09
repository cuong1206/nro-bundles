using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using UnityEditor;
using UnityEngine;

public class BundlePacker
{
    [MenuItem("Tools/Pack Bundle to data.bytes")]
    public static void PackBundles()
    {
        string sourceFolder = "Library/com.unity.addressables/aa/Windows/StandaloneWindows64/";
        string outputFilePath = Application.streamingAssetsPath + "/data.bytes";

        // Tùy chọn: đặt key mã hóa (nên lưu ở nơi khác, ví dụ trong code client hoặc server)
        string encryptionKey = "my-secret-key";

        // Gộp toàn bộ file
        using (BinaryWriter writer = new BinaryWriter(File.Open(outputFilePath, FileMode.Create)))
        {
            string[] bundleFiles = Directory.GetFiles(sourceFolder, "*.bundle");
            writer.Write(bundleFiles.Length);

            foreach (string file in bundleFiles)
            {
                byte[] fileBytes = File.ReadAllBytes(file);
                byte[] encryptedBytes = EncryptAES(fileBytes, encryptionKey);
                string fileName = Path.GetFileName(file);

                writer.Write(fileName);
                writer.Write(encryptedBytes.Length);
                writer.Write(encryptedBytes);
            }
        }

        Debug.Log($"✅ Packed {Application.streamingAssetsPath}/data.bytes thành công!");
    }

    // Hàm mã hóa AES đơn giản
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
}
