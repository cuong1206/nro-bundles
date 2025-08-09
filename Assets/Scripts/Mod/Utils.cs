using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Newtonsoft.Json.Linq;
using UnityEngine;

namespace Mod
{
    internal static class Utils
    {
        static string persistentDataPath = Application.persistentDataPath;
        internal static string PersistentDataPath => persistentDataPath; 

        internal static readonly string CommonModDataPath = Path.Combine(GetRootDataPath(), "CommonModData");

        internal static readonly string PathAutoChat = Path.Combine(CommonModDataPath, "autochat.txt");
        internal static readonly string PathChatCommand = Path.Combine(CommonModDataPath, "chatCommands.json");
        internal static readonly string PathChatHistory = Path.Combine(CommonModDataPath, "chat.txt");
        internal static readonly string PathHotkeyCommand = Path.Combine(CommonModDataPath, "hotkeyCommands.json");

        internal static readonly sbyte ID_SKILL_BUFF = 7;
        internal static readonly short ID_ICON_ITEM_TDLT = 4387;
        internal static readonly short ID_NPC_MOD_FACE = 1571;    // Doraemon

        internal static string status = "Đã kết nối";

        internal static int myCharSpeed = 8;
        internal static JObject server = null;
        internal static Waypoint waypointLeft;
        internal static Waypoint waypointMiddle;
        internal static Waypoint waypointRight;

        internal static string username = "";
        internal static string password = "";

        internal static int channelSyncKey = -1;

        internal static System.Random random = new System.Random();

        static bool isOpenedByExternalAccountManager;
        internal static bool IsOpenedByExternalAccountManager => isOpenedByExternalAccountManager;


        /// <summary>
        /// Kiểm tra xem game đang chạy trên Android hay không.
        /// </summary>
        /// <returns>Trả về true nếu đang chạy trên Android, ngược lại trả về false.</returns>
        internal static bool IsAndroidBuild() => Application.platform == RuntimePlatform.Android;

        /// <summary>
        /// Kiểm tra xem game đang chạy trên Linux hay không.
        /// </summary>
        /// <returns>Trả về true nếu đang chạy trên Linux, ngược lại trả về false.</returns>
        internal static bool IsLinuxBuild() => Application.platform == RuntimePlatform.LinuxPlayer;

        /// <summary>
        /// Kiểm tra xem game đang chạy trên Windows hay không.
        /// </summary>
        /// <returns>Trả về true nếu đang chạy trên Windows, ngược lại trả về false.</returns>
        internal static bool IsWindowsBuild() => Application.platform == RuntimePlatform.WindowsPlayer;

        /// <summary>
        /// Kiểm tra xem game có đang chạy trên Unity Editor hay không.
        /// </summary>
        /// <returns>Trả về true nếu game đang chạy trên Editor, ngược lại trả về false.</returns>
        internal static bool IsEditor() => Application.isEditor;

        /// <summary>
        /// Kiểm tra xem game đang chạy trên điện thoại hay không.
        /// </summary>
        /// <returns>Trả về true nếu đang chạy trên điện thoại, ngược lại trả về false.</returns>
        internal static bool IsMobile() => IsAndroidBuild() || Application.platform == RuntimePlatform.IPhonePlayer;

        /// <summary>
        /// Kiểm tra xem game đang chạy trên PC hay không.
        /// </summary>
        /// <returns>Trả về true nếu đang chạy trên PC, ngược lại trả về false.</returns>
        internal static bool IsPC() => !IsMobile();

        internal static void CheckBackButtonPress()
        {
            if (GameCanvas.panel != null || GameCanvas.panel2 != null)
            {
                if (GameCanvas.panel != null && GameCanvas.panel.isShow)
                {
                    GameCanvas.panel.hide();
                    return;
                }
                if (GameCanvas.panel2 != null && GameCanvas.panel2.isShow)
                {
                    GameCanvas.panel2.hide();
                    return;
                }
            }
            if (InfoDlg.isShow)
                return;
            if (GameCanvas.currentDialog != null && GameCanvas.currentDialog is MsgDlg)
            {
                GameCanvas.endDlg();
                return;
            }
            if (ChatTextField.gI().isShow)
            {
                ChatTextField.gI().close();
                return;
            }
            if (GameCanvas.menu.showMenu)
            {
                GameCanvas.menu.doCloseMenu();
                return;
            }
            GameCanvas.checkBackButton();
        }

