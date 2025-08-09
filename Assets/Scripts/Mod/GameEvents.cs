using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
using Mod.AccountManager;
using Mod.Background;
using Mod.Graphics;
using Mod.R;
using Newtonsoft.Json.Linq;
using UnityEngine;
using UnityEngine.UIElements;

namespace Mod
{
    /// <summary>
    /// Định nghĩa các sự kiện của game.
    /// </summary>
    /// <remarks>
    /// - Các hàm bool trả về true thì sự kiện game sẽ không được thực hiện, 
    /// trả về false thì sự kiện sẽ được kích hoạt như bình thường.<br/>
    /// - Các hàm void hỗ trợ thực hiện các lệnh cùng với sự kiện.
    /// </remarks>
    internal static class GameEvents
    {
        static float _previousWidth = Screen.width;
        static float _previousHeight = Screen.height;
        static bool isHaveSelectSkill_old;
        static long lastTimeGamePause;
        static long lastTimeRequestPetInfo;
        static long delayRequestPetInfo = 1000;
        static long lastTimeRequestZoneInfo;
        static long delayRequestZoneInfo = 100;
        static bool isFirstPause = true;
        static bool isOpenZoneUI;
        static GUIStyle style;
        static string nameCustomServer = "";
        static string currentHost = "";
        static ushort currentPort = 0;

        internal static void OnAwake()
        {

        }

        /// <summary>
        /// Kích hoạt sau khi game khởi động.
        /// </summary>
        internal static void OnGameStart()
        {

            Time.fixedDeltaTime = (float)1 / 100f;
            Application.targetFrameRate = (int)Screen.currentResolution.refreshRateRatio.value;
            QualitySettings.vSyncCount = 1;

            if (Utils.IsAndroidBuild())
            {
                Screen.sleepTimeout = SleepTimeout.NeverSleep;
                Screen.fullScreenMode = FullScreenMode.FullScreenWindow;
                Screen.orientation = ScreenOrientation.AutoRotation;
                Screen.autorotateToLandscapeLeft = true;
                Screen.autorotateToLandscapeRight = true;
                Screen.autorotateToPortrait = false;
                Screen.autorotateToPortraitUpsideDown = false;
            }
            OnSetResolution();
            OnCheckZoomLevel(Screen.width, Screen.height);
            /*GameEventHook.InstallAllHooks();
            if (!Directory.Exists(Utils.CommonModDataPath))
                Directory.CreateDirectory(Utils.CommonModDataPath);
            CustomBackground.LoadData();
            CharEffectMain.Init();
            Setup.loadFile();
            //ChatCommandHandler.loadDefault();
            HotkeyCommandHandler.loadDefault();
            //SocketClient.gI.initSender();
            //CustomLogo.LoadData();
            //CustomCursor.LoadData();
            SetDo.LoadData();
            GraphicsReducer.InitializeTileMap(true);
            //UIReportersManager.AddReporter(Boss.Paint);
            UIReportersManager.AddReporter(ListCharsInMap.Paint);
            //ShareInfo.gI.toggle(true);
            VietnameseInput.LoadData();
            
            LogoMethod.LoadData();*/
            if (!Utils.IsOpenedByExternalAccountManager)
                InGameAccountManager.OnStart();
        }

        /// <summary>
        /// Kích hoạt khi người chơi chat.
        /// </summary>
        /// <param name="text">Nội dung chat.</param>
        /// <returns></returns>
        /// 
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
        internal static bool OnSendChat(string text)
        {
            /*HistoryChat.gI.append(text);
            return ChatCommandHandler.handleChatText(text);*/
            return false;
        }

        /// <summary>
        /// Kích hoạt sau khi <see cref="MonoBehaviour"/> <see cref="Main"/> được kích hoạt.
        /// </summary>
        internal static void OnMainStart()
        {
            UIImage.OnStart();
            if (Rms.loadRMSInt("svselect") == -1)
            {
                ServerListScreen.linkDefault = Strings.DEFAULT_IP_SERVERS;
                ServerListScreen.GetServerList(Strings.DEFAULT_IP_SERVERS);
            }
        }

