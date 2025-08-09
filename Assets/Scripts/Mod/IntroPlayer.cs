using System.IO;
using Mod;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Video;
using System.Threading;
using System.Collections;
using System;
using UnityEngine.Networking;
using UnityEngine.UI;
using Mod.Background;
using Unity.VisualScripting;



#if UNITY_EDITOR || UNITY_STANDALONE
#endif

namespace Mod
{
    public class IntroPlayer : MonoBehaviour
    {

        internal static bool isEnabled = true;
        internal static string path = Path.Combine(Application.streamingAssetsPath, "videos/background.mp4");
        internal static float volume = 1f;
        static bool isExtractDone = false;
        static bool isWaitingForExtract = false;
        static VideoPlayer videoPlayer;
        static bool isPlaying;
        private int downloadProgressPercent = 0;
        private string downloadStatusText = "Đang kết nối...";
        private int count = 0;
        public Text loadingText; // gán từ Inspector
        void Awake()
        {
            videoPlayer = GetComponent<VideoPlayer>();
            if (videoPlayer == null) Res.err("VideoPlayer component not found!");
            isEnabled = true;
            volume = 0f;
            Res.outz($"VideoPlayer initialized, source: {videoPlayer.source}, url: {videoPlayer.url}");
        }

        private IEnumerator Start()
        {
            ShowText("Đang tải video intro...");
            isExtractDone = false;
            isWaitingForExtract = false;

            string extractFolder = Path.Combine(Application.persistentDataPath, "Extracted");
            string encryptionKey = Main._43756F6E6764657676697070726F3132332E2E();

            bool needExtract = true;
            if (Directory.Exists(extractFolder))
            {
                string[] bundles = Directory.GetFiles(extractFolder, "*.cuongdev");
                if (bundles.Length > 0)
                {
                    Res.outz($" Da co san {bundles.Length} file bundle, ko can extract lai.");
                    needExtract = false;
                    isExtractDone = true;
                }
            }

            // 1. Tải và chuẩn bị video intro
            yield return StartCoroutine(IntroVideoLoader.DownloadIntroVideo(() =>
            {
                videoPlayer.url = IntroVideoLoader.VideoPath();
                videoPlayer.Prepare();
            }));

            yield return new WaitUntil(() => videoPlayer.isPrepared);
            SetupAndPlayVideo();
            StartCoroutine(InitSequence());

            // 2. Nếu cần, sau khi video chuẩn bị xong mới bắt đầu download & extract
            if (needExtract)
            {
                string downloadURL = GameCanvas.getLinkBundle();
                yield return StartCoroutine(DownloadAndExtract(downloadURL, extractFolder, encryptionKey, () =>
                {
                    Res.outz("Callback: Giai nen xong bat dau load asset");
                   
                    isExtractDone = true;
                }));
            }
            else
            {
                // Nếu đã có bundle thì cho đếm ngược 5 giây
                StartCoroutine(WaitAndAutoTransition(5f));
            }
        }

        private IEnumerator WaitAndAutoTransition(float time)
        {
            float timer = 0f;
            downloadStatusText = "Đang vào game...";
            while (timer < time)
            {
                timer += Time.deltaTime;
                yield return null;
            }
            OnDestroy();
            SceneManager.LoadScene("SampleScene");
        }
        void ShowText(string msg)
        {
            if (loadingText != null)
                loadingText.text = msg;
        }

        IEnumerator SimulateLoading()
        {
            float progress = 0f;
            while (progress < 1f)
            {
                progress += 0.1f;
                ShowText($"Đang tải dữ liệu... {Mathf.RoundToInt(progress * 100)}%");
                yield return new WaitForSeconds(0.3f);
            }

            ShowText("Tải xong, vào game...");
            yield return new WaitForSeconds(1f);
            UnityEngine.SceneManagement.SceneManager.LoadScene("SampleScene");
        }
        IEnumerator InitSequence()
        {
            // 1. Tải video intro trước
            yield return StartCoroutine(IntroVideoLoader.DownloadIntroVideo(() =>
            {
                videoPlayer.url = IntroVideoLoader.VideoPath();
                videoPlayer.Prepare();
            }));

            // 2. Đợi video chuẩn bị xong rồi phát
            yield return new WaitUntil(() => videoPlayer.isPrepared);
            videoPlayer.Play();
            Res.outz($"Dang phat video...");
            // 3. Hiển thị UI hoặc chữ "Đang tải..." nếu cần
            // 4. Trong lúc đó, có thể load bundle song song hoặc sau đó
        }
        void SetupAndPlayVideo()
        {
            if (!isEnabled)
            {
                OnDestroy();
                SceneManager.LoadScene("SampleScene");
                return;
            }

            if (File.Exists(path))
            {
                videoPlayer.source = VideoSource.Url;
                videoPlayer.url = path;
            }
            else
            {
                videoPlayer.source = VideoSource.VideoClip;
            }
            Res.outz($"VideoPlayer source: {videoPlayer.source}, url: {videoPlayer.url}");
            if (volume == 0)
                videoPlayer.audioOutputMode = VideoAudioOutputMode.None;
            else
                videoPlayer.SetDirectAudioVolume(0, volume);

            videoPlayer.prepareCompleted += VideoPlayer_prepareCompleted;
            videoPlayer.loopPointReached += VideoEnded;
            videoPlayer.Prepare();
        }


