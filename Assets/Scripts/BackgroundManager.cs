using UnityEngine;
using Mod.Background;

public class BackgroundManager : MonoBehaviour
{
    private BackgroundVideo backgroundVideo;

    void Start()
    {
        try
        {
            backgroundVideo = new BackgroundVideo(IntroVideoLoader.VideoPath());
            backgroundVideo.Prepare();
            backgroundVideo.ScaleMode = ScaleMode.ScaleToFit; // Chọn chế độ vừa với màn hình
        }
        catch (System.Exception e)
        {
            Debug.LogError("Lỗi khi khởi tạo video: " + e.Message);
        }
    }

    void OnGUI()
    {
        if (backgroundVideo != null && backgroundVideo.IsLoaded)
        {
            mGraphics g = GameCanvas.g; // Lấy mGraphics thực tế từ game
            if (g != null)
            {
                backgroundVideo.Paint(g, 0, 0);
            }
            else
            {
                Debug.LogError("GameCanvas.g is null, check initialization!");
            }
        }
    }

    void OnDestroy()
    {
        if (backgroundVideo != null)
        {
            backgroundVideo.Stop();
        }
    }
}