        internal static void OnGamePause(bool paused)
        {
            if (mSystem.currentTimeMillis() - lastTimeGamePause > 1000 && !isFirstPause)
            {
               /* ModMenuMain.SaveData();
                CustomBackground.SaveData();*/
                if (!Utils.IsOpenedByExternalAccountManager)
                    InGameAccountManager.OnCloseAndPause();
            }
            lastTimeGamePause = mSystem.currentTimeMillis();
            if (isFirstPause)
                isFirstPause = false;
        }

        /// <summary>
        /// Kích hoạt khi game đóng
        /// </summary>
        internal static void OnGameClosing()
        {
            /*ModMenuMain.SaveData();
            CustomBackground.SaveData();

            Setup.clearStringTrash();
            //SocketClient.gI.close();
            TeleportMenuMain.SaveData();
            CustomBackground.SaveData();
            //CustomLogo.SaveData();
            //CustomCursor.SaveData();
            SetDo.SaveData();*/
            VietnameseInput.SaveData();
            //UIReportersManager.ClearReporters();
            if (!Utils.IsOpenedByExternalAccountManager)
                InGameAccountManager.OnCloseAndPause();
        }

        internal static void OnFixedUpdateMain()
        {
            if (GameCanvas.currentScreen != null)
            {
                if (!GameCanvas.panel.isShow && GameCanvas.panel2 != null && GameCanvas.panel2.isShow)
                {
                    GameCanvas.isFocusPanel2 = true;
                    GameCanvas.panel2?.update();
                    if (GameCanvas.panel2?.chatTField != null && GameCanvas.panel2.chatTField.isShow)
                        GameCanvas.panel2?.chatTFUpdateKey();
                    else
                        GameCanvas.panel2?.updateKey();
                }
                if (!GameCanvas.panel.isShow && GameCanvas.panel2 != null && GameCanvas.panel2.isShow)
                    if (!GameCanvas.isPointer(GameCanvas.panel2.X, GameCanvas.panel2.Y, GameCanvas.panel2.W, GameCanvas.panel2.H) && GameCanvas.isPointerJustRelease && GameCanvas.panel2.isDoneCombine)
                        GameCanvas.panel2?.hide();
            }
            CustomBackground.Update();
            //CustomLogo.update();
        }

        internal static void OnUpdateMain()
        {
            if (!Main.started)
                return;
            if (_previousWidth != Screen.width || _previousHeight != Screen.height)
            {
                _previousWidth = Screen.width;
                _previousHeight = Screen.height;
                ScaleGUI.initScaleGUI();
                GameCanvas.instance?.ResetSize();
                Utils.ResetTextField(ChatTextField.gI());
                GameScr.gamePad?.SetGamePadZone();
                GameScr.loadCamera(false, -1, -1);
                //if (GameCanvas.currentScreen is not GameScr)
                //    GameCanvas.loadBG(0);
               /* if (GameCanvas.panel2 != null)
                    GameCanvas.panel2.EmulateSetTypePanel(1);
                ModMenuMain.UpdatePosition();*/
                if (!Utils.IsOpenedByExternalAccountManager)
                    InGameAccountManager.UpdateSizeAndPos();
            }
#if UNITY_ANDROID && !UNITY_EDITOR
            EHVN.FileChooser.Update();
#endif
           /* MainThreadDispatcher.update();
            AutoLogin.Update();*/
            //CustomCursor.Update();
        }

        internal static void OnSaveRMSString(ref string filename, ref string data)
        {
            //if (filename == "acc" || filename == "pass")
            //    data = "pk9r327";
            if (filename.StartsWith("userAo") && string.IsNullOrEmpty(data))
                filename = "";
        }

       