        #region Get info
        /// <summary>
        /// Lấy MyVector chứa nhân vật của người chơi.
        /// </summary>
        /// <returns></returns>
        internal static MyVector getMyVectorMe()
        {
            var vMe = new MyVector();
            vMe.addElement(Char.myCharz());
            return vMe;
        }

        /// <summary>
        /// Kiểm tra khả năng sử dụng skill Trị thương vào bản thân.
        /// </summary>
        /// <param name="skillBuff">Skill trị thương.</param>
        /// <returns>true nếu có thể sử dụng skill trị thương vào bản thân.</returns>
        internal static bool canBuffMe(out Skill skillBuff)
        {
            skillBuff = Char.myCharz().
                getSkill(new SkillTemplate { id = ID_SKILL_BUFF });

            if (skillBuff == null)
            {
                return false;
            }

            return true;
        }

        internal static string getTextPopup(PopUp popUp)
        {
            StringBuilder stringBuilder = new StringBuilder();
            for (int i = 0; i < popUp.says.Length; i++)
            {
                stringBuilder.Append(popUp.says[i]);
                stringBuilder.Append(" ");
            }
            return stringBuilder.ToString().Trim();
        }

        /// <summary>
        /// Kiểm tra trạng thái sử dụng TĐLT.
        /// </summary>
        /// <returns>true nếu đang sử dụng tự động luyện tập</returns>
        internal static bool isUsingTDLT() =>
            ItemTime.isExistItem(ID_ICON_ITEM_TDLT);

        /// <summary>
        /// Sử dụng một item có id là một trong số các id truyền vào.
        /// </summary>
        /// <param name="templatesId">Mảng chứa các id của các item muốn sử dụng.</param>
        /// <returns>true nếu có vật phẩm được sử dụng.</returns>
        internal static sbyte getIndexItemBag(params short[] templatesId)
        {
            var myChar = Char.myCharz();
            int length = myChar.arrItemBag.Length;
            for (sbyte i = 0; i < length; i++)
            {
                var item = myChar.arrItemBag[i];
                if (item != null && templatesId.Contains(item.template.id))
                {
                    return i;
                }
            }

            return -1;
        }
        #endregion

        /// <summary>
        /// Dịch chuyển tới npc trong map.
        /// </summary>
        /// <param name="npc">Npc cần dịch chuyển tới</param>
        internal static void teleToNpc(Npc npc)
        {
            TeleportMyChar(npc.cx, npc.ySd - npc.ySd % 24);
            Char.myCharz().npcFocus = npc;
        }

        internal static void requestChangeMap(Waypoint waypoint)
        {
            if (waypoint.isOffline)
            {
                Service.gI().getMapOffline();
                return;
            }

            Service.gI().requestChangeMap();
        }

        internal static void setWaypointChangeMap(Waypoint waypoint)
        {
            int cMapID = TileMap.mapID;
            var textPopup = getTextPopup(waypoint.popup);

            if (cMapID == 27 && textPopup == TileMap.mapNames[53])
                return;

            if (cMapID == 70 && textPopup == TileMap.mapNames[69] ||
                cMapID == 73 && textPopup == TileMap.mapNames[67] ||
                cMapID == 110 && textPopup == TileMap.mapNames[106])
            {
                waypointLeft = waypoint;
                return;
            }

            if (((cMapID == 106 || cMapID == 107) && textPopup == TileMap.mapNames[110]) ||
                ((cMapID == 105 || cMapID == 108) && textPopup == TileMap.mapNames[109]) ||
                (cMapID == 109 && textPopup == TileMap.mapNames[105]))
            {
                waypointMiddle = waypoint;
                return;
            }

            if (cMapID == 70 && textPopup == TileMap.mapNames[71])
            {
                waypointRight = waypoint;
                return;
            }

            if (waypoint.maxX < 60)
            {
                waypointLeft = waypoint;
                return;
            }

            if (waypoint.minX > TileMap.pxw - 60)
            {
                waypointRight = waypoint;
                return;
            }

            waypointMiddle = waypoint;
        }

