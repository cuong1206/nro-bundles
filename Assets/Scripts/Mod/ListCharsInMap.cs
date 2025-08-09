using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Mod
{
    internal class ListCharsInMap
    {
        internal static List<Char> listChars = new();
        private static readonly HashSet<int> charIDs = new();
        internal static bool isEnabled = true;
        internal static bool isShowPet;

        internal static int x = 6;
        internal static int y = 0;

        static int maxLength = 0;
        static readonly int MAX_CHAR = 6;
        static int distanceBetweenLines = 10;
        static int offset = 0;
        static bool isCollapsed;
        static int titleWidth;
        static int offsetX;

        private static readonly StringBuilder sb = new();
        private static readonly StringBuilder sharedSB = new();

        private static readonly Dictionary<double, string> cachedHP = new();
        private static readonly Dictionary<double, string> cachedHPFull = new();
        private static readonly Dictionary<int, Font> preloadedFonts = new();
        private static readonly Dictionary<int, GUIStyle> cachedCharStyle = new();
        private static readonly Dictionary<int, string> charDescCache = new();
        private static readonly Dictionary<int, int> charWidthCache = new();
        private static readonly List<(string, GUIStyle)> charDescriptions = new();

        private static Char[] charBuffer = new Char[128];

        static ListCharsInMap()
        {
            for (int z = 1; z <= 5; z++)
            {
                if (!preloadedFonts.ContainsKey(z))
                {
                    var font = (Font)Resources.Load("FontSys/x" + z + "/barmeneb");
                    preloadedFonts[z] = font;
                }
            }
        }

        internal static void Update()
        {
            if (!isEnabled) return;

            listChars.Clear();
            charIDs.Clear();

            int totalChars = GameScr.vCharInMap.size();
            if (charBuffer.Length < totalChars)
                charBuffer = new Char[totalChars];

            for (int i = 0; i < totalChars; i++)
                charBuffer[i] = (Char)GameScr.vCharInMap.elementAt(i);

            for (int i = 0; i < totalChars; i++)
            {
                Char ch = charBuffer[i];
                if (ch.IsNormalChar(true))
                {
                    listChars.Add(ch);
                    charIDs.Add(ch.charID);

                    if (isShowPet && ch.charID > 0)
                    {
                        Char chPet = GameScr.findCharInMap(-ch.charID);
                        if (chPet != null)
                        {
                            listChars.Add(chPet);
                            charIDs.Add(chPet.charID);
                        }
                    }
                }
            }

            if (isShowPet)
            {
                for (int i = 0; i < totalChars; i++)
                {
                    Char ch = charBuffer[i];
                    if (ch.IsNormalChar(false, true) && !charIDs.Contains(ch.charID))
                    {
                        listChars.Add(ch);
                        charIDs.Add(ch.charID);
                    }
                }
            }

            int extra = listChars.Count - MAX_CHAR;
            if (offset >= extra)
                offset = extra > 0 ? extra : 0;

            if (GameCanvas.isMouseFocus(GameCanvas.w - (x + offsetX) - maxLength, y + 1, maxLength, 8 * MAX_CHAR))
            {
                if (GameCanvas.pXYScrollMouse > 0 && offset < listChars.Count - MAX_CHAR)
                    offset++;
                if (GameCanvas.pXYScrollMouse < 0 && offset > 0)
                    offset--;
            }

            getScrollBar(out int scrollBarWidth, out _, out _);
            offsetX = listChars.Count > MAX_CHAR ? scrollBarWidth : 0;
        }
        internal static int Paint(int _y, mGraphics g)
        {
            if (!isEnabled) return getSpaceOccupied();

            if (offset >= listChars.Count - MAX_CHAR)
                offset = listChars.Count - MAX_CHAR > 0 ? listChars.Count - MAX_CHAR : 0;

            y = _y;
            maxLength = 0;

            if (!isCollapsed)
            {
                PaintListChars(g);
                PaintScroll(g);
            }

            PaintRect(g);
            return getSpaceOccupied();
        }
        static string formatHP(Char ch)
        {
            double hp = ch.cHP;
            double hpFull = ch.cHPFull;

            if (!cachedHP.TryGetValue(hp, out var hpStr))
            {
                hpStr = NinjaUtil.getMoneys(hp);
                cachedHP[hp] = hpStr;
            }

            if (!cachedHPFull.TryGetValue(hpFull, out var hpFullStr))
            {
                hpFullStr = NinjaUtil.getMoneys(hpFull);
                cachedHPFull[hpFull] = hpFullStr;
            }

            sb.Clear();
            sb.Append('[').Append(hpStr).Append('/').Append(hpFullStr).Append(']');
            return sb.ToString();
        }
        private static readonly Dictionary<int, Font> fontCache = new();
        private static GUIStyle style;
        static void PaintListChars(mGraphics g)
        {
            int skippedCharCount = 0;
            charDescriptions.Clear();
            int start = listChars.Count > MAX_CHAR ? listChars.Count - MAX_CHAR : 0;
            if (!cachedCharStyle.TryGetValue(mGraphics.zoomLevel, out style))
            {
                if (!fontCache.TryGetValue(mGraphics.zoomLevel, out var font))
                {
                    font = (Font)Resources.Load("FontSys/x" + mGraphics.zoomLevel + "/barmeneb");
                    fontCache[mGraphics.zoomLevel] = font;
                }

                style = new GUIStyle(GUI.skin.label)
                {
                    fontSize =  8 * mGraphics.zoomLevel,
                    fontStyle = FontStyle.Normal,
                    alignment = TextAnchor.UpperRight,
                    richText = false,
                    font = font
                };
                cachedCharStyle[mGraphics.zoomLevel] = style;
            }

            for (int i = start - offset; i < listChars.Count - offset; i++)
            {
                Char ch = listChars[i];

                sharedSB.Clear();
                sharedSB.Append(ch.GetClanTag());
                sharedSB.Append(ch.GetNameWithoutClanTag(false));
                sharedSB.Append(" ");
                sharedSB.Append(formatHP(ch));

                if (ch.IsNormalChar())
                {
                    sharedSB.Append(" - ");
                    sharedSB.Append(ch.GetGender(false));
                }

                if (ch.IsPet())
                {
                    sharedSB.Append(" - ");
                    sharedSB.Append(ch.GetGender(false));

                    Char chMaster = GameScr.findCharInMap(-ch.charID);
                    if (chMaster != null)
                    {
                        sharedSB.Append(" - ");
                        sharedSB.Append(chMaster.GetNameWithoutClanTag(false));
                        sharedSB.Append(" (pet)");
                    }
                    else
                    {
                        sharedSB.Append(" - ");
                        sharedSB.Append("Bị lạc");
                    }

                    skippedCharCount++;
                }
                else
                {
                    skippedCharCount++;
                }

                string desc = sharedSB.ToString();
                charDescriptions.Add((desc, style));

                int width = Utils.getWidth(style, desc);
                if (ch.cFlag != 0) width += distanceBetweenLines + 1;
                maxLength = System.Math.Max(maxLength, width);
            }

            FillBackground(g);

            for (int i = start - offset; i < listChars.Count - offset; i++)
            {
                int offsetPaint = 0;
                Char ch = listChars[i];
                if (GameCanvas.isMouseFocus(GameCanvas.w - (x + offsetX) - maxLength - offsetPaint, y + 1 + distanceBetweenLines * (i - start + offset), maxLength, distanceBetweenLines - 1))
                {
                    g.setColor(new Color(.2f, .2f, .2f, .7f));
                    g.fillRect(GameCanvas.w - (x + offsetX) - maxLength, y - 2 + distanceBetweenLines * (i - start + offset), maxLength - offsetPaint, distanceBetweenLines - 1, 5);
                }
                g.setColor(ch.GetFlagColor());
                if (ch.cFlag != 0)
                {
                    offsetPaint = distanceBetweenLines + 1;
                    if (ch.cFlag is 9 or 10)
                    {
                        GUIStyle flagStyle = new GUIStyle(GUI.skin.label)
                        {
                            alignment = TextAnchor.UpperCenter,
                            fontSize = 7 * mGraphics.zoomLevel,
                            normal = { textColor = Color.white }
                        };

                        string flagText = ch.cFlag == 9 ? "K" : "M";
                        g.drawString(flagText, -(x + offsetX), mGraphics.zoomLevel - 3 + y + distanceBetweenLines * (i - start + offset), flagStyle);
                    }
                    
                    g.fillRect(GameCanvas.w - (x + offsetX) - distanceBetweenLines + 1, y  + distanceBetweenLines * (i - start + offset), distanceBetweenLines - 5, distanceBetweenLines - 5, 5);
                }

                string colorr = "#ffffff";
                if (GameCanvas.isMouseFocus(GameCanvas.w - (x + offsetX) - maxLength - offsetPaint, y + 1 + distanceBetweenLines * (i - start + offset), maxLength, distanceBetweenLines - 1))
                    colorr = "#fffde6";
                if (Char.myCharz().charFocus == ch)
                    colorr = "#fff366";
                if (ch.IsBoss())
                    colorr = "#e7847c";
                else if (ch.IsPet())
                    colorr = "#66a6c4";
                if (ch.cHP <= 0)
                    colorr = "#00405e";

                int offset2 = ch.IsBoss() ? -1 : 0;
                Utils.DrawStringWithOutline(g,charDescriptions[i - start + offset].Item1, -(x + offsetX) - offsetPaint, mGraphics.zoomLevel - 3 + y + distanceBetweenLines * (i - start + offset) + offset2, charDescriptions[i - start + offset].Item2, colorr, "#525455", 0);
            }
        }
        private static void FillBackground(mGraphics g)
        {
            if (!isCollapsed && listChars.Count > 0)
            {
                g.setColor(0,0.4f);
                getScrollBar(out int scrollBarWidth, out _, out _);
                if (listChars.Count <= MAX_CHAR)
                    scrollBarWidth = 0;
                int w = maxLength + 5 + (scrollBarWidth > 0 ? (scrollBarWidth + 2) : 0);
                int h = distanceBetweenLines * System.Math.Min(MAX_CHAR, listChars.Count) + 7;
                g.fillRect(GameCanvas.w - (x + offsetX) - maxLength - 3, y - 5, w, h);
            }
        }

        static void PaintScroll(mGraphics g)
        {
            if (listChars.Count > MAX_CHAR)
            {
                getButtonUp(out int buttonUpX, out int buttonUpY);
                getButtonDown(out int buttonDownX, out int buttonDownY);
                getScrollBar(out int scrollBarWidth, out int scrollBarHeight, out int scrollBarThumbHeight);
                g.setColor(new Color(.2f, .2f, .2f, .4f));
                g.fillRect(buttonUpX, buttonUpY, 9, scrollBarHeight + 6 * 2);
                g.drawRegion(Mob.imgHP, 0, (offset < listChars.Count - MAX_CHAR ? 18 : 54), 9, 6, 1, buttonUpX, buttonUpY, 0);
                g.drawRegion(Mob.imgHP, 0, (offset > 0 ? 18 : 54), 9, 6, 0, buttonDownX, buttonDownY, 0);
                //draw thumb
                g.setColor(new Color(.2f, .2f, .2f, .7f));
                g.fillRect(buttonUpX, buttonUpY + 6 + Mathf.CeilToInt((float)scrollBarHeight / listChars.Count * (listChars.Count - offset - MAX_CHAR)), scrollBarWidth, scrollBarThumbHeight);
                g.setColor(new Color(.7f, .7f, 0f, 1f));
                g.drawRect(buttonUpX, buttonUpY + 6 + Mathf.CeilToInt((float)scrollBarHeight / listChars.Count * (listChars.Count - offset - MAX_CHAR)), scrollBarWidth - 1, scrollBarThumbHeight - 1);
            }
        }

        static void PaintRect(mGraphics g)
        {
            try {
            getScrollBar(out int scrollBarWidth, out _, out _);
            if (listChars.Count <= MAX_CHAR)
                scrollBarWidth = 0;
            int w = maxLength + 5 + (scrollBarWidth > 0 ? (scrollBarWidth + 2) : 0);
            int h = distanceBetweenLines * System.Math.Min(MAX_CHAR, listChars.Count) + 7;
                int count = 0;
                foreach (var c in listChars)
                    if (c.IsNormalChar())
                        count++;


                string str = $"<color=yellow>{TileMap.mapName}</color> {"K"}<color=yellow>{TileMap.zoneID}</color> ";
            GUIStyle style = new GUIStyle(GUI.skin.label)
            {
                fontSize = 9 * mGraphics.zoomLevel,
                fontStyle = FontStyle.Normal,
                alignment = TextAnchor.UpperRight,
                richText = true
            };
           // style.normal.textColor = Color.white;
            
            style.font = (Font)Resources.Load("FontSys/x" + mGraphics.zoomLevel + "/" + "barmeneb");
            titleWidth = Utils.getWidth(style, str) + 2;
                g.setColor(0, 0.5f);
                g.fillRect(GameCanvas.w - (x + offsetX) - titleWidth + scrollBarWidth, y - distanceBetweenLines - 1, titleWidth + 2, 10, 5);
            /*if (GameCanvas.isMouseFocus(GameCanvas.w - (x + offsetX) - titleWidth + scrollBarWidth, y - distanceBetweenLines, titleWidth, 8))
            {
                g.setColor(style.normal.textColor);
                g.fillRect(GameCanvas.w - (x + offsetX) - titleWidth + scrollBarWidth, y - 1, titleWidth - 1, 1);
            }*/
            g.drawString(str, -(x + offsetX) + scrollBarWidth, y - distanceBetweenLines - 1, style);
            getCollapseButton(out int collapseButtonX, out int collapseButtonY);
            g.drawRegion(Mob.imgHP, 0, 18, 9, 6, (isCollapsed ? 5 : 4), collapseButtonX, collapseButtonY, 0);
            if (isCollapsed || listChars.Count <= 0)
                return;
            g.setColor(Color.yellow);
            g.fillRect(GameCanvas.w - (x + offsetX) - maxLength - 3, y - 5, w - titleWidth - 9 - (scrollBarWidth > 0 ? 2 : 0), 1, 5);
            g.fillRect(GameCanvas.w - (x + offsetX) + scrollBarWidth, y - 5, 3 + (scrollBarWidth > 0 ? 1 : 0), 1,5);
            g.fillRect(GameCanvas.w - (x + offsetX) - maxLength - 3, y - 5, 1, h,5);
            g.fillRect(GameCanvas.w - (x + offsetX) - maxLength - 3 + w, y - 5, 1, h + 1, 5);
            g.fillRect(GameCanvas.w - (x + offsetX) - maxLength - 3, y - 5 + h, w + 1, 1, 5);
            }
            catch (Exception ex)
            {
                Cout.LogError("Loi paint list char " + ex.ToString());
            }
}

        internal static void updateTouch()
        {
          //  Res.err("UPDATE PAINRCHARMAP");
            if (!isEnabled)
                return;
            try
            {
                if (!GameCanvas.isTouch || ChatTextField.gI().isShow || GameCanvas.menu.showMenu)
                    return;
                getScrollBar(out int scrollBarWidth, out int scrollBarHeight, out int scrollBarThumbHeight);
                getCollapseButton(out int collapseButtonX, out int collapseButtonY);
                if (GameCanvas.isPointerHoldIn(collapseButtonX, collapseButtonY, 9, 6) || GameCanvas.isPointerHoldIn(GameCanvas.w - (x + offsetX) - titleWidth + scrollBarWidth, y - distanceBetweenLines, titleWidth, 8))
                {
                    GameCanvas.isPointerJustDown = false;
                    GameScr.gI().isPointerDowning = false;
                    if (GameCanvas.isPointerClick)
                        isCollapsed = !isCollapsed;
                    GameCanvas.clearAllPointerEvent();
                    return;
                }
                int start = 0;
                if (listChars.Count > MAX_CHAR)
                    start = listChars.Count - MAX_CHAR;
                for (int i = start - offset; i < listChars.Count - offset; i++)
                {
                    if (GameCanvas.isPointerHoldIn(GameCanvas.w - (x + offsetX) - maxLength - (listChars[i].cFlag != 0 ? (distanceBetweenLines + 1) : 0), y + 1 + distanceBetweenLines * (i - start + offset), maxLength, distanceBetweenLines - 1))
                    {
                        GameCanvas.isPointerJustDown = false;
                        GameScr.gI().isPointerDowning = false;
                        if (GameCanvas.isPointerClick)
                        {
                            Char.myCharz().mobFocus = null;
                            Char.myCharz().npcFocus = null;
                            Char.myCharz().itemFocus = null;
                                Utils.TeleportMyChar(listChars[i]);
                            if (Char.myCharz().charFocus != listChars[i])
                                Char.myCharz().charFocus = listChars[i];
                        }
                        Char.myCharz().currentMovePoint = null;
                        GameCanvas.clearAllPointerEvent();
                        return;
                    }
                }
                if (listChars.Count > MAX_CHAR)
                {
                    getButtonUp(out int buttonUpX, out int buttonUpY);
                    if (GameCanvas.isPointerMove && GameCanvas.isPointerDown && GameCanvas.isPointerHoldIn(buttonUpX, buttonUpY, scrollBarWidth, scrollBarHeight))
                    {
                        float increment = scrollBarHeight / (float)listChars.Count;
                        float newOffset = (GameCanvas.pyMouse - buttonUpY) / increment;
                        if (float.IsNaN(newOffset))
                            return;
                        offset = Mathf.Clamp(listChars.Count - Mathf.RoundToInt(newOffset), 0, listChars.Count - MAX_CHAR);
                        return;
                    }
                    if (GameCanvas.isPointerHoldIn(buttonUpX, buttonUpY, 9, 6))
                    {
                        GameCanvas.isPointerJustDown = false;
                        GameScr.gI().isPointerDowning = false;
                        if (GameCanvas.isPointerClick)
                        {
                            if (offset < listChars.Count - MAX_CHAR)
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
                            if (offset > 0)
                                offset--;
                        }
                        GameCanvas.clearAllPointerEvent();
                        return;
                    }
                }
            }
            catch (Exception) { }
        }

        static void getButtonUp(out int buttonUpX, out int buttonUpY)
        {
            buttonUpX = GameCanvas.w - (x + offsetX) + 2;
            buttonUpY = y + 1;
        }
        
        static void getButtonDown(out int buttonDownX, out int buttonDownY)
        {
            buttonDownX = GameCanvas.w - (x + offsetX) + 2;
            buttonDownY = y + 2 + distanceBetweenLines * (MAX_CHAR - 1);
        }

        static void getScrollBar(out int scrollBarWidth, out int scrollBarHeight, out int scrollBarThumbHeight)
        {
            scrollBarWidth = 9;
            scrollBarHeight = MAX_CHAR * distanceBetweenLines - 1 - 6 * 2;
            scrollBarThumbHeight = Mathf.CeilToInt((float)MAX_CHAR / listChars.Count * scrollBarHeight);
        }

        static void getCollapseButton(out int collapseButtonX, out int collapseButtonY)
        {
            getScrollBar(out int scrollBarWidth, out _, out _);
            if (listChars.Count <= MAX_CHAR)
            scrollBarWidth = 0;
            collapseButtonX = GameCanvas.w - (x + offsetX) - titleWidth + scrollBarWidth - 8;
            collapseButtonY = y - distanceBetweenLines + 1;
        }

        internal static void setState(bool value) => isEnabled = value;

        internal static void setStatePet(bool value) => isShowPet = value;

        internal static int getSpaceOccupied()
        {
            if (!isEnabled) 
                return 0;
            byte border = 5;
            byte titleH = 7;
            if (isCollapsed)
                return distanceBetweenLines;
            else
                return titleH + border + distanceBetweenLines * Mathf.Clamp(listChars.Count, 0, MAX_CHAR);
        }
    }
}