        /// <summary>
        /// Kích hoạt khi cài đăt kích thước màn hình.
        /// </summary>
        /// <returns></returns>
        internal static void OnSetResolution()
        {
            /*if (Utils.IsAndroidBuild())
                return;
            if (Utils.sizeData != null)
            {
                int width = (int)Utils.sizeData["width"];
                int height = (int)Utils.sizeData["height"];
                bool fullScreen = (bool)Utils.sizeData["fullScreen"];
                if (Screen.width != width || Screen.height != height)
                    Screen.SetResolution(width, height, fullScreen);
                new Thread(() =>
                {
                    while (Screen.fullScreen != fullScreen)
                    {
                        Screen.fullScreen = fullScreen;
                        Thread.Sleep(100);
                    }
                }).Start();
            }*/
        }

        /// <summary>
        /// Kích hoạt khi nhấn phím tắt (GameScr) chưa được xử lý.
        /// </summary>
        
        /// <summary>
        /// Kích hoạt khi nhấn phím tắt (GameScr).
        /// </summary>
        

        /// <summary>
        /// Kích hoạt sau khi vẽ khung chat.
        /// </summary>
        internal static void OnPaintChatTextField(ChatTextField instance, mGraphics g)
        {
            /*if (instance == ChatTextField.gI() && instance.strChat.Replace(" ", "") == "Chat" && instance.tfChat.name == "chat")
                HistoryChat.gI.paint(g);*/
        }

        /// <summary>
        /// Kích hoạt khi mở khung chat.
        /// </summary>
        internal static bool OnStartChatTextField(ChatTextField sender, IChatable parentScreen)
        {
            sender.parentScreen = parentScreen;
            if (sender.strChat.Replace(" ", "") != "Chat" || sender.tfChat.name != "chat")
                return false;
            /*if (sender == ChatTextField.gI())
            HistoryChat.gI.show();*/
            return false;
        }
        internal static void OnSessionConnecting(string host, int port)
        {
            if (!Utils.IsOpenedByExternalAccountManager)
            {
                nameCustomServer = "";
                if (InGameAccountManager.SelectedAccount == null)
                    return;
                Server server = InGameAccountManager.SelectedServer;
                if (server == null)
                    return;
                InGameAccountManager.SelectedServer = null;
                if (server.IsCustomIP())
                {
                    host = currentHost = server.hostnameOrIPAddress;
                    port = currentPort = server.port;
                    nameCustomServer = server.name;
                }
                else
                {
                    host = currentHost = ServerListScreen.address[server.index];
                    port = currentPort = (ushort)ServerListScreen.port[server.index];
                    ServerListScreen.ipSelect = server.index;
                }
            }
            else
            {
                if (Utils.server != null)
                {
                    host = (string)Utils.server["ip"];
                    port = (int)Utils.server["port"];
                }
            }
        }

        internal static bool OnGetRMSPath(out string result)
        {
            //result = $"{Application.persistentDataPath}\\{GameMidlet.IP}_{GameMidlet.PORT}_x{mGraphics.zoomLevel}\\";
            string subFolder = $"TeaMobi";
            //string subFolder = $"TeaMobi{Path.DirectorySeparatorChar}Vietnam";

            //if (ServerListScreen.address[ServerListScreen.ipSelect] == "dragon.indonaga.com")
            //{
            //    switch (ServerListScreen.language[ServerListScreen.ipSelect])
            //    {
            //        case 1:
            //            subFolder = $"TeaMobi{Path.DirectorySeparatorChar}World";
            //            break;
            //        case 2:
            //            subFolder = $"TeaMobi{Path.DirectorySeparatorChar}Indonaga";
            //            break;
            //    }
            //}

            result = Utils.GetRootDataPath();
            // check ip server lậu, lưu rms riêng
            // ...
            result = Path.Combine(result, subFolder);
            if (!Directory.Exists(result))
                Directory.CreateDirectory(result);
            return true;
        }

       

