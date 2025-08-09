
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using Mod.XMAP;
using UnityEngine;

namespace Mod
{
    public class ListBoss
    {
        public string name;

        public string map;

        public int mapId;

        public int zoneId = -1;

        public DateTime AppearTime;

        public bool isDied;

        public string killer;

        public static List<ListBoss> listBosses = new List<ListBoss>();

        public static bool isEnabled = true;

        public static int distanceBetweenLines = 8;

        static int offset = 0;

        public static int x = 15 - 9;

        public static int y = 0;

        static int maxLength = 0;

        static int lastBoss = -1;

        public static bool isCollapsed;

        public static readonly int MAX_BOSS_DISPLAY = 5;

        static readonly int MAX_BOSS = 100;

        static readonly string LIST_BOSS = "Danh sách Boss";

        static GUIStyle collapsedStyle;

        static int listBossWidth = 0;

        static int titleWidth;

        static int offsetX;

        static readonly System.Text.StringBuilder sharedSB = new System.Text.StringBuilder(256);


        static readonly List<string> strBossHasBeenKilled = new List<string>()
        {
            " mọi người đều ngưỡng mộ.",
            " everyone admired.",
            " semua orang mengagumi.",
            " đã đánh bại và nhận được cải trang thành ",
            " killed and receive disguise of ",
            " membunuh Dan menerima disguise ",
            ": Đã tiêu diệt được ",
            ": defeated ",
            ": mengalahkan ",
        };

        static readonly List<string> strBossAppeared = new List<string>()
        {
            "BOSS ",
            " vừa xuất hiện tại ",
            " appear at ",
            " muncul di ",
            " khu vực ",
            " zone ",
            //Google translated: daerah, in-game menu: zona ???
            " zona ",
        };

        ListBoss(string name, string map)
        {
            this.name = name;
            this.map = map;
            GetMapId(name, map);
            AppearTime = DateTime.Now;
        }
        
        private void GetMapId(string name, string map)
        {
            if (map == "Vách núi Aru")
                mapId = 42;
            else if (map == "Vách núi Moori")
                mapId = 43;
            else if (map == "Trạm tàu vũ trụ")
            {
                if (name.StartsWith("Số ") || name.StartsWith("Tiểu đội"))
                    mapId = 25;
                else if (name.Contains("Bojack") || name.StartsWith("Bujin") || name.StartsWith("Bido") || name.StartsWith("Zangya") || name.StartsWith("Bido"))
                    mapId = 24;
            }
            else mapId = GetMapID(map);
        }

        public static void AddBoss(string chatVip)
        {
            if (strBossHasBeenKilled.Any(chatVip.Contains))
            {
                strBossHasBeenKilled.ForEach(s => chatVip = chatVip.Replace(s, "|"));
                string[] array = chatVip.Split('|');
                ListBoss boss = null;
                try
                {
                    boss = listBosses.Last(b =>
                    {
                        if (new int[] { 79, 82, 83 }.Contains(b.mapId))
                        {
                            if (Regex.IsMatch(b.name, "(Tiểu đội trưởng|(Captain|Kapten) Ginyu|Số [1-4]|Jeice|Burter|Recoome|Guldo)"))
                                return false;
                        }
                        return b.name == array[1] && string.IsNullOrEmpty(b.killer);
                    });
                }
                catch (InvalidOperationException) { }
                if (boss == null)
                {
                    boss = new ListBoss(array[1], "");
                    listBosses.Add(boss);
                }
                boss.isDied = true;
                boss.killer = array[0];
            }
            else if (chatVip.StartsWith(strBossAppeared[0]))
            {
                strBossAppeared.ForEach(s => chatVip = chatVip.Replace(s, "|"));
                string[] array = chatVip.Split('|');
                ListBoss boss = null;
                try
                {
                    int mapId = GetMapID(array[2]);
                    boss = listBosses.Last(b =>
                    {
                        if (new int[] { 79, 82, 83 }.Contains(mapId))
                        {
                            if (Regex.IsMatch(array[1], "(Tiểu đội trưởng|(Captain|Kapten) Ginyu|Số [1-4]|Jeice|Burter|Recoome|Guldo)"))
                                return false;
                        }
                        return string.IsNullOrEmpty(b.map) && b.name == array[1];
                    });
                }
                catch (InvalidOperationException) { }
                if (boss == null)
                {
                    boss = new ListBoss(array[1], array[2]);
                    listBosses.Add(boss);
                }
                else
                {
                    boss.map = array[2];
                    boss.GetMapId(boss.name, boss.map);
                }
                if (array.Length == 4)
                    boss.zoneId = int.Parse(array[3]);
                if (listBosses.Count > MAX_BOSS)
                    listBosses.RemoveAt(0);
                getScrollBar(out int scrollBarWidth, out _, out _);
                if (listBosses.Count > MAX_BOSS_DISPLAY && offsetX == 0)
                    offsetX = scrollBarWidth;
            }
        }
        static GUIStyle[] bossLabelStyles = new GUIStyle[MAX_BOSS_DISPLAY];

