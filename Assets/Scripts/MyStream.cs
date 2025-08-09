using System;
using UnityEngine;

public class MyStream
{
	public static DataInputStream readFile(string path)
	{
		path = Main.res + path;
		try
		{
			return DataInputStream.getResourceAsStream(path);
		}
		catch (Exception)
		{
			return null;
		}
	}
    public static DataInputStream readFileBundle(string path)
    {
        path = Main.res + path;
        // Nếu path chưa có đuôi .bytes thì thêm vào
        if (!path.EndsWith(".bytes"))
            path += ".bytes";

        try
        {
            // Load từ AssetBundle (đã giải mã & preload)
            TextAsset asset = LoadAssetHelper.Load<TextAsset>(path);
            if (asset != null)
            {
                return new DataInputStream(asset);
            }
        }
        catch (System.Exception ex)
        {
            Debug.LogError("❌ Lỗi load từ AssetBundle: " + ex.Message);
        }

        // Có thể fallback về dạng load cũ nếu cần (tuỳ bạn muốn giữ không)
        try
        {
            return DataInputStream.getResourceAsStream(Main.res + path);
        }
        catch (System.Exception)
        {
            return null;
        }
    }
}