        /// <summary>
        /// Kích hoạt khi có ChatTextField update.
        /// </summary>
        internal static void OnUpdateChatTextField(ChatTextField sender)
        {
            if (!string.IsNullOrEmpty(sender.tfChat.getText()))
                GameCanvas.keyPressed[14] = false;
        }

        internal static bool OnClearAllRMS()
        {
            foreach (FileInfo file in new DirectoryInfo(Rms.GetiPhoneDocumentsPath() + "/").GetFiles().Where(f => f.Extension != ".log"))
            {
                try
                {
                    if (file.Name != "isPlaySound")
                        file.Delete();
                }
                catch { }
            }
            return true;
        }

        /// <summary>
        /// Kích hoạt khi <see cref="GameScr.update"/> được gọi.
        /// </summary>
        internal static void OnUpdateGameScr()
        {
            if (!Utils.IsOpenedByExternalAccountManager && GameCanvas.gameTick % (60 * Time.timeScale) == 0)
            {
                Account account = InGameAccountManager.SelectedAccount;
                /*if (account != null && account.Server.hostnameOrIPAddress == currentHost && account.Server.port == currentPort)
                {*/
                account.Gold = Char.myCharz().xu;
                account.Gem = Char.myCharz().luong;
                account.Ruby = Char.myCharz().luongKhoa;

                account.Info.Name = Char.myCharz().cName;
                account.Info.CharID = Char.myCharz().charID;
                account.Info.Gender = (sbyte)Char.myCharz().cgender;
                account.Info.EXP = Char.myCharz().cPower;
                account.Info.MaxHP = Char.myCharz().cHPFull;
                account.Info.MaxMP = Char.myCharz().cMPFull;
                account.Info.Icon = Char.myCharz().avatarz();
                if (Char.myCharz().havePet)
                {
                    if (account.PetInfo == null)
                        account.PetInfo = new AccountManager.CharacterInfo();
                    account.PetInfo.Name = Char.myPetz().cName;
                    account.PetInfo.CharID = Char.myPetz().charID;
                    account.PetInfo.Gender = (sbyte)Utils.GetPetGender();
                    account.PetInfo.EXP = Char.myPetz().cPower;
                    account.PetInfo.MaxHP = Char.myPetz().cHPFull;
                    account.PetInfo.MaxMP = Char.myPetz().cMPFull;
                    account.PetInfo.Icon = Char.myPetz().avatarz();
                }
                else
                    account.PetInfo = null;
                // }
            }
            if (Char.myCharz().havePet && mSystem.currentTimeMillis() - lastTimeRequestPetInfo > delayRequestPetInfo)
            {
                delayRequestPetInfo = Res.random(750, 1000);
                lastTimeRequestPetInfo = mSystem.currentTimeMillis();
                Service.gI().petInfo();
            }
            if (mSystem.currentTimeMillis() - lastTimeRequestZoneInfo > delayRequestZoneInfo)
            {
                delayRequestZoneInfo = Res.random(200, 300);
                lastTimeRequestZoneInfo = mSystem.currentTimeMillis();
                Service.gI().openUIZone();
            }
            Char.myCharz().cspeed = Utils.myCharSpeed;
            //NOTE onUpdateChatTextField không thể bấm tab.
            /*if (ChatTextField.gI().strChat.Replace(" ", "") != "Chat" || ChatTextField.gI().tfChat.name != "chat")
                return;
            HistoryChat.gI.update();*/
        }