        private static Font cachedBossFont;
        private static int lastZoom = -1;

        static void InitStyles()
        {
            if (lastZoom != mGraphics.zoomLevel || cachedBossFont == null)
            {
                cachedBossFont = (Font)Resources.Load("FontSys/x" + mGraphics.zoomLevel + "/" + "barmeneb");
                lastZoom = mGraphics.zoomLevel;

                // Invalidate tất cả styles để cập nhật lại font
                for (int i = 0; i < MAX_BOSS_DISPLAY; i++)
                    bossLabelStyles[i] = null;
            }

            for (int i = 0; i < MAX_BOSS_DISPLAY; i++)
            {
                if (bossLabelStyles[i] == null)
                {
                    var style = new GUIStyle(GUI.skin.label)
                    {
                        alignment = TextAnchor.UpperRight,
                        fontSize = 8 * mGraphics.zoomLevel,
                        fontStyle = FontStyle.Bold,
                        richText = true,
                        font = cachedBossFont
                    };
                    bossLabelStyles[i] = style;
                }
            }
        }
        public static bool isShow()
        {
            
            return ModFunc.notifBoss && !ListBoss.isCollapsed;
        }
        private string cachedDisplayString;
        private float lastCacheTime = -1f;

        public string GetDisplayString(bool richText)
        {
            float now = Time.time;
            if (cachedDisplayString == null || now - lastCacheTime > 1f)
            {
                cachedDisplayString = BuildDisplayString(richText);
                lastCacheTime = now;
            }
            return cachedDisplayString;
        }
        private int cachedFrame = -1;

        public string GetDisplayStringCached(bool richText)
        {
            int frame = Time.frameCount;
            if (cachedFrame != frame)
            {
                cachedFrame = frame;
                sharedSB.Clear();

                TimeSpan t = DateTime.Now - AppearTime;

                if (richText)
                {
                    sharedSB.Append("<color=yellow>").Append(name).Append("</color>")
                            .Append(" - ").Append(map).Append(" - ");

                    if (t.Minutes > 0)
                        sharedSB.Append(t.Minutes).Append("m").Append(t.Seconds).Append("s");
                    else
                        sharedSB.Append(t.Seconds).Append("s");
                }
                else
                {
                    sharedSB.Append(name)
                            .Append(" - ").Append(map).Append(" (").Append(zoneId).Append(")")
                            .Append(" - ");

                    if (t.Minutes > 0)
                        sharedSB.Append(t.Minutes).Append("m").Append(t.Seconds).Append("s");
                    else
                        sharedSB.Append(t.Seconds).Append("s");
                }

                cachedDisplayString = sharedSB.ToString();
            }

            return cachedDisplayString;
        }
 