        internal static void UpdateWaypointChangeMap()
        {
            waypointLeft = waypointMiddle = waypointRight = null;

            if (TileMap.mapID == 46)
                waypointRight = new Waypoint(570, 576, 570, 576, true, false, TileMap.mapNames[47]);

            var vGoSize = TileMap.vGo.size();
            if (vGoSize == 0)
            {
                if (TileMap.mapID == 45)
                    waypointMiddle = new Waypoint(570, 576, 570, 576, true, false, TileMap.mapNames[46]);
            }
            for (int i = 0; i < vGoSize; i++)
            {
                Waypoint waypoint = (Waypoint)TileMap.vGo.elementAt(i);
                setWaypointChangeMap(waypoint);
            }
        }


        internal static void buffMe()
        {
            if (!canBuffMe(out Skill skillBuff))
            {
                GameScr.info1.addInfo("Không tìm thấy kỹ năng Trị thương", 0);
                return;
            }

            // Đổi sang skill hồi sinh
            Service.gI().selectSkill(ID_SKILL_BUFF);

            // Tự tấn công vào bản thân
            Service.gI().sendPlayerAttack(new MyVector(), getMyVectorMe(), -1);

            // Trả về skill cũ
            Service.gI().selectSkill(Char.myCharz().myskill.template.id);

            // Đặt thời gian hồi cho skill
            skillBuff.lastTimeUseThisSkill = mSystem.currentTimeMillis();
        }

        /// <summary>
        /// Dịch chuyển tới một toạ độ cụ thể trong map.
        /// </summary>
        /// <param name="x">Toạ độ x.</param>
        /// <param name="y">Toạ độ y.</param>
        internal static void TeleportMyChar(int x, int y)
        {
            Char.myCharz().currentMovePoint = null;
            Char.myCharz().cx = x;
            Char.myCharz().cy = y;
            Service.gI().charMove();

            /*if (isUsingTDLT())
                return;*/

            Char.myCharz().cx = x;
            Char.myCharz().cy = y + 1;
            Service.gI().charMove();
            Char.myCharz().cx = x;
            Char.myCharz().cy = y;
            Service.gI().charMove();
        }

        
        

        internal static bool IsMeInNRDMap() => TileMap.mapID >= 85 && TileMap.mapID <= 91;

        internal static long LoadDataLong(string name, bool isCommon = true)
        {
            string path = GetFullPath(name, isCommon);
            if (!File.Exists(path))
                return 0; // Giá trị mặc định nếu file không tồn tại

            using (FileStream fileStream = new FileStream(path, FileMode.Open))
            {
                byte[] array = new byte[8];
                fileStream.Read(array, 0, array.Length);
                return BitConverter.ToInt64(array, 0);
            }
        }

        internal static bool LoadDataBool(string name, bool isCommon = true)
        {
            string path = GetFullPath(name, isCommon);
            if (!File.Exists(path))
                return false; // Giá trị mặc định nếu file không tồn tại

            using (FileStream fileStream = new FileStream(path, FileMode.Open))
            {
                byte[] array = new byte[1];
                fileStream.Read(array, 0, 1);
                return array[0] == 1;
            }
        }
        internal static bool LoadDataBool2(string name, bool isCommon = true)
        {
            string path = GetFullPath(name, isCommon);
            if (!File.Exists(path))
                return true; // Giá trị mặc định nếu file không tồn tại

            using (FileStream fileStream = new FileStream(path, FileMode.Open))
            {
                byte[] array = new byte[1];
                fileStream.Read(array, 0, 1);
                return array[0] == 1;
            }
        }
        internal static string LoadDataString(string name, bool isCommon = true)
        {
            string path = GetFullPath(name, isCommon);
            if (!File.Exists(path))
                return string.Empty; // Giá trị mặc định nếu file không tồn tại

            using (FileStream fileStream = new FileStream(path, FileMode.Open))
            using (StreamReader streamReader = new StreamReader(fileStream))
            {
                return streamReader.ReadToEnd();
            }
        }