        /// <summary>
        /// Kích hoạt khi gửi yêu cầu đăng nhập.
        /// </summary>
        /// <param name="username"></param>
        /// <param name="pass"></param>
        /// <param name="type"></param>
        internal static void OnLogin( string username,  string pass,  sbyte type)
        {
            if (!Utils.IsOpenedByExternalAccountManager)
            {
                if (type == 1)
                {
                    InGameAccountManager.ResetSelectedAccountIndex();
                    Rms.DeleteStorage("acc");
                    Rms.DeleteStorage("pass");
                    return;
                }
                Account acc = InGameAccountManager.SelectedAccount;
                if (acc == null)
                    return;
                type = (sbyte)acc.Type;
                username = acc.Username;
                if (acc.Type == AccountType.Registered)
                    pass = acc.Password;
                else
                {
                    pass = string.Empty;
                    Rms.DeleteStorage("userAo" + acc.Server.index);
                }
                acc.LastTimeLogin = DateTime.Now;
            }
            else
            {
                username = Utils.username == "" ? username : Utils.username;
                if (username.StartsWith("User"))
                {
                    pass = string.Empty;
                    type = 1;
                }
                else
                    pass = Utils.password == "" ? pass : Utils.password;
            }
        }

        /// <summary>
        /// Kích hoạt sau khi màn hình chọn server được load.
        /// </summary>
       

        /// <summary>
        /// Kích hoạt khi Session kết nối đến server.
        /// </summary>
        /// <param name="host"></param>
        /// <param name="port"></param>
        

        internal static void OnScreenDownloadDataShow()
        {
            //GameCanvas.serverScreen.perform(2, null);
        }

        internal static bool OnCheckZoomLevel(int w, int h)
        {
            //mGraphics.zoomLevel = 3;
            //return true;
            if (Utils.IsAndroidBuild())
            {
                if (w * h >= 2073600)
                    mGraphics.zoomLevel = 4;
                else if (w * h >= 691200)
                    mGraphics.zoomLevel = 3;
                else if (w * h > 153600)
                    mGraphics.zoomLevel = 2;
                else
                    mGraphics.zoomLevel = 1;
            }
            else
            {
                mGraphics.zoomLevel = 2;
                if (w * h < 480000)
                    mGraphics.zoomLevel = 1;
            }
            return true;
        }

        internal static bool OnKeyPressed(int keyCode, bool isFromSync)
        {
            if (Utils.channelSyncKey != -1 && !isFromSync)
            {
                //SocketClient.gI.sendMessage(new
                //{
                //    action = "syncKeyPressed",
                //    keyCode,
                //    Utils.channelSyncKey
                //});
            }
            return false;
        }

        internal static bool OnKeyReleased(int keyCode, bool isFromSync)
        {
            if (Utils.channelSyncKey != -1 && !isFromSync)
            {
                //SocketClient.gI.sendMessage(new
                //{
                //    action = "syncKeyReleased",
                //    keyCode,
                //    Utils.channelSyncKey
                //});
            }
            return false;
        }

       
        
        internal static void OnInfoMapLoaded()
        {
            Utils.UpdateWaypointChangeMap();
            GameScr.gI().pts = null;
        }

        

        

        
       

        

        
        internal static bool OnPaintBgGameScr(mGraphics g)
        {
            bool result = false;
            if (GraphicsReducer.Level > ReduceGraphicsLevel.Off || (CustomBackground.isEnabled && CustomBackground.customBgs.Count > 0))
            {
                //GUI.DrawTexture(new Rect(0, 0, Screen.width, Screen.height), Texture2D.blackTexture, ScaleMode.StretchToFill);
                g.setColor(0);
                g.fillRect(0, 0, GameCanvas.w, GameCanvas.h);
                result = true;
            }
            if (CustomBackground.isEnabled && CustomBackground.customBgs.Count > 0)
            {
                CustomBackground.Paint(g);
                result = true;
            }
            return result;
        }

        
        