        public override string ToString()
        {
            var timeSpan = DateTime.Now - AppearTime;

            string location = string.IsNullOrEmpty(map)
                ? "chưa biết"
                : Utils.Fmt(map, " [", mapId, "]");

            string timeOrStatus;

            if (!isDied)
            {
                string zoneText =  "";

                string hour = (int)timeSpan.TotalHours > 0 ? Utils.Fmt((int)timeSpan.TotalHours, "h") : "";
                string minute = timeSpan.Minutes > 0 ? Utils.Fmt(timeSpan.Minutes, "m") : "";
                string second = Utils.Fmt(timeSpan.Seconds, "s");

                timeOrStatus = Utils.Fmt(zoneText, hour, minute, second);
            }
            else
            {
                timeOrStatus = string.IsNullOrEmpty(killer)
                    ? "Đã chết"
                    : Utils.Fmt("Bị ", killer, " tiêu diệt");
            }

            return Utils.Fmt(name, " - ", location, " - ", timeOrStatus);
        }




        public string BuildDisplayString(bool enableRichText)
{
    if (!enableRichText)
        return ToString(); // Gọi lại bản thường đã được tối ưu

    TimeSpan timeSpan = DateTime.Now - AppearTime;

    string colorName = "yellow";
    string colorMap = "yellow";

    if (TileMap.mapID == mapId)
    {
        colorName = Utils.FindCharInMap(name) != null ? "red" : "orange";
        colorMap = "red";
    }

    string namePart = Utils.Fmt("<color=", colorName, ">", name, "</color>");
    string mapPart = string.IsNullOrEmpty(map)
        ? "chưa biết"
        : Utils.Fmt("<color=", colorMap, ">", map, "</color> [<color=", colorMap, ">", mapId, "</color>]");

    string zoneText = "";
    if (!isDied && zoneId > -1)
    {
        if (TileMap.mapID == mapId)
        {
            if (TileMap.zoneID == zoneId)
                zoneText = Utils.Fmt("<color=yellow>khu</color> <color=red>", zoneId, "</color> - ");
            else
                zoneText = Utils.Fmt("<color=yellow>khu ", zoneId, "</color> - ");
        }
        else
        {
            zoneText = Utils.Fmt("khu <color=yellow>", zoneId, "</color> - ");
        }
    }

    string timeOrStatus;

    if (!isDied)
    {
        string hour = (int)timeSpan.TotalHours > 0 ? Utils.Fmt("<color=orange>", (int)timeSpan.TotalHours, "</color>h") : "";
        string minute = timeSpan.Minutes > 0 ? Utils.Fmt("<color=orange>", timeSpan.Minutes, "</color>m") : "";
        string second = Utils.Fmt("<color=orange>", timeSpan.Seconds, "</color>s");

        timeOrStatus = Utils.Fmt(zoneText, hour, minute, second);
    }
    else
    {
        timeOrStatus = string.IsNullOrEmpty(killer)
            ? "Đã chết"
            : Utils.Fmt("Bị <color=orange>", killer, "</color> tiêu diệt");
    }

    return Utils.Fmt(namePart, " - ", mapPart, " - ", timeOrStatus);
}

        private static readonly Dictionary<int, Font> fontCache = new();
        private static readonly Dictionary<int, GUIStyle> cachedBossStyle = new();

        public static int Paint(int _y, mGraphics g)
        {
            
            if (listBosses.Count <= 0) return getSpaceOccupied();

            maxLength = 0;
            y = _y;

            if (!isCollapsed)
            {
                PaintListBosses(g);
                PaintScroll(g);
            }
            PaintRect(g);
            

            return getSpaceOccupied();
        }
        private static int lastBossCount = -1;
        private static int cachedMaxLength = 0;