        internal static double LoadDataDouble(string name, bool isCommon = true)
        {
            string path = GetFullPath(name, isCommon);
            if (!File.Exists(path))
                return 0.0; // Giá trị mặc định nếu file không tồn tại

            using (FileStream fileStream = new FileStream(path, FileMode.Open))
            {
                byte[] array = new byte[8];
                fileStream.Read(array, 0, array.Length);
                return BitConverter.ToDouble(array, 0);
            }
        }

        private static string GetFullPath(string name, bool isCommon)
        {
            string path = CommonModDataPath;
            if (!isCommon)
                path = Path.Combine(Rms.GetiPhoneDocumentsPath(), "ModData");
            return Path.Combine(path, name);
        }


        /*internal static bool TryLoadDataLong(string name, out long value, bool isCommon = true)
        {
            value = default;
            try
            {
                value = LoadDataLong(name, isCommon);
                return true;
            }
            catch (Exception ex) { Debug.LogException(ex); }
            return false;
        }*/

        internal static bool TryLoadDataBool(string name, out bool value, bool isCommon = true)
        {
            string path = GetFullPath(name, isCommon);
            if (!File.Exists(path))
            {
                value = default; // Không cần dùng cũng được vì sẽ không gán
                return false; // Quan trọng: trả về false để LoadData không gán
            }

            try
            {
                value = LoadDataBool(name, isCommon);
                return true;
            }
            catch (Exception ex)
            {
                Debug.LogException(ex);
                value = default;
                return false;
            }
        }


        internal static bool TryLoadDataString(string name, out string value, bool isCommon = true)
        {
            value = default;
            try
            {
                value = LoadDataString(name, isCommon);
                return true;
            }
            catch (Exception ex) { Debug.LogException(ex); }
            return false;
        }

        internal static bool TryLoadDataLong(string name, out long value, bool isCommon = true)
        {
            string path = GetFullPath(name, isCommon);
            if (!File.Exists(path))
            {
                value = default;
                return false;
            }

            try
            {
                value = LoadDataLong(name, isCommon);
                return true;
            }
            catch (Exception ex)
            {
                Debug.LogException(ex);
                value = default;
                return false;
            }
        }

        internal static bool TryLoadDataDouble(string name, out double value, bool isCommon = true)
        {
            string path = GetFullPath(name, isCommon);
            if (!File.Exists(path))
            {
                value = default;
                return false;
            }

            try
            {
                value = LoadDataDouble(name, isCommon);
                return true;
            }
            catch (Exception ex)
            {
                Debug.LogException(ex);
                value = default;
                return false;
            }
        }

        internal static void SaveData(string name, long value, bool isCommon = true)
        {
            string path = CommonModDataPath;
            if (!isCommon)
                path = Path.Combine(Rms.GetiPhoneDocumentsPath(), "ModData");
            if (!Directory.Exists(path))
                Directory.CreateDirectory(path);
            FileStream fileStream = new FileStream(Path.Combine(path, name), FileMode.Create);
            fileStream.Write(BitConverter.GetBytes(value), 0, 8);
            fileStream.Flush();
            fileStream.Close();
        }

        internal static void SaveData(string name, bool status, bool isCommon = true)
        {
            string path = CommonModDataPath;
            if (!isCommon)
                path = Path.Combine(Rms.GetiPhoneDocumentsPath(), "ModData");
            if (!Directory.Exists(path))
                Directory.CreateDirectory(path);
            FileStream fileStream = new FileStream(Path.Combine(path, name), FileMode.Create);
            fileStream.Write(new byte[] { (byte)(status ? 1 : 0) }, 0, 1);
            fileStream.Flush();
            fileStream.Close();
        }