        internal static bool OnCreateImage(string filename, out Image image)
        {
            string streamingAssetsPath = Application.streamingAssetsPath;
            if (Utils.IsAndroidBuild())
                streamingAssetsPath = Path.Combine(Utils.PersistentDataPath, "StreamingAssets");
            string customAssetsPath = Path.Combine(streamingAssetsPath, "CustomAssets");
            image = new Image();
            Texture2D texture2D;
            if (!Utils.IsEditor() && !Directory.Exists(customAssetsPath))
                Directory.CreateDirectory(customAssetsPath);
            string filePath = Path.Combine(customAssetsPath, filename.Replace('/', Path.DirectorySeparatorChar) + ".png");
            if (File.Exists(filePath))
            {
                texture2D = new Texture2D(1, 1);
                texture2D.LoadImage(File.ReadAllBytes(filePath));
            }
            else
                texture2D = LoadAssetHelper.Load<Texture2D>(filename);
            if (texture2D == null)
                throw new NullReferenceException(nameof(texture2D));
            image.texture = texture2D;
            image.w = image.texture.width;
            image.h = image.texture.height;
            image.texture.anisoLevel = 0;
            image.texture.filterMode = FilterMode.Point;
            image.texture.mipMapBias = 0f;
            image.texture.wrapMode = TextureWrapMode.Clamp;
            return true;
        }

        internal static void OnChatVip(string chatVip)
        {
            ListBoss.AddBoss(chatVip);
        }

        
        internal static void OnPanelHide(Panel instance)
        {

        }

        internal static void OnUpdateKeyPanel(Panel instance)
        {

        }

       
       
      

        internal static bool OnPanelFireOption(Panel panel)
        {
            if (panel.selected >= 0)
            {
                switch (panel.selected)
                {
                    case 0:
                        SoundMn.gI().AuraToolOption();
                        break;
                    case 1:
                        SoundMn.gI().AuraToolOption2();
                        break;
                    case 2:
                        SoundMn.gI().chatVipToolOption();
                        break;
                    case 3:
                        SoundMn.gI().soundToolOption();
                        break;
                    case 4:
                        SoundMn.gI().analogToolOption();
                        break;
                    case 5:
                        SoundMn.gI().CaseSizeScr();
                        break;
                    case 6:
                        GameCanvas.startYesNoDlg(mResources.changeSizeScreen, new Command(mResources.YES, panel, 170391, null), new Command(mResources.NO, panel, 4005, null));
                        break;
                }
            }
            return true;
        }

        internal static bool OnSoundMnGetStrOption()
        {
            Panel.strCauhinh = new string[]
            {
                mResources.aura_off?.Trim() + ": " + Strings.OnOffStatus(Char.isPaintAura),
                mResources.aura_off_2?.Trim() + ": " + Strings.OnOffStatus(Char.isPaintAura2),
                mResources.serverchat_off?.Trim() + ": " + Strings.OnOffStatus(GameScr.isPaintChatVip),
                mResources.turnOffSound?.Trim() + ": " + Strings.OnOffStatus(GameCanvas.isPlaySound),
                mResources.analog?.Trim() + ": " + Strings.OnOffStatus(GameScr.isAnalog != 0),
                (GameCanvas.lowGraphic ? mResources.cauhinhcao : mResources.cauhinhthap)?.Trim(),
                mGraphics.zoomLevel <= 1 ? mResources.x2Screen : mResources.x1Screen,
            };
            return true;
        }

        
       

       

        internal static bool OnPanelPaintToolInfo(mGraphics g)
        {
            mFont.tahoma_7b_white.drawString(g, Strings.communityMod, 60, 4, mFont.LEFT, mFont.tahoma_7b_dark);
            mFont.tahoma_7_yellow.drawString(g, Strings.gameVersion + ": v" + GameMidlet.VERSION, 60, 16, mFont.LEFT, mFont.tahoma_7_grey);
            mFont.tahoma_7_yellow.drawString(g, mResources.character + ": " + Char.myCharz().cName, 60, 27, mFont.LEFT, mFont.tahoma_7_grey);
            Account account = InGameAccountManager.SelectedAccount;
            string serverName = account.Server.IsCustomIP() ? account.Server.name : ServerListScreen.nameServer[account.Server.index];
            mFont.tahoma_7_yellow.drawString(g, mResources.account + " " + mResources.account_server.ToLower() + " " + serverName, 60, 39, mFont.LEFT, mFont.tahoma_7_grey);
            return true;
        }