        static void PaintListBosses(mGraphics g)
        {
            int start = 0;
            if (listBosses.Count > MAX_BOSS_DISPLAY)
                start = listBosses.Count - MAX_BOSS_DISPLAY;
            GUIStyle[] styles = new GUIStyle[MAX_BOSS_DISPLAY];
            for (int i = start - offset; i < listBosses.Count - offset; i++)
            {
                styles[i - start + offset] = new GUIStyle(GUI.skin.label)
                {
                    alignment = TextAnchor.UpperRight,
                    fontSize = 7 * mGraphics.zoomLevel,
                    fontStyle = FontStyle.Normal,
                    richText = true,
                };
                styles[i - start + offset].font = (Font)Resources.Load("FontSys/x" + mGraphics.zoomLevel + "/" + "barmeneb");
                ListBoss boss = listBosses[i];
                int length = (Utils.getWidth(styles[i - start + offset], $"{boss}")) - 10;
                maxLength = Math.max(length, maxLength);
            }
            FillBackground(g);

            int xDraw = GameCanvas.w - (x + offsetX) - maxLength;
            for (int i = start - offset; i < listBosses.Count - offset; i++)
            {
                int yDraw = y + distanceBetweenLines * (i - start + offset);
                ListBoss boss = listBosses[i];

                if (GameCanvas.isMouseFocus(xDraw, yDraw, maxLength, 7))
                {
                    g.setColor(new Color(.2f, .2f, .2f, .7f));
                    g.fillRect(xDraw, yDraw + 1, maxLength, 7, 5);
                }

                /*if (GameCanvas.isMouseFocus(xDraw, yDraw, maxLength, 7))
                    CustomGraphics.fillRect(xDraw + 1, yDraw + 7, (maxLength - 2) * mGraphics.zoomLevel + 2, 1, Color.white);*/

                sharedSB.Clear();
                sharedSB.Append(i + 1).Append(". ").Append(boss.GetDisplayString(true));
                g.drawString(boss.GetDisplayStringCached(true), -(x + offsetX), mGraphics.zoomLevel - 3 + yDraw, styles[i - start + offset]);
            }
        }


        static void PaintScroll(mGraphics g)
        {
            if (listBosses.Count > MAX_BOSS_DISPLAY)
            {
                getButtonUp(out int buttonUpX, out int buttonUpY);
                getButtonDown(out int buttonDownX, out int buttonDownY);
                getScrollBar(out int scrollBarWidth, out int scrollBarHeight, out int scrollBarThumbHeight);
                g.setColor(new Color(.2f, .2f, .2f, .4f));
                g.fillRect(buttonUpX, buttonUpY, 9, scrollBarHeight + 6 * 2);
                g.drawRegion(Mob.imgHP, 0, (offset < listBosses.Count - MAX_BOSS_DISPLAY ? 24 : 54), 9, 6, 1, buttonUpX, buttonUpY, 0);
                g.drawRegion(Mob.imgHP, 0, (offset > 0 ? 24 : 54), 9, 6, 0, buttonDownX, buttonDownY, 0);
                //draw thumb
                g.setColor(new Color(.2f, .2f, .2f, .7f));
                g.fillRect(buttonUpX, buttonUpY + 6 + Mathf.CeilToInt((float)scrollBarHeight / listBosses.Count * (listBosses.Count - offset - MAX_BOSS_DISPLAY)), scrollBarWidth, scrollBarThumbHeight);
                g.setColor(new Color(.7f, .7f, 0f, 1f));
                g.drawRect(buttonUpX, buttonUpY + 6 + Mathf.CeilToInt((float)scrollBarHeight / listBosses.Count * (listBosses.Count - offset - MAX_BOSS_DISPLAY)), scrollBarWidth - 1, scrollBarThumbHeight - 1);
            }
        }