        IEnumerator DownloadAndExtract(string url, string extractFolder, string encryptionKey, System.Action onDone)
        {
            downloadStatusText = $"Đang chuẩn bị gói tài nguyên Client...";
            Res.outz($"🌐 Đang tải file từ: {url}");
            using UnityWebRequest uwr = UnityWebRequest.Get(url);
            uwr.SendWebRequest();

            float lastProgress = 0f;

            while (!uwr.isDone)
            {
                if (uwr.downloadProgress - lastProgress >= 0.01f)
                {
                    lastProgress = uwr.downloadProgress;
                    downloadProgressPercent = Mathf.FloorToInt(uwr.downloadProgress * 100f);
                    downloadStatusText = $"Đang tải dữ liệu client {downloadProgressPercent}%";
                }
                yield return null;
            }

            if (uwr.result != UnityWebRequest.Result.Success)
            {
                Res.err("❌ Loi tai file: " + uwr.error);
                yield break;
            }

            downloadStatusText = "Tải xong, đang giải nén...";
            byte[] data = uwr.downloadHandler.data;
            yield return AddressableExtractor.ExtractFromBytes(data, extractFolder, encryptionKey, onDone);
        }
        void VideoPlayer_prepareCompleted(VideoPlayer source)
        {
            videoPlayer.Play();
            Res.outz($"Dang phat video 2...");
            isPlaying = true;
        }
        private void VideoEnded(VideoPlayer vp)
        {
            Res.outz("🎞️ Video đã phát hết");

            if (!isExtractDone)
            {
                Res.outz("⏳ Extract chưa xong → phát lại video");
                vp.Stop();
                vp.Play(); // Phát lại video
                isWaitingForExtract = true;
            }
            else
            {
                OnDestroy();
                Res.outz("✅ Extract xong rồi → chuyển scene");
                SceneManager.LoadScene("SampleScene");
            }
        }

        void Update()
        {
            if (isExtractDone)
            {
                LoadAssetHelper.PreloadBundles();
                downloadStatusText = "Đang vào game...";
                OnDestroy();
                SceneManager.LoadScene("SampleScene");
            }
            
            if (!videoPlayer.isPlaying && isPlaying)
            {
                if (!isExtractDone)
                {
                    if (!isWaitingForExtract)
                    {
                        Res.outz("⏳ Video hết nhưng vẫn đang tải/extract, sẽ phát lại.");
                        videoPlayer.Play();
                        isWaitingForExtract = true;
                    }
                }
                else
                {
                    downloadStatusText = "Đang vào game...";
                    OnDestroy();
                    SceneManager.LoadScene("SampleScene");
                }
            }
        }


        void OnGUI()
        {
                GUIStyle style = new GUIStyle(GUI.skin.label)
                {
                    fontSize = 30,
                    alignment = TextAnchor.MiddleCenter,
                    normal = { textColor = Color.white }
                };

                Rect labelRect = new Rect(0, Screen.height * 0.8f, Screen.width, 50);
                Rect backgroundRect = new Rect(0, labelRect.y, Screen.width, 60);

                // Vẽ nền đen mờ sau chữ
                Color prevColor = GUI.color;
                GUI.color = new Color(0, 0, 0, 0.75f); // đen có độ trong suốt
                GUI.DrawTexture(backgroundRect, Texture2D.whiteTexture); // texture trắng mặc định
                GUI.color = prevColor;

                // Vẽ chữ
                GUI.Label(labelRect, downloadStatusText, style);

            // Skip video nếu nhấn
            if (Input.GetMouseButtonDown(0) || GameCanvas.keyPressed[(!Main.isPC) ? 5 : 25])
            {
                if (isExtractDone)
                {
                    videoPlayer.Stop();
                    SceneManager.LoadScene("SampleScene");
                }
                else
                {
                    Res.outz("🚫 Đang tải dữ liệu Client, không thể bỏ qua video lúc này.");
                }
            }
        }
        private void OnDestroy()
        {
            if (videoPlayer != null)
            {
                videoPlayer.Stop();
                videoPlayer.clip = null;
                Resources.UnloadUnusedAssets(); // Giải phóng tài nguyên không dùng
                Res.outz("✅ Đã giải phóng VideoPlayer.");
            }
        }

    }

}