        internal static bool OnSkillPaint(Skill skill, int x, int y, mGraphics g)
        {
            if (!HideGameUI.isEnabled)
                SmallImage.drawSmallImage(g, skill.template.iconId, x, y, 0, StaticObj.VCENTER_HCENTER);
            long coolingDown = mSystem.currentTimeMillis() - skill.lastTimeUseThisSkill;
            if (coolingDown < skill.coolDown)
            {
                float opacity = .6f;
                int realX = x - 11;
                int realY = y - 11;
                Color color = new Color(0, 0, 0, opacity);
                Color color2 = new Color(0, 0, 0, opacity / 2);
                g.setColor(color2);
                g.fillRect(realX, realY, 22, 22);
                float coolDownRatio = 1 - coolingDown / (float)skill.coolDown;
                CustomGraphics.drawCooldownRect(x, y, 22, 22, coolDownRatio, color);
                string cooldownStr = $"{(skill.coolDown - coolingDown) / 1000f:#.0}".Replace(',', '.');
                if (cooldownStr.Length > 4)
                    cooldownStr = cooldownStr.Substring(0, cooldownStr.IndexOf('.'));
                mFont.tahoma_7_yellow.drawString(g, cooldownStr, x + 1, y - 12 + mFont.tahoma_7.getHeight() / 2, mFont.CENTER);
            }
            else
                skill.paintCanNotUseSkill = false;
            return true;
        }

       

        

        internal static void OnPaintGameCanvas(GameCanvas instance, mGraphics g)
        {
            if (style == null)
            {
                style = new GUIStyle(GUI.skin.label)
                {
                    fontStyle = FontStyle.Bold,
                    fontSize = (int)(8.5 * mGraphics.zoomLevel),
                };
                style.normal.textColor = style.hover.textColor = Color.yellow;
            }
            if (!GameCanvas.panel.isShow)
            {
                if (GameCanvas.panel2 != null)
                {
                    g.translate(-g.getTranslateX(), -g.getTranslateY());
                    g.setClip(0, 0, GameCanvas.w, GameCanvas.h);
                    if (GameCanvas.panel2.isShow)
                        GameCanvas.panel2.paint(g);
                    if (GameCanvas.panel2.chatTField != null && GameCanvas.panel2.chatTField.isShow)
                        GameCanvas.panel2.chatTField.paint(g);
                }
            }

            /*g.setColor(new Color(0.2f, 0.2f, 0.2f, 0.6f));
            double fps = Math.Round((double)(1f / Time.smoothDeltaTime * Time.timeScale), 1);
            string fpsStr = fps.ToString("F1").Replace(',', '.');
            g.fillRect(0, 0, mFont.tahoma_7b_red.getWidth(fpsStr) + 2, 12);
            mFont.tahoma_7b_red.drawString(g, fpsStr, 2, 0, 0);*/
        }

       

       

        private static readonly Dictionary<int, Font> fontCache = new Dictionary<int, Font>();
        private static readonly System.Text.StringBuilder sharedSB = new System.Text.StringBuilder();
        private static string lastHPStr = "";
        private static double lastHPVal = -1;
        private static string lastMPStr = "";
        private static double lastMPVal = -1;

        

        internal static void OnLoadIP()
        {
            ServerListScreen.GetServerList(Strings.DEFAULT_IP_SERVERS);
        }

        