        static void PaintRect(mGraphics g)
        {
            getScrollBar(out int scrollBarWidth, out _, out _);
            if (listBosses.Count <= MAX_BOSS_DISPLAY)
                scrollBarWidth = 0;
            int w = maxLength + 5 + (scrollBarWidth > 0 ? (scrollBarWidth + 2) : 0);
            int h = distanceBetweenLines * System.Math.Min(MAX_BOSS_DISPLAY, listBosses.Count) + 7;
            GUIStyle style = new GUIStyle(GUI.skin.label)
            {
                fontSize = 9 * mGraphics.zoomLevel,
                fontStyle = FontStyle.Bold,
                alignment = TextAnchor.UpperRight,
                richText = true
            };
            style.normal.textColor = Color.white;
            style.font = (Font)Resources.Load("FontSys/x" + mGraphics.zoomLevel + "/" + "barmeneb");
            titleWidth = Utils.getWidth(style, LIST_BOSS) + 2;
            g.setColor(0, 0.5f);
            g.fillRect(GameCanvas.w - (x + offsetX) - titleWidth + scrollBarWidth, y - distanceBetweenLines, titleWidth + 2, 10, 5);
            /*if (GameCanvas.isMouseFocus(GameCanvas.w - (x + offsetX) - titleWidth + scrollBarWidth, y - distanceBetweenLines, titleWidth, 8))
            {
                g.setColor(style.normal.textColor);
                g.fillRect(GameCanvas.w - (x + offsetX) - titleWidth + scrollBarWidth, y - 1, titleWidth - 1, 1);
            }*/
            g.drawString(Utils.Fmt("<color=yellow>", LIST_BOSS, "</color>"), -(x + offsetX) + scrollBarWidth, y - distanceBetweenLines, style);
            getCollapseButton(out int collapseButtonX, out int collapseButtonY);
            g.drawRegion(Mob.imgHP, 0, 18, 9, 6, (isCollapsed ? 5 : 4), collapseButtonX, collapseButtonY, 0);
            if (isCollapsed || listBosses.Count <= 0)
                return;
            g.setColor(Color.yellow);
            g.fillRect(GameCanvas.w - (x + offsetX) - maxLength - 3, y - 5, w - titleWidth - 9 - (scrollBarWidth > 0 ? 2 : 0), 1);
            g.fillRect(GameCanvas.w - (x + offsetX) + scrollBarWidth, y - 5, 3 + (scrollBarWidth > 0 ? 1 : 0), 1);
            g.fillRect(GameCanvas.w - (x + offsetX) - maxLength - 3, y - 5, 1, h);
            g.fillRect(GameCanvas.w - (x + offsetX) - maxLength - 3 + w, y - 5, 1, h + 1);
            g.fillRect(GameCanvas.w - (x + offsetX) - maxLength - 3, y - 5 + h, w + 1, 1);
        }

        private static void FillBackground(mGraphics g)
        {
            if (!isCollapsed && listBosses.Count > 0)
            {
                g.setColor(0, 0.5f);
                getScrollBar(out int scrollBarWidth, out _, out _);
                if (listBosses.Count <= MAX_BOSS_DISPLAY)
                    scrollBarWidth = 0;
                int w = maxLength + 5 + (scrollBarWidth > 0 ? (scrollBarWidth + 2) : 0);
                int h = distanceBetweenLines * System.Math.Min(MAX_BOSS_DISPLAY, listBosses.Count) + 7;
                g.fillRect(GameCanvas.w - (x + offsetX) - maxLength - 3, y - 5, w, h);
            }
        }

        static int GetMapID(string mapName)
        {
            for (int i = 0; i < TileMap.mapNames.Length; i++)
            {
                if (TileMap.mapNames[i].Equals(mapName))
                    return i;
            }
            return -1;
        }

