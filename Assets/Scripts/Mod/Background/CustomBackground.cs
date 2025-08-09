using System;
using System.Collections.Generic;
using System.IO;
using System.IO.IsolatedStorage;
using System.Linq;
using System.Threading;
using UnityEngine;
#if UNITY_EDITOR || UNITY_STANDALONE
#endif

namespace Mod.Background
{
    internal class CustomBackground
    {
        internal static bool isEnabled = true;

        internal static Dictionary<string, IBackground> customBgs = new Dictionary<string, IBackground>();
        static ScaleMode _defaultScaleMode = ScaleMode.ScaleAndCrop;
        internal static ScaleMode DefaultScaleMode
        {
            get => _defaultScaleMode;
            set
            {
                _defaultScaleMode = value;
                if (_defaultScaleMode > ScaleMode.ScaleToFit)
                    _defaultScaleMode = 0;
                foreach (var customBg in customBgs.Where(cBG => !overrideScaleMode.ContainsKey(cBG.Key)))
                    customBg.Value.ScaleMode = _defaultScaleMode;
            }
        }
        internal static Dictionary<string, ScaleMode> overrideScaleMode = new Dictionary<string, ScaleMode>();

        internal static int intervalChangeBg = 30000;
        static int bgIndex;
        static bool isAllBgsLoaded;
        static long lastTimeChangedBg;
        static bool isChangeBg = true;
        static float speed = 1f;
        static CustomBackground instance = new CustomBackground();

        internal static IBackground CurrentBg => customBgs.ElementAt(bgIndex).Value;

        internal static void StopAllBackgroundVideo()
        {
            foreach (BackgroundVideo backgroundVideo in customBgs.Values.OfType<BackgroundVideo>().Where(v => v.isPlaying))
                backgroundVideo.Stop();
        }

        public static void SelectBackgrounds()
        {
            string path = IntroVideoLoader.VideoPath();
            if (!customBgs.ContainsKey(path))
            {
                try
                {
                    customBgs[path] = new BackgroundVideo(path);
                    customBgs[path].ScaleMode = DefaultScaleMode;
                    isAllBgsLoaded = true;
                    Res.outz("Setup thanh cong video bg: " + path);
                }
                catch (FileNotFoundException)
                {
                    Res.outz("File not found: " + path);
                }
                catch (IsolatedStorageException)
                {
                    Res.outz("Isolated storage error: " + path);
                }
                catch (Exception ex)
                {
                    Debug.LogException(ex);
                }
            }
        }

        internal static void Update()
        {
            if (customBgs.Count > 0)
                return;
            SelectBackgrounds();
            
           /* if (GameCanvas.currentScreen is GameScr)
            {
                StopAllBackgroundVideo();
            }*/
            if (!isAllBgsLoaded)
            {
                List<string> paths = new List<string>(customBgs.Keys);
                for (int i = paths.Count - 1; i >= 0; i--)
                {
                    string path = paths[i];
                    try
                    {
                        if (customBgs[path] != null && customBgs[path].IsLoaded)
                            continue;
                        if (path.EndsWith(".mp4"))
                            customBgs[path] = new BackgroundVideo(path);
                        else
                            customBgs[path] = new StaticImage(path);
                        if (overrideScaleMode.ContainsKey(path))
                            customBgs[path].ScaleMode = overrideScaleMode[path];
                        else
                            customBgs[path].ScaleMode = DefaultScaleMode;
                    }
                    catch (FileNotFoundException)
                    {
                        customBgs.Remove(path);
                    }
                    catch (IsolatedStorageException)
                    {
                        customBgs.Remove(path);
                    }
                    catch (Exception ex) { Debug.LogException(ex); }
                }
                lastTimeChangedBg = mSystem.currentTimeMillis();
                isAllBgsLoaded = true;
            }
        }

        internal static void Paint(mGraphics g)
        {
            Res.outz("Paint 0");
            if (customBgs.Count <= 0)
                return;
            try
            {
                Res.outz("Paint 1");
                if (bgIndex >= customBgs.Count)
                    bgIndex = 0;
                Res.outz("Paint 2");
                string videoPath = IntroVideoLoader.VideoPath();
                if (!customBgs.ContainsKey(videoPath) || customBgs[videoPath] == null)
                {
                    SelectBackgrounds();
                }
                IBackground background = customBgs[videoPath];
                if (background == null)
                {
                    Res.outz("Background is null, reinitializing");
                    SelectBackgrounds();
                    background = customBgs[videoPath];
                    if (background == null)
                    {
                        Res.outz("Failed to initialize background for: " + videoPath);
                        return;
                    }
                }
                Res.outz("Paint 3");
                if (background is BackgroundVideo backgroundVideo && !backgroundVideo.isPlaying)
                {
                    if (!backgroundVideo.IsLoaded && !backgroundVideo.isPreparing)
                        backgroundVideo.Prepare();
                    backgroundVideo.Play();
                }
                Res.outz("Paint 4");
                background.Paint(g, 0, 0);
                Res.outz("Paint 5");

            }
            catch (Exception ex) { Debug.LogException(ex); }
        }

        internal static void SetState(bool value)
        {
            isEnabled = value;
            if (value)
                return;
            foreach (BackgroundVideo backgroundVideo in customBgs.Values.Where((background) => background is BackgroundVideo))
                backgroundVideo.Stop();
        }

        public void onCancelChat() { }
    }
}