        internal static bool OnServerListScreenInitCommand(ServerListScreen screen)
        {
            screen.nCmdPlay = 0;
            string text = Rms.loadRMSString("acc");
            sbyte[] userAo = Rms.loadRMS("userAo" + ServerListScreen.ipSelect);
            if (text == null)
            {
                if (userAo != null)
                    screen.nCmdPlay = 1;
            }
            else if (text.Equals(string.Empty))
            {
                if (userAo != null)
                    screen.nCmdPlay = 1;
            }
            else
            {
                screen.nCmdPlay = 1;
            }
            screen.cmd = new Command[4 + screen.nCmdPlay];
            int num = GameCanvas.hh - 15 * screen.cmd.Length + 28;
            for (int i = 0; i < screen.cmd.Length; i++)
            {
                switch (i)
                {
                    case 0:
                        screen.cmd[0] = new Command(string.Empty, screen, 3, null);
                        if (string.IsNullOrEmpty(text))
                        {
                            screen.cmd[0].caption = mResources.playNew + "";
                            if (Rms.loadRMS("userAo" + ServerListScreen.ipSelect) != null)
                                screen.cmd[0].caption = mResources.choitiep + "";
                        }
                        else if (!Utils.IsOpenedByExternalAccountManager)
                        {
                            Account acc = InGameAccountManager.SelectedAccount;
                            if (acc == null)
                                screen.cmd[0].caption = mResources.playAcc + ": " + new string('*', text.Length);
                            else
                                screen.cmd[0].caption = mResources.playAcc + ": " + acc.Info.Name;
                        }
                        else
                            screen.cmd[0].caption = mResources.playAcc + ": " + new string('*', text.Length);
                        if (screen.cmd[0].caption.Length > 23)
                        {
                            screen.cmd[0].caption = screen.cmd[0].caption.Substring(0, 23);
                            screen.cmd[0].caption += "...";
                        }
                        break;
                    case 1:
                        if (screen.nCmdPlay == 1)
                        {
                            screen.cmd[1] = new Command(string.Empty, screen, 10100, null);
                            screen.cmd[1].caption = mResources.playNew;
                        }
                        else
                        {
                            if (!Utils.IsOpenedByExternalAccountManager)
                                screen.cmd[1] = new Command(Strings.accounts, new InGameAccountManager.ActionListener(), 7, null);
                            else
                                screen.cmd[1] = new Command(mResources.change_account, screen, 7, null);
                        }
                        break;
                    case 2:
                        if (screen.nCmdPlay == 1)
                        {
                            if (!Utils.IsOpenedByExternalAccountManager)
                                screen.cmd[2] = new Command(Strings.accounts, new InGameAccountManager.ActionListener(), 7, null);
                            else
                                screen.cmd[2] = new Command(mResources.change_account, screen, 7, null);
                        }
                        else
                            screen.cmd[2] = new Command(string.Empty, screen, 17, null);
                        break;
                    case 3:
                        if (screen.nCmdPlay == 1)
                            screen.cmd[3] = new Command(string.Empty, screen, 17, null);
                        else
                            screen.cmd[3] = new Command(mResources.option, screen, 8, null);
                        break;
                    case 4:
                        screen.cmd[4] = new Command(mResources.option, screen, 8, null);
                        break;
                }
                screen.cmd[i].y = num;
                screen.cmd[i].setType();
                screen.cmd[i].x = (GameCanvas.w - screen.cmd[i].w) / 2;
                num += 30;
            }
            return true;
        }

        
       
        internal static bool OnStartOKDlg(string info)
        {
            if (info == LocalizedString.cantChangeZoneInThisMap)
            {
                if (isOpenZoneUI)
                {
                    isOpenZoneUI = false;
                    return false;
                }
                return true;
            }
            return false;
        }

        internal static bool OnRequestChangeMap()
        {
            isOpenZoneUI = false;
            return false;
        }

        internal static bool OnGetMapOffline()
        {
            isOpenZoneUI = false;
            return false;
        }

        internal static bool OnMGraphicsDrawImage(Image image, int x, int y, int anchor)
        {
            if (HideGameUI.isEnabled && !HideGameUI.ShouldDrawImage(image))
                return true;
            if (GraphicsReducer.IsEnabled && !GraphicsReducer.ShouldDrawImage(image))
                return true;
            return false;
        }

    }
}