        public static void UpdateTouch()
        {
            if (lastBoss != -1 && mSystem.currentTimeMillis() - Utils.GetLastTimePress() > 200)
                lastBoss = -1;
            if (!isEnabled)
                return;
            if (!GameCanvas.isTouch || ChatTextField.gI().isShow || GameCanvas.menu.showMenu)
                return;
            getCollapseButton(out int collapseButtonX, out int collapseButtonY);
            getScrollBar(out int scrollBarWidth, out int scrollBarHeight, out _);
            if (GameCanvas.isPointerHoldIn(collapseButtonX, collapseButtonY, 6, 9) || GameCanvas.isMouseFocus(GameCanvas.w - (x + offsetX) - titleWidth + scrollBarWidth, y - distanceBetweenLines, titleWidth, 8))
            {
                GameCanvas.isPointerJustDown = false;
                GameScr.gI().isPointerDowning = false;
                if (GameCanvas.isPointerClick)
                    isCollapsed = !isCollapsed;
                GameCanvas.clearAllPointerEvent();
                return;
            }
            if (isCollapsed)
                return;
            int start = 0;
            if (listBosses.Count > MAX_BOSS_DISPLAY)
                start = listBosses.Count - MAX_BOSS_DISPLAY;
            for (int i = start - offset; i < listBosses.Count - offset; i++)
            {
                if (GameCanvas.isPointerHoldIn(GameCanvas.w - (x + offsetX) - maxLength, y + 1 + distanceBetweenLines * (i - start + offset), maxLength, 7))
                {
                    GameCanvas.isPointerJustDown = false;
                    GameScr.gI().isPointerDowning = false;
                    if (GameCanvas.isPointerClick)
                    {
                        if (listBosses[i].isDied)
                            GameScr.info1.addInfo($"Boss đã {(string.IsNullOrEmpty(listBosses[i].killer) ? "chết" : $"bị {listBosses[i].killer} tiêu diệt")}!", 0);
                        else
                        {
                            if (lastBoss == i && mSystem.currentTimeMillis() - Utils.GetLastTimePress() <= 200)
                            {
                                if (TileMap.mapID != listBosses[i].mapId)
                                {
                                    string notif;
                                        XmapController.StartRunToMapId(listBosses[i].mapId);
                                    GameScr.info1.addInfo("Di chuyển đến boss ở MAP " + listBosses[i].mapId, 1);
                                    return;
                                }
                            }
                            else
                                lastBoss = i;
                            if (TileMap.mapID == listBosses[i].mapId)
                            {
                                int j = 0;
                                for (; j < GameScr.vCharInMap.size(); j++)
                                {
                                    Char ch = GameScr.vCharInMap.elementAt(j) as Char;
                                    if (ch.cName == listBosses[i].name)
                                    {
                                        Char.myCharz().deFocusNPC();
                                        Char.myCharz().itemFocus = null;
                                        Char.myCharz().mobFocus = null;
                                        if (Char.myCharz().charFocus != ch)
                                            Char.myCharz().charFocus = ch;
                                        else
                                            Utils.TeleportMyChar(ch);
                                        break;
                                    }
                                }
                                if (j == GameScr.vCharInMap.size())
                                {
                                    if (listBosses[i].zoneId != -1 && TileMap.zoneID != listBosses[i].zoneId)
                                    {
                                        GameScr.info1.addInfo($"Vào khu {listBosses[i].zoneId}!", 0);
                                        Service.gI().requestChangeZone(listBosses[i].zoneId, 0);
                                        return;
                                    }
                                    GameScr.info1.addInfo("Boss không có trong khu!", 0);
                                }
                            }
                        }
                    }
                    GameCanvas.clearAllPointerEvent();
                    return;
                }
            }
            if (listBosses.Count > MAX_BOSS_DISPLAY)
            {
                getButtonUp(out int buttonUpX, out int buttonUpY);
                if (GameCanvas.isPointerMove && GameCanvas.isPointerDown && GameCanvas.isPointerHoldIn(buttonUpX, buttonUpY, scrollBarWidth, scrollBarHeight))
                {
                    float increment = scrollBarHeight / (float)listBosses.Count;
                    float newOffset = (GameCanvas.pyMouse - buttonUpY) / increment;
                    if (float.IsNaN(newOffset))
                        return;
                    offset = Mathf.Clamp(listBosses.Count - Mathf.RoundToInt(newOffset), 0, listBosses.Count - MAX_BOSS_DISPLAY);
                    return;
                }
                if (GameCanvas.isPointerHoldIn(buttonUpX, buttonUpY, 9, 6))
                {
                    GameCanvas.isPointerJustDown = false;
                    GameScr.gI().isPointerDowning = false;
                    if (GameCanvas.isPointerClick)
                    {
                        if (offset + MAX_BOSS_DISPLAY <= listBosses.Count - MAX_BOSS_DISPLAY)
                            offset += MAX_BOSS_DISPLAY;
                        else if (offset < listBosses.Count - MAX_BOSS_DISPLAY)
                            offset++;
                    }
                    GameCanvas.clearAllPointerEvent();
                    return;
                }
                getButtonDown(out int buttonDownX, out int buttonDownY);
                if (GameCanvas.isPointerHoldIn(buttonDownX, buttonDownY, 9, 6))
                {
                    GameCanvas.isPointerJustDown = false;
                    GameScr.gI().isPointerDowning = false;
                    if (GameCanvas.isPointerClick)
                    {
                        if (offset - MAX_BOSS_DISPLAY >= 0)
                            offset -= MAX_BOSS_DISPLAY;
                        else if (offset > 0)
                            offset--;
                    }
                    GameCanvas.clearAllPointerEvent();
                    return;
                }
            }
        }