        internal static void SaveData(string name, string data, bool isCommon = true)
        {
            string path = CommonModDataPath;
            if (!isCommon)
                path = Path.Combine(Rms.GetiPhoneDocumentsPath(), "ModData");
            if (!Directory.Exists(path))
                Directory.CreateDirectory(path);
            FileStream fileStream = new FileStream(Path.Combine(path, name), FileMode.Create);
            byte[] buffer = Encoding.UTF8.GetBytes(data);
            fileStream.Write(buffer, 0, buffer.Length);
            fileStream.Flush();
            fileStream.Close();
        }

        internal static void SaveData(string name, double value, bool isCommon = true)
        {
            string path = CommonModDataPath;
            if (!isCommon)
                path = Path.Combine(Rms.GetiPhoneDocumentsPath(), "ModData");
            if (!Directory.Exists(path))
                Directory.CreateDirectory(path);
            FileStream fileStream = new FileStream(Path.Combine(path, name), FileMode.Create);
            fileStream.Write(BitConverter.GetBytes(value), 0, 8);
            fileStream.Flush();
            fileStream.Close();
        }

        /// <summary>
        /// Dịch chuyển đến đối tượng trong map
        /// </summary>
        /// <param name="obj">Đối tượng cần dịch chuyển tới</param>
        internal static void TeleportMyChar(IMapObject obj)
        {
            TeleportMyChar(obj.getX(), obj.getY());
        }

        /// <summary>
        /// Dịch chuyển đến vị trí trên mặt đất có hoành độ x
        /// </summary>
        /// <param name="x">Hoành độ</param>
        internal static void TeleportMyChar(int x)
        {
            TeleportMyChar(x, GetYGround(x));
        }

        static GUIContent cachedContent = new GUIContent();

        internal static int getWidth(GUIStyle gUIStyle, string s)
        {
            cachedContent.text = s;
            return (int)(gUIStyle.CalcSize(cachedContent).x * 1.025f / mGraphics.zoomLevel);
        }


        internal static int getHeight(GUIStyle gUIStyle, string content)
        {
            return (int)gUIStyle.CalcSize(new GUIContent(content)).y / mGraphics.zoomLevel;
        }

        /// <summary>
        /// Lấy tung độ mặt đất từ hoành độ
        /// </summary>
        /// <param name="x">Hoành độ x</param>
        /// <returns>Tung độ y thỏa mãn (x, y) là mặt đất</returns>
        internal static int GetYGround(int x)
        {
            int y = 50;
            for (int i = 0; i < 30; i++)
            {
                y += 24;
                if (TileMap.tileTypeAt(x, y, 2))
                {
                    if (y % 24 != 0)
                        y -= y % 24;
                    break;
                }
            }
            return y;
        }

        internal static int Distance(IMapObject mapObject1, IMapObject mapObject2)
        {
            return Res.distance(mapObject1.getX(), mapObject1.getY(), mapObject2.getX(), mapObject2.getY());
        }


        internal static short getNRSDId()
        {
            if (IsMeInNRDMap()) return (short)(2400 - TileMap.mapID);
            return 0;
        }

        internal static bool isMeWearingActivationSet(int idSet)
        {
            int activateCount = 0;
            for (int i = 0; i < 5; i++)
            {
                Item item = Char.myCharz().arrItemBody[i];
                if (item == null) return false;
                if (item.itemOption == null) return false;
                for (int j = 0; j < item.itemOption.Length; j++)
                {
                    if (item.itemOption[j].optionTemplate.id == idSet)
                    {
                        activateCount++;
                        break;
                    }
                }
            }
            return activateCount == 5;
        }

        internal static bool isMeWearingTXHSet() => Char.myCharz().cgender == 0 && isMeWearingActivationSet(127);

        internal static bool isMeWearingPikkoroDaimaoSet() => Char.myCharz().cgender == 1 && isMeWearingActivationSet(132);

        internal static bool isMeWearingCadicSet() => Char.myCharz().cgender == 2 && isMeWearingActivationSet(134);

        internal static void DoDoubleClickToObj(IMapObject mapObject) => GameScr.gI().doDoubleClickToObj(mapObject);