        public static void Update()
        {
            for (int i = listBosses.Count - 1; i >= 0; i--)
            {
                ListBoss boss = listBosses[i];
                if (boss.mapId == TileMap.mapID && !Char.isLoadingMap)
                {
                    int j = 0;
                    for (; j < GameScr.vCharInMap.size(); j++)
                    {
                        Char ch = GameScr.vCharInMap.elementAt(j) as Char;
                        if (ch.cName == boss.name)
                        {
                            if (boss.zoneId == -1)
                                boss.zoneId = TileMap.zoneID;
                            if (ch.isDie || ch.cHP == 0)
                                boss.isDied = true;
                            break;
                        }   
                    }
                    if (boss.zoneId == TileMap.zoneID && j == GameScr.vCharInMap.size())
                        boss.isDied = true;
                }
            }
            if (isEnabled && !isCollapsed && GameCanvas.isMouseFocus(GameCanvas.w - (x + offsetX) - maxLength, y + 1, maxLength, 8 * MAX_BOSS_DISPLAY))
            {
                if (GameCanvas.pXYScrollMouse > 0)
                    if (offset < listBosses.Count - MAX_BOSS_DISPLAY)
                        offset++;
                if (GameCanvas.pXYScrollMouse < 0)
                    if (offset > 0)
                        offset--;
            }
        }

        static void getButtonUp(out int buttonUpX, out int buttonUpY)
        {
            buttonUpX = GameCanvas.w - (x + offsetX) + 2;
            buttonUpY = y + 1;
        }

        static void getButtonDown(out int buttonDownX, out int buttonDownY)
        {
            buttonDownX = GameCanvas.w - (x + offsetX) + 2;
            buttonDownY = y + 2 + distanceBetweenLines * (MAX_BOSS_DISPLAY - 1);
        }

        static void getScrollBar(out int scrollBarWidth, out int scrollBarHeight, out int scrollBarThumbHeight)
        {
            scrollBarWidth = 9;
            scrollBarHeight = MAX_BOSS_DISPLAY * distanceBetweenLines - 1 - 6 * 2;
            scrollBarThumbHeight = Mathf.CeilToInt((float)MAX_BOSS_DISPLAY / listBosses.Count * scrollBarHeight);
        }

        static void getCollapseButton(out int collapseButtonX, out int collapseButtonY)
        {
            getScrollBar(out int scrollBarWidth, out _, out _);
            if (listBosses.Count <= MAX_BOSS_DISPLAY)
                scrollBarWidth = 0;
            collapseButtonX = GameCanvas.w - (x + offsetX) - titleWidth + scrollBarWidth - 8;
            collapseButtonY = y - distanceBetweenLines + 1;
        }

        public static void setState(bool value) => isEnabled = value;

        internal static int getSpaceOccupied()
        {
            if (!isEnabled || listBosses.Count <= 0) return 0;

            byte border = 5;
            byte titleH = 7;

            return isCollapsed
                ? distanceBetweenLines
                : titleH + border + distanceBetweenLines * Mathf.Clamp(listBosses.Count, 0, MAX_BOSS_DISPLAY);
        }

        // [ChatCommand("testboss")]
        // public static void Test()
        // {
        //     for (int i = 0; i < 10; i++)
        //     {
        //         GameEvents.OnChatVip("BOSS Vũ Đăng vừa xuất hiện tại Đảo Kamê khu vực 10");
        //     }
        // }
    }
}