        internal static bool CanNextMap() => !Char.isLoadingMap && !Char.ischangingMap && !Controller.isStopReadMessage;

        

        /// <summary>
        /// Lấy hệ của đệ tử bằng cách kiểm tra skill 1
        /// </summary>
        /// <returns></returns>
        internal static int GetPetGender()
        {
            if (Char.myPetz().arrPetSkill != null && Char.myPetz().arrPetSkill.Length > 0)
            {
                string skill1Pet = Char.myPetz().arrPetSkill[0].template.name;
                if (skill1Pet == GameScr.nClasss[0].skillTemplates[0].name)
                    return GameScr.nClasss[0].classId;
                if (skill1Pet == GameScr.nClasss[1].skillTemplates[0].name)
                    return GameScr.nClasss[1].classId;
                if (skill1Pet == GameScr.nClasss[2].skillTemplates[0].name)
                    return GameScr.nClasss[2].classId;
            }
            return 3;
        }

        internal static string TrimUntilFit(string str, GUIStyle style, int width)
        {
            if (string.IsNullOrEmpty(str)) return "...";

            cachedContent.text = str;
            float fullWidth = style.CalcSize(cachedContent).x / mGraphics.zoomLevel;
            if (fullWidth <= width) return str;

            // Tính toán trước phần hậu tố "..."
            const string ellipsis = "...";
            int len = str.Length;

            // Tránh tạo GC trong vòng lặp: dùng Substring cố định, không + "..."
            while (len > 0)
            {
                cachedContent.text = str.Substring(0, len) + ellipsis;
                float testWidth = style.CalcSize(cachedContent).x / mGraphics.zoomLevel;
                if (testWidth <= width)
                    break;
                len--;
            }

            return (len > 0 ? str.Substring(0, len) : "") + ellipsis;
        }

        internal static long GetLastTimePress()
        {
            return GameCanvas.lastTimePress;
        }
        internal static bool HasActivateOption(Item item)
        {
            if (item.itemOption == null)
                return false;
            for (int i = 0; i < item.itemOption.Length; i++)
            {
                if (item.itemOption[i].optionTemplate.id >= 127 && item.itemOption[i].optionTemplate.id <= 144)
                    return true;
            }
            return false;
        }
        private static readonly System.Text.StringBuilder sb = new System.Text.StringBuilder(256);
        public static string Fmt(params object[] parts)
        {
            sb.Clear();
            foreach (var part in parts)
                sb.Append(part);
            return sb.ToString();
        }
        internal static Char FindCharInMap(string name)
        {
            for (int i = 0; i < GameScr.vCharInMap.size(); i++)
            {
                Char ch = (Char)GameScr.vCharInMap.elementAt(i);
                if (ch.GetNameWithoutClanTag() == name)
                    return ch;
            }
            return null;
        }

        internal static bool IsMyCharHome() => TileMap.mapID == Char.myCharz().cgender + 21;

        internal static string FormatWithSIPrefix(double number)
        {
            string[] prefix = new string[] { "", "k", "M", "B", "T" };
            int degree = System.Math.Max(0, System.Math.Min((int)System.Math.Floor(System.Math.Log10(System.Math.Abs(number)) / 3), prefix.Length - 1));
            double scaled = number * System.Math.Pow(1000, -degree);
            return $"{scaled:0.##}{prefix[degree]}";
        }

        internal static void ResetTextField(ChatTextField chatTextField)
        {
            if (chatTextField == null)
                return;
            chatTextField.left = new Command(mResources.OK, chatTextField, 8000, null, 1, GameCanvas.h - mScreen.cmdH + 1);
            chatTextField.right = new Command(mResources.DELETE, chatTextField, 8001, null, GameCanvas.w - 70, GameCanvas.h - mScreen.cmdH + 1);
            chatTextField.center = null;
            chatTextField.w = chatTextField.tfChat.width + 20;
            chatTextField.h = chatTextField.tfChat.height + 26;
            chatTextField.x = GameCanvas.w / 2 - chatTextField.w / 2;
            chatTextField.tfChat.y = GameCanvas.h - 40 - chatTextField.tfChat.height;
            chatTextField.y = chatTextField.tfChat.y - 18;
            if (Main.isPC && chatTextField.w > 320)
                chatTextField.w = 320;
            chatTextField.left.x = chatTextField.x;
            chatTextField.right.x = chatTextField.x + chatTextField.w - 68;
            if (GameCanvas.isTouch)
            {
                //tfChat.y -= 5;
                chatTextField.y -= 15;
                chatTextField.h += 30;
                chatTextField.left.x = GameCanvas.w / 2 - 68 - 5;
                chatTextField.right.x = GameCanvas.w / 2 + 5;
                chatTextField.left.y = GameCanvas.h - 30;
                chatTextField.right.y = GameCanvas.h - 30;
            }
            chatTextField.yBegin = chatTextField.tfChat.y;
            chatTextField.yUp = GameCanvas.h / 2 - 2 * chatTextField.tfChat.height;
            if (Main.isWindowsPhone)
                chatTextField.tfChat.showSubTextField = false;
            if (Main.isIPhone)
                chatTextField.tfChat.isPaintMouse = false;
            chatTextField.tfChat.name = "chat";
            if (Main.isWindowsPhone)
                chatTextField.tfChat.strInfo = chatTextField.tfChat.name;
            chatTextField.tfChat.width = GameCanvas.w - 6;
            if (Main.isPC && chatTextField.tfChat.width > 250)
                chatTextField.tfChat.width = 250;
            chatTextField.tfChat.height = mScreen.ITEM_HEIGHT + 2;
            chatTextField.tfChat.x = GameCanvas.w / 2 - chatTextField.tfChat.width / 2;
            chatTextField.tfChat.isFocus = true;
            chatTextField.tfChat.setMaxTextLenght(80);
        }

        internal static string GetRootDataPath()
        {
            string result = Path.Combine(Path.GetDirectoryName(Application.dataPath), "Data");
            /*if (IsEditor() || IsAndroidBuild())*/
                result = PersistentDataPath;
            return result;
        }
        public static void AppendMoneyFormat(StringBuilder sb, long m)
        {
            if (m < 1000)
            {
                sb.Append(m);
                return;
            }
// Mảng chứa từng nhóm 3 số: 567, 234, 1
Span<int> groups = stackalloc int[8]; // tối đa cho 999.999.999.999
            int index = 0;

            while (m >= 1000)
            {
                groups[index++] = (int)(m % 1000);
                m /= 1000;
            }

            sb.Append(m);

            for (int i = index - 1; i >= 0; i--)
            {
                sb.Append('.');
                int g = groups[i];
                if (g < 10) sb.Append("00");
                else if (g < 100) sb.Append('0');
                sb.Append(g);
            }
        }
        public static void AppendMoneyFormat(StringBuilder sb, double m)
        {
            if (m < 1000)
            {
                sb.Append(System.Math.Floor(m));
                double decimalPart = m - System.Math.Floor(m);
                if (decimalPart > 0)
                {
                    sb.Append(',');
                    sb.Append(((int)(decimalPart * 100)).ToString("00")); // hiển thị 2 chữ số
                }
                return;
            }

            Span<int> groups = stackalloc int[8]; // Tối đa cho 999.999.999.999
            int index = 0;

            long whole = (long)System.Math.Floor(m);
            double decimalPartDouble = m - whole;

            while (whole >= 1000)
            {
                groups[index++] = (int)(whole % 1000);
                whole /= 1000;
            }

            sb.Append(whole);

            for (int i = index - 1; i >= 0; i--)
            {
                sb.Append('.');
                int g = groups[i];
                if (g < 10) sb.Append("00");
                else if (g < 100) sb.Append('0');
                sb.Append(g);
            }

            // Nếu có phần thập phân thì thêm vào
            if (decimalPartDouble > 0)
            {
                sb.Append(',');
                sb.Append(((int)(decimalPartDouble * 100)).ToString("00")); // hiển thị 2 số sau dấu phẩy
            }
        }

        private static Color GetColorFromHex(string hex)
        {
            if (!colorCache.TryGetValue(hex, out var c))
            {
                if (hex.StartsWith("#")) hex = hex.Substring(1);
                byte r = byte.Parse(hex.Substring(0, 2), System.Globalization.NumberStyles.HexNumber);
                byte g = byte.Parse(hex.Substring(2, 2), System.Globalization.NumberStyles.HexNumber);
                byte b = byte.Parse(hex.Substring(4, 2), System.Globalization.NumberStyles.HexNumber);
                byte a = (hex.Length >= 8) ? byte.Parse(hex.Substring(6, 2), System.Globalization.NumberStyles.HexNumber) : (byte)255;
                c = new Color32(r, g, b, a);
                colorCache[hex] = c;
            }
            return c;
        }
        private static readonly GUIStyle sharedMainStyle = new();
        private static readonly GUIStyle sharedOutlineStyle = new();
        private static readonly Dictionary<string, Color> colorCache = new();

        private static Color GetColor(string hex)
        {
            if (!colorCache.TryGetValue(hex, out var color))
            {
                if (hex.StartsWith("#")) hex = hex.Substring(1);
                byte r = byte.Parse(hex.Substring(0, 2), System.Globalization.NumberStyles.HexNumber);
                byte g = byte.Parse(hex.Substring(2, 2), System.Globalization.NumberStyles.HexNumber);
                byte b = byte.Parse(hex.Substring(4, 2), System.Globalization.NumberStyles.HexNumber);
                byte a = (hex.Length == 8) ? byte.Parse(hex.Substring(6, 2), System.Globalization.NumberStyles.HexNumber) : (byte)255;
                color = new Color32(r, g, b, a);
                colorCache[hex] = color;
            }
            return color;
        }
        public static void DrawStringWithOutline(mGraphics g, string s, int x, int y, GUIStyle baseStyle, string textHex, string outlineHex, int align)
        {
            // Canh giữa nếu cần
            if (align == 3)
                x -= Utils.getWidth(baseStyle, s) / 2;
            sharedMainStyle.font = baseStyle.font;
            sharedMainStyle.fontSize = baseStyle.fontSize;
            sharedMainStyle.richText = baseStyle.richText;
            sharedMainStyle.alignment = baseStyle.alignment;
            sharedMainStyle.normal.textColor = GetColor(textHex);

            sharedOutlineStyle.font = baseStyle.font;
            sharedOutlineStyle.fontSize = baseStyle.fontSize;
            sharedOutlineStyle.richText = baseStyle.richText;
            sharedOutlineStyle.alignment = baseStyle.alignment;
            sharedOutlineStyle.normal.textColor = GetColor(outlineHex);

            g.drawString(s, x - 1, y, sharedOutlineStyle);
            g.drawString(s, x + 1, y, sharedOutlineStyle);
            g.drawString(s, x, y - 1, sharedOutlineStyle);
            g.drawString(s, x, y + 1, sharedOutlineStyle);
            g.drawString(s, x, y, sharedMainStyle);
        }
        public static Color HexToColor(string hex)
        {
            if (hex.StartsWith("#")) hex = hex.Substring(1);
            if (hex.Length != 6 && hex.Length != 8)
                throw new ArgumentException("Mã HEX không hợp lệ");

            byte r = byte.Parse(hex.Substring(0, 2), System.Globalization.NumberStyles.HexNumber);
            byte g = byte.Parse(hex.Substring(2, 2), System.Globalization.NumberStyles.HexNumber);
            byte b = byte.Parse(hex.Substring(4, 2), System.Globalization.NumberStyles.HexNumber);
            byte a = (hex.Length == 8) ? byte.Parse(hex.Substring(6, 2), System.Globalization.NumberStyles.HexNumber) : (byte)255;

            return new Color32(r, g, b, a);
        }
        internal static double Distance(double x1, double y1, double x2, double y2) => System.Math.Sqrt((x1 - x2) * (x1 - x2) + (y1 - y2) * (y1 - y2));
    }
}