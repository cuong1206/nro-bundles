using Assets.src.e;
using System;
using System.Drawing;
using UnityEngine;
using UnityEngine.TextCore.Text;
using UnityEngine.UIElements;
using C_U_O_N_G;
using Assets.src.g;
using System.Data;
using System.Xml.Linq;
using System.Collections.Generic;
using System.Numerics;
using System.Linq;
using System.Reflection;
using TextAsset = UnityEngine.TextAsset;
namespace C_U_O_N_G
{
    public class Bag : IActionListener, IChatable
    {
        /* AUTHOR: C-U-O-N-G */
        private enum TypePaint
        {
            typeBody,
            typeBag,
            typeBox
        }
        Image imgSlThoiVang;
        private int currspecialskill = -1;
        public ChatPopup cp;
        private static Bag instance;
        public TabClanIcon tabIcon;
        public bool isShow = false;
        private TypePaint typePaint;
        private Image bgSize;
        public Image bgchar;
        private Image btnItem;
        private Image left;
        private Image right;
        private Image imgBoxPowwer;
        private Image imgZoneUI;
        private Image imgNameClan;
        private Image chaticon;
        private Image stickericon;
        private Image xindau;
        private Image bgClan;
        private Image bgClanMain;
        private Image bgplayer;
        private Image bginven;
        private Image bginfo;
        private Image khunginfo;
        public ChatTextField chatTField;
        private Image[] imgBox = new Image[5];
        public static Image[] sticker = new Image[39];
        private Image[] indexBag = new Image[2];
        private Image[] tilemap = new Image[3];
        private Image[] indexTabSkill = new Image[2];
        private Image[] selectmenu = new Image[2];
        private Image[] buttonmem = new Image[4];
        private int currIndex = 0, currTab = 0, currTabBody = 0, currTabSkill = 0;
        private int maxTabSkill = -1;
        private int HANH_TRANG = 0;
        private int THONG_TIN = 1;
        private int BANG_HOI = 2;
        private int DE_TU = 3;
        private int CHUC_NANG = 4;
        public Scroll scrollBag, scrollInfo, scrollNoitai, scrollMemClan, scrollClans, scrollChatClan, scrollSticker, scrollInfoSkill;
        private int numCol = 5, numRow = 5;
        private int X, Y, W, H;
        private Cell[] cellsBag;
        private Item itemDetails;
        private bool isBody = true, isItemInfo;
        public bool isSpeacialSkill = false;
        public bool isShowchatClan = false;
        public bool isShowMemClan = false;
        public bool isShowInfomem = false;
        public bool isPaintStickers = false;
        public int sizeSpecialSkill = 0;
        public bool isshowmenuright = true;
        private int selected;
        private Tab[] tabs;
        private TabSkill[] tabSkills;
        public new int[] PointPlus = new int[3] { 1, 10, 100 };
        public new int typePoint = 0;
        public new int typeNumPoint = 0;
        private Image imgX;
        private Image beach;
        private Image iceright;
        private Image iceleft;
        private Image icetop;
        private Image[] imgTab = new Image[2];
        private Image[] imgIce = new Image[6];
        private int numCol2, numRow2;
        public int[] LeaderpartID;
        private Image iconMainClan;
        private Image iconChatClan;
        private Image iconMemClan;
        private SkillTemplate currentSkill;
        public string namesearch;
        Member currmember = null;
        Clan currclan = null;

        public static Bag gI()
        {
            if (instance == null)
            {
                instance = new Bag();
            }
            return instance;
        }
        private class Cell
        {
            public int x;
            public int y;
            public Cell(int x, int y)
            {
                this.x = x;
                this.y = y;
            }
            public void paint(mGraphics g, byte index)
            {

                /* GameScr.info1.addInfo("|2|size body " + Char.myCharz().itemsbagsize, 0);*/
                int size = (Bag.gI().currIndex == 0) ? (Char.myCharz().itemsbagsize - 1) : (Char.myCharz().itemsboxsize - 1);
                if (Char.myCharz().itemsbagsize > 0 && index > size)
                {
                    return;
                }
                /*g.setColor(Bag.gI().selected == (sbyte)index ? 16383818 : 0xed9056, 1f);
                g.fillRect(x, y, Bag.gI().btnItem.getWidth(), Bag.gI().btnItem.getHeight(), 4);*/
                g.drawImage(Bag.gI().btnItem, x, y);
            }
        }

        public Bag()
        {
            if (!ModFunc.GI().isnewTab) return;
            if (bgSize == null)
            {
                bgSize = GameCanvas.loadImage("/bag/bgSize.png");
            }

            if (imgSlThoiVang == null)
            {
                imgSlThoiVang = GameCanvas.loadImage("/mainImage/thoiVang.png");
            }
            if (imgBoxPowwer == null)
            {
                imgBoxPowwer = GameCanvas.loadImage("/bag/textbox1.png");
            }
            if (beach == null)
            {
                beach = GameCanvas.loadImage("/bag/beach.png");
            }
            if (left == null)
            {
                left = GameCanvas.loadImage("/bag/left.png");
            }
            if (right == null)
            {
                right = GameCanvas.loadImage("/bag/right.png");
            }

            if (imgNameClan == null)
            {
                imgNameClan = GameCanvas.loadImage("/bag/textboxclan.png");
            }
            if (chaticon == null)
            {
                chaticon = GameCanvas.loadImage("/bag/chaticon.png");
            }
            if (stickericon == null)
            {
                stickericon = GameCanvas.loadImage("/bag/stickericon.png");
            }
            if (xindau == null)
            {
                xindau = GameCanvas.loadImage("/bag/xindau.png");
            }
            if (bgClan == null)
            {
                bgClan = GameCanvas.loadImage("/bag/bgClan.png");
            }
            if (bgClanMain == null)
            {
                bgClanMain = GameCanvas.loadImage("/bag/bgClanMain.png");
            }
            if (bgplayer == null)
            {
                bgplayer = GameCanvas.loadImage("/bag/bgplayer.png");
            }
            if (bginven == null)
            {
                bginven = GameCanvas.loadImage("/bag/bginven.png");
            }
            if (bginfo == null)
            {
                bginfo = GameCanvas.loadImage("/bag/bginfo.png");
            }
            if (khunginfo == null)
            {
                khunginfo = GameCanvas.loadImage("/bag/khunginfo.png");
            }
            if (iconMainClan == null)
            {
                iconMainClan = GameCanvas.loadImage("/bag/iconMainClan.png");
            }
            if (iconChatClan == null)
            {
                iconChatClan = GameCanvas.loadImage("/bag/iconChatClan.png");
            }
            if (iconMemClan == null)
            {
                iconMemClan = GameCanvas.loadImage("/bag/iconMemClan.png");
            }

            X = GameCanvas.w / 2 - bgSize.getWidth() / 2;
            Y = GameCanvas.h / 2 - bgSize.getHeight() / 2;
            W = bgSize.getWidth();
            H = bgSize.getHeight();
            for (int i = 0; i < imgBox.Length; i++)
            {
                if (imgBox[i] == null)
                {
                    imgBox[i] = GameCanvas.loadImage("/bag/textbox.png");
                }
            }
            btnItem = GameCanvas.loadImage("/bag/btnItem.png");
            for (int i = 0; i < indexBag.Length; i++)
            {
                if (indexBag[i] == null)
                {
                    indexBag[i] = GameCanvas.loadImage($"/bag/index{i}.png");
                }
            }
            for (int i = 0; i < imgIce.Length; i++)
            {
                if (imgIce[i] == null)
                {
                    imgIce[i] = GameCanvas.loadImage($"/bag/imgice{i}.png");
                }
            }
            for (int i = 0; i < indexTabSkill.Length; i++)
            {
                if (indexTabSkill[i] == null)
                {
                    indexTabSkill[i] = GameCanvas.loadImage($"/bag/numtab{i}.png");
                }
            }
            for (int i = 0; i < selectmenu.Length; i++)
            {
                if (selectmenu[i] == null)
                {
                    selectmenu[i] = GameCanvas.loadImage($"/bag/selectmenu{i}.png");
                }
            }
            for (int i = 0; i < buttonmem.Length; i++)
            {
                if (buttonmem[i] == null)
                {
                    buttonmem[i] = GameCanvas.loadImage($"/bag/buttonmem{i}.png");
                }
            }
            for (int i = 0; i < tilemap.Length; i++)
            {
                if (tilemap[i] == null)
                {
                    tilemap[i] = GameCanvas.loadImage($"/bag/tile{i}.png");
                }
            }
            for (int i = 0; i < sticker.Length; i++)
            {
                if (sticker[i] == null)
                {
                    sticker[i] = GameCanvas.loadImage($"/stickergoku/tile0{i}.png");
                }
            }
            InitScroll();
            InitScrollNoiTai();
            InitScrollClan();
            InitScrollClans();
            InitScrollChatClan();
            InitScrollSticker();
            InitscrollInfoSkill();
            /*UpdateLayout();*/
            if (maxTabSkill == -1)
            {
                initMaxTabSkill();
            }
            tabs = new Tab[]
            {
                new Tab("Lấy Ra", this, 1 , null),
                new Tab("Dùng", this, 2, null),
                new Tab("Vứt Ra", this, 3, null),
                new Tab("Cất Vào", this, 4, null),
                new Tab("Cho Đệ", this, 5, null),
                new Tab("Đóng", this, 6, null),
            };
            tabSkills = new TabSkill[]
            {
                new TabSkill("+ " + PointPlus[typeNumPoint] * 20, this, 7 , null),
                new TabSkill("+ " + PointPlus[typeNumPoint] * 10, this, 8, null),
                new TabSkill("+ " + PointPlus[0], this, 9, null),

            };
            imgX = GameCanvas.loadImage("/mainImage/myTexture2dbtx.png");
            for (int i = 0; i < imgTab.Length; i++)
            {
                if (imgTab[i] == null)
                {
                    imgTab[i] = GameCanvas.loadImage($"/bag/tab{i}.png");
                }
            }
        }
        public void initMaxTabSkill()
        {
            int num = Char.myCharz().nClass.skillTemplates.Length + 1;
            maxTabSkill = UnityEngine.Mathf.Max(1, UnityEngine.Mathf.CeilToInt(num / 5f));
        }
        public void update()
        {
            if (!isShow) return;

            if (tabIcon != null && tabIcon.isShow)
            {
                tabIcon.update();
                return;
            }
            if (isShow && !GameScr.isPaintOther)
            {
                GameScr.isPaintOther = true;
            }
            if (currTab == HANH_TRANG || currTab == THONG_TIN || currTab == DE_TU)
            {

                if (isSpeacialSkill)
                {
                    updateScroll(scrollNoitai);
                }
                if (isItemInfo)
                {
                    updateScroll(scrollInfo);
                }
                else
                {
                    updateScroll(scrollBag);
                }

            }

            if (currTab == THONG_TIN && currentSkill != null)
            {
                updateScroll(scrollInfoSkill);
            }
            if (currTab == BANG_HOI && isShowchatClan)
            {

                updateScroll(scrollChatClan);
                updateScroll(scrollSticker);
                for (int i = 0; i < ClanMessage.vMessage.size(); i++)
                {
                    ((ClanMessage)ClanMessage.vMessage.elementAt(i)).update();
                }
            }
            if (isShowMemClan && !isShowInfomem)
            {
                updateScroll(scrollMemClan);
            }
            if (currTab == BANG_HOI && Char.myCharz().clan == null)
            {
                updateScroll(scrollClans);
            }
            if (currclan != null && isShowMemClan && currmember == null)
            {
                updateScroll(scrollMemClan);
            }

        }
        public void updateScroll(Scroll sc)
        {
            if (sc != null && sc.cmyLim > 0)
            {
                ScrollResult s = sc.updateKey();
                if (sc.cmy < 0 && !s.isDowning)
                {
                    sc.cmy -= sc.cmy / 2;
                }
                else if (sc.cmy > sc.cmyLim && !s.isDowning)
                {
                    sc.cmy -= (sc.cmy - sc.cmyLim + 6) / 2;
                }
                if (s.isDowning) GameCanvas.isPointerMove = false;
                GameCanvas.clearKeyHold();
                sc.updatecm();
            }
        }
        public void updateKey()
        {
            if (!isShow) return;

            if (currTab != BANG_HOI)
            {

                if (scrollBag != null && currTab != BANG_HOI && !isItemInfo)
                {
                    scrollBag.updateKey();
                }
                if (scrollNoitai != null)
                {
                    scrollNoitai.updateKey();
                }


                if (scrollInfo != null)
                {
                    scrollInfo.updateKey();
                }
                if (scrollInfoSkill != null)
                {
                    scrollInfoSkill.updateKey();
                }
            }
            else
            {
                if (tabIcon != null && tabIcon.isShow)
                {
                    tabIcon.updateKey();
                }
                else
                {
                    if (scrollMemClan != null && isShowMemClan && !isShowInfomem)
                    {
                        scrollMemClan.updateKey();
                    }
                    if (currTab == BANG_HOI && Char.myCharz().clan == null)
                    {
                        scrollClans.updateKey();
                    }
                    if (scrollChatClan != null && isShowchatClan)
                    {
                        scrollChatClan.updateKey();
                    }
                    if (scrollSticker != null && isPaintStickers)
                    {
                        scrollSticker.updateKey();
                    }
                }
            }
            if (currclan != null && isShowMemClan && currmember == null)
            {
                scrollMemClan.updateKey();
            }
        }
        private void InitScroll()
        {
            int x1 = X + 1, y1 = Y, w1 = W - 2, h1 = H - 2;
            int x2 = x1 + 2, y2 = y1 + 2, w2 = w1 - 4, h2 = h1 - 1;
            int x3 = x2 + 2, y3 = y2, w3 = w2 - 4, h3 = h2 - 4;
            int x4 = x3 + 3, y4 = y3 + 2, w4 = w3 - 4, h4 = h3 - 4;
            int x5 = x4 + 4, y5 = y4 + 25, w5 = w4 - 8, h5 = h4 - 30;
            int x6 = x5 + 1, y6 = y5 + 1, w6 = w5 - 2, h6 = h5 - 2;
            int x7 = x6 + 4, y7 = y6 + 8, w7 = w6 / 2 + 2, h7 = h6 - 16;
            int x10 = x7 + w7 + 5;
            int y10 = y7 + 20;
            int w10 = w7 - 16;
            int h10 = h7 - 20;
            GameCanvas.clearAllPointerEvent();
            Item[] items = currIndex == 0 ? Char.myCharz().arrItemBag : Char.myCharz().arrItemBox;
            numCol2 = 6;
            numRow2 = UnityEngine.Mathf.Max(6, UnityEngine.Mathf.CeilToInt(items.Length / 6f) + 1);
            scrollBag = new Scroll();
            scrollBag.setStyle(numCol2 * numRow2, btnItem.getWidth() + 5, x10, y10 + btnItem.getHeight(), (btnItem.getWidth() + 5) * 6, h10, true, 6);
        }
        private void InitScrollNoiTai()
        {
            GameCanvas.clearAllPointerEvent();
            scrollNoitai = new Scroll();
        }
        private void InitScrollClan()
        {
            GameCanvas.clearAllPointerEvent();
            scrollMemClan = new Scroll();
        }
        private void InitScrollClans()
        {
            GameCanvas.clearAllPointerEvent();
            scrollClans = new Scroll();
        }
        private void InitScrollChatClan()
        {
            GameCanvas.clearAllPointerEvent();
            scrollChatClan = new Scroll();
        }
        private void InitScrollSticker()
        {
            GameCanvas.clearAllPointerEvent();
            scrollSticker = new Scroll();
        }
        private void InitscrollInfoSkill()
        {
            GameCanvas.clearAllPointerEvent();
            scrollInfoSkill = new Scroll();
        }
        private void InitScrollInfo()
        {
            int x1 = X + 1, y1 = Y, w1 = W - 2, h1 = H - 2;
            int x2 = x1 + 2, y2 = y1 + 2, w2 = w1 - 4, h2 = h1 - 1;
            int x3 = x2 + 2, y3 = y2, w3 = w2 - 4, h3 = h2 - 4;
            int x4 = x3 + 3, y4 = y3 + 2, w4 = w3 - 4, h4 = h3 - 4;
            int x5 = x4 + 4, y5 = y4 + 25, w5 = w4 - 8, h5 = h4 - 30;
            int x6 = x5 + 1, y6 = y5 + 1, w6 = w5 - 2, h6 = h5 - 2;
            int x7 = x6 + 4, y7 = y6 + 8, w7 = w6 / 2 + 2, h7 = h6 - 16;
            int x10 = x7 + w7 + 5;
            int y10 = y7 + 20;
            int w10 = w7 - 16;
            int h10 = h7 - 20;
            if (itemDetails.itemOption.Length > 5)
            {

                GameCanvas.clearAllPointerEvent();
                scrollInfo = new Scroll();
                scrollInfo.clear();
                if (itemDetails != null && itemDetails.itemOption != null)
                    scrollInfo.setStyle(itemDetails.itemOption.Length, mFont.tahoma_7b_green.getHeight() + itemDetails.itemOption.Length, x7 + 5, y7 + 40, w7 - 10, h7 - 80, true, 1);
            }
        }
        public void Paint(mGraphics g)
        {
            try
            {
                if (!isShow) return;
                g.setColor(0x0588bc);
                // viền ngoài 
                g.fillRect(X, Y, W, H, 10);
                int x1 = X + 1, y1 = Y, w1 = W - 2, h1 = H - 2;
                g.setColor(0x108fb7, 1f);
                // viền ngoài 
                g.fillRect(x1, y1, w1, h1, 10);
                int x2 = x1 + 2, y2 = y1 + 2, w2 = w1 - 4, h2 = h1 - 1;
                g.setColor(0x00a2d6);
                // viền ngoài 
                g.fillRect(x2, y2, w2, h2, 8);
                int x3 = x2 + 2, y3 = y2, w3 = w2 - 4, h3 = h2 - 4;
                g.setColor(0x00baf1, 2f);
                // viền ngoài 
                g.fillRect(x3, y3, w3, h3, 4);
                int x4 = x3 + 3, y4 = y3 + 2, w4 = w3 - 4, h4 = h3 - 4;
                g.setColor(0x06d8ff, 2f);
                // viền ngoài 
                g.fillRect(x4, y4, w4, h4, 4);
                // mFont.tahoma_7b_green.drawStringBd(g, "@C-U-O-N-G", GameCanvas.w / 2 + x4 + 10, y4 + 2, mFont.LEFT, mFont.tahoma_7);

                paintSnow(g);
                g.drawImage(imgX, GameCanvas.w - x4 - 10 - imgX.getWidth(), y4, 0);
                if (GameCanvas.isPointerHoldIn(GameCanvas.w - x4 - 10 - imgX.getWidth(), y4, imgX.getWidth(), imgX.getHeight()))
                {
                    if (GameCanvas.isPointerJustRelease && GameCanvas.isPointerClick)
                    {
                        itemDetails = null;
                        isItemInfo = false;
                        isBody = true;
                        isShow = false;
                        GameScr.isPaintOther = false;
                        SoundMn.gI().buttonClose();
                        GameCanvas.clearAllPointerEvent();
                    }
                }


                g.drawImage((currTab == HANH_TRANG) ? imgTab[1] : imgTab[0], x4 + imgTab[0].getWidth() / 2 + 5, y4 + imgTab[0].getHeight() / 2 + 5, mGraphics.VCENTER | mGraphics.HCENTER);
                mFont.tahoma_7b_white.drawStringBd(g, "Hành trang", x4 + imgTab[0].getWidth() / 2 + 5, y4 + imgTab[0].getHeight() / 2, mGraphics.VCENTER | mGraphics.HCENTER, mFont.tahoma_7b_dark);
                g.drawImage((currTab == THONG_TIN) ? imgTab[1] : imgTab[0], x4 + imgTab[0].getWidth() + imgTab[0].getWidth() / 2 + 5, y4 + imgTab[0].getHeight() / 2 + 5, mGraphics.VCENTER | mGraphics.HCENTER);
                mFont.tahoma_7b_white.drawStringBd(g, "Thông tin", x4 + imgTab[0].getWidth() + imgTab[0].getWidth() / 2 + 5, y4 + imgTab[0].getHeight() / 2, mGraphics.VCENTER | mGraphics.HCENTER, mFont.tahoma_7b_dark);
                g.drawImage((currTab == BANG_HOI) ? imgTab[1] : imgTab[0], x4 + imgTab[0].getWidth() * 2 + imgTab[0].getWidth() / 2 + 5, y4 + imgTab[0].getHeight() / 2 + 5, mGraphics.VCENTER | mGraphics.HCENTER);
                mFont.tahoma_7b_white.drawStringBd(g, "Bang hội", x4 + imgTab[0].getWidth() * 2 + imgTab[0].getWidth() / 2 + 5, y4 + imgTab[0].getHeight() / 2, mGraphics.VCENTER | mGraphics.HCENTER, mFont.tahoma_7b_dark);
                g.drawImage((currTab == DE_TU) ? imgTab[1] : imgTab[0], x4 + imgTab[0].getWidth() * 3 + imgTab[0].getWidth() / 2 + 5, y4 + imgTab[0].getHeight() / 2 + 5, mGraphics.VCENTER | mGraphics.HCENTER);
                mFont.tahoma_7b_white.drawStringBd(g, "Đệ tử", x4 + imgTab[0].getWidth() * 3 + imgTab[0].getWidth() / 2 + 5, y4 + imgTab[0].getHeight() / 2, mGraphics.VCENTER | mGraphics.HCENTER, mFont.tahoma_7b_dark);
                /*g.drawImage((currTab == CHUC_NANG) ? imgTab[1] : imgTab[0], x4 + imgTab[0].getWidth() * 4 + imgTab[0].getWidth() / 2 + 5, y4 + imgTab[0].getHeight() / 2 + 5, mGraphics.VCENTER | mGraphics.HCENTER);
                mFont.tahoma_7b_white.drawStringBd(g, "Chức năng", x4 + imgTab[0].getWidth() * 4 + imgTab[0].getWidth() / 2 + 5, y4 + imgTab[0].getHeight() / 2, mGraphics.VCENTER | mGraphics.HCENTER, mFont.tahoma_7b_dark);*/
                int x5 = x4 + 4, y5 = y4 + 25, w5 = w4 - 8, h5 = h4 - 30;
                if (Char.myCharz().clan != null && GameScr.isNewClanMessage && GameCanvas.gameTick % 4 == 0)
                {
                    g.drawImage(ItemMap.imageFlare, x4 + imgTab[0].getWidth() * 2 + imgTab[0].getWidth() / 2 + 5, y4 + imgTab[0].getHeight() / 2, mGraphics.VCENTER | mGraphics.HCENTER);
                }
                g.setColor(0, 1f);
                // viền ngoài kế 2 nút bản thân và đệ tử
                g.fillRect(x5, y5, w5, h5, 4);
                int x6 = x5 + 1, y6 = y5 + 1, w6 = w5 - 2, h6 = h5 - 2;
                g.setColor(0x88ddf4, 3f);
                // viền ngoài kế tiếp
                g.fillRect(x6, y6, w6, h6, 4);
                int x7 = x6 + 4, y7 = y6 + 8, w7 = w6 / 2 + 2, h7 = h6 - 16;
                g.setColor(0x19162e); // nền body
                                      // nền  nhân vật 
                if (currTab == CHUC_NANG)
                {
                    g.fillRect(x7, y7, (w7 * 2) - 10, h7, 4);
                }
                if (isBody)
                {
                    if (currTab != BANG_HOI && currTab != CHUC_NANG)
                    {
                        g.fillRect(x7, y7, w7, h7, 4);
                        g.setClip(x7 + 2, y7 + 2, w7 - 4, h7 - 4);
                        g.drawImageScale(bgplayer, x7 + 2, y7 + 2, w7 - 4, h7 - 4, 0);
                    }
                    int cx = x7 + w7 / 2;
                    int cy = y7 + h7 / 2 - TileMap.bong.getHeight() / 2;
                    #region EFFECT & MYCHARZ
                    if (currTab == HANH_TRANG || currTab == THONG_TIN)
                    {
                        int index = (GameCanvas.gameTick / 100) % 3;
                        g.drawImage(tilemap[index], x7 + w7 / 2, y7 + h7 / 3 - 14, mGraphics.VCENTER | mGraphics.HCENTER);



                        if (currTabBody == 1)
                        {
                            g.drawImage(left, x7 + w7 / 2 - 40, y7 + h7 / 2 - 20, mGraphics.VCENTER | mGraphics.HCENTER);
                        }
                        else
                        {
                            g.drawImage(right, x7 + w7 / 2 + 40, y7 + h7 / 2 - 20, mGraphics.VCENTER | mGraphics.HCENTER);
                        }
                        g.drawImage(TileMap.bong, x7 + w7 / 2, y7 + h7 / 2 + 3, mGraphics.VCENTER | mGraphics.HCENTER);
                        Char.myCharz().paintCharBody(g, cx, cy + 5, 1, Char.myCharz().cf, true);
                        // Char.myCharz().paintMountvip(g,cx, cy + 5);
                        for (int i = 0; i < Char.myCharz().vEffChar.size(); i++)
                        {
                            Effect effect = (Effect)Char.myCharz().vEffChar.elementAt(i);
                            if (effect.layer == 1)
                            {
                                bool flag = true;
                                if (effect.isStand == 0)
                                {
                                    flag = ((Char.myCharz().statusMe == 1 || Char.myCharz().statusMe == 6) ? true : false);
                                }
                                if (flag)
                                {
                                    effect.x = cx;
                                    effect.y = cy + 20;
                                    effect.paint(g);
                                }
                            }
                        }
                    }
                    if (currTab == DE_TU)
                    {
                        int index = (GameCanvas.gameTick / 100) % 3;
                        g.drawImage(tilemap[index], x7 + w7 / 2, y7 + h7 / 3 - 14, mGraphics.VCENTER | mGraphics.HCENTER);
                        if (currTabBody == 1)
                        {
                            g.drawImage(left, x7 + w7 / 2 - 40, y7 + h7 / 2 - 20, mGraphics.VCENTER | mGraphics.HCENTER);
                        }
                        else
                        {
                            g.drawImage(right, x7 + w7 / 2 + 40, y7 + h7 / 2 - 20, mGraphics.VCENTER | mGraphics.HCENTER);
                        }
                        Char.myPetz().paintCharBody(
                            g, cx, cy + 5, 1, Char.myPetz().cf, true);
                        for (int i = 0; i < Char.myPetz().vEffChar.size(); i++)
                        {
                            Effect effect = (Effect)Char.myPetz().vEffChar.elementAt(i);
                            if (effect.layer == 1)
                            {
                                bool flag = true;
                                if (effect.isStand == 0)
                                {
                                    flag = ((Char.myPetz().statusMe == 1 || Char.myPetz().statusMe == 6) ? true : false);
                                }
                                if (flag)
                                {
                                    effect.x = cx;
                                    effect.y = cy + 20;
                                    effect.paint(g);
                                }
                            }
                        }
                    }
                    #region BANGHOI
                    else if (currTab == BANG_HOI)
                    {
                        if (Char.myCharz().clan != null)
                        {
                            // RIGHT
                            int wNew = w7 - 20;

                            if (isShowchatClan)
                            {


                                int[] Hmess = new int[ClanMessage.vMessage.size()];
                                int highmess = 0;
                                int x7r = x7 + wNew + 2;
                                int y7r = y7;
                                int w7r = w7;
                                int h7r = h7;
                                /*g.setColor(0xFFF3B0);
                                g.fillRect(x7, y7, (w7 * 2) - 10, h7, 4);*/
                                g.setClip(x7, y7, (w7 * 2) - 10, h7);
                                g.drawImage(bgClan, (x7 + w7), y7 + h7 / 2, mGraphics.VCENTER | mGraphics.HCENTER);

                                /*g.setColor(0x06d7fe);
                                g.fillRect((x7 + w7 * 2) - 155, (y7 + h7) - 30, 145, 30, 4);*/

                                int hUpdate = 0;
                                /*g.setColor(0x21130d);
                                g.drawRect(x7r, y7r, w7r, h7r);*/
                                if (ClanMessage.vMessage.size() > 0)
                                {
                                    // Tọa độ y ban đầu
                                    for (int j = ClanMessage.vMessage.size() - 1; j >= 0; j--) // Duyệt ngược từ cuối về đầu
                                    {
                                        ClanMessage clanMessage = (ClanMessage)ClanMessage.vMessage.elementAt(j);
                                        ClanMessage clanMessageBefore = null;

                                        // Kiểm tra xem có tin nhắn trước đó không
                                        if (j + 1 >= 0)
                                        {
                                            clanMessageBefore = (ClanMessage)ClanMessage.vMessage.elementAt(j);
                                        }

                                        string chatfull = "";
                                        if (clanMessageBefore != null && clanMessage.type == 0)
                                        {
                                            for (int i = 0; i < clanMessageBefore.chat.Length; i++)
                                            {
                                                chatfull += clanMessageBefore.chat[i];
                                            }
                                            string[] array = mFont.tahoma_7_greySmall.splitFontArray(chatfull, 200);
                                            highmess = array.Length == 1 ? array.Length * 28 : array.Length * 18;
                                            if (j == 0)
                                            {
                                                highmess = highmess + 5;
                                            }
                                            if (chatfull.Contains("Gửi Sticker "))
                                            {
                                                int idSticker = -1;
                                                chatfull = chatfull.Replace("Gửi Sticker ", "").Trim();
                                                if (int.TryParse(chatfull, out idSticker))
                                                {
                                                    highmess = sticker[idSticker].getHeight() + 10;
                                                }

                                            }
                                            Hmess[j] = highmess;

                                        }
                                        else
                                        {
                                            if (clanMessage.type == 2 && Char.myCharz().role != 0)
                                            {
                                                highmess = 28;
                                            }
                                            else
                                            {
                                                highmess = 50;
                                            }
                                            Hmess[j] = highmess;
                                        }
                                        hUpdate = highmess;




                                    }
                                    int ynum7 = y7;  // Tọa độ y ban đầu

                                    for (int j = ClanMessage.vMessage.size() - 1; j >= 0; j--)
                                    {
                                        ClanMessage clanMessage = (ClanMessage)ClanMessage.vMessage.elementAt(j);

                                        // Cộng dồn tọa độ y với chiều cao của tin nhắn hiện tại
                                        ynum7 += Hmess[j];
                                    }
                                    if (ynum7 > h7)
                                    {
                                        scrollChatClan.setStyle(Hmess, x7, y7, w7, h7, true);
                                        g.setClip(x7, y7, (w7 * 2) - 10, h7);
                                        g.translate(scrollChatClan.cmx, -scrollChatClan.cmy);

                                        /*if (scrollChatClan.cmtoY >= (scrollChatClan.cmyLim - sticker[0].getHeight()))
                                        {
                                            scrollChatClan.cmtoY = scrollChatClan.cmyLim;
                                        }*/
                                    }
                                    // Vẽ từng ClanMessage với tọa độ y động
                                    int ynum = y7;  // Tọa độ y ban đầu
                                    for (int j = ClanMessage.vMessage.size() - 1; j >= 0; j--)
                                    {
                                        ClanMessage clanMessage = (ClanMessage)ClanMessage.vMessage.elementAt(j);
                                        paintMessClan(g, clanMessage, x7 + 5, ynum);

                                        // Cộng dồn tọa độ y với chiều cao của tin nhắn hiện tại
                                        ynum += Hmess[j];
                                    }

                                }
                                g.reset();
                                g.setColor(0x87dcf3, 0.3f);
                                g.fillRect((x7 + w7 * 2) - 150, (y7 + h7) - 35, 140, 35, 4);

                                g.setColor(0x06d7fe, 0.3f);
                                g.fillRect((x7 + w7 * 2) - 147, (y7 + h7) - 32, 134, 29, 4);

                                g.drawImage(chaticon, (x7 + w7 * 2) - 130, (y7 + h7) - 19, mGraphics.VCENTER | mGraphics.HCENTER);
                                g.drawImage(stickericon, (x7 + w7 * 2) - 80, (y7 + h7) - 19, mGraphics.VCENTER | mGraphics.HCENTER);
                                g.drawImage(xindau, (x7 + w7 * 2) - 27, (y7 + h7) - 19, mGraphics.VCENTER | mGraphics.HCENTER);

                                paintPopupSticker(g, (x7 + w7 * 2) - 150, y7 + 20, 140, 140, (x7 + w7 * 2) - 150, (y7 + h7) - 35, 140, 35);


                                if (GameCanvas.isPointerHoldIn((x7 + w7 * 2) - 130 - (chaticon.getWidth() / 2), (y7 + h7) - 19 - (chaticon.getHeight() / 2), chaticon.getWidth(), chaticon.getHeight()))
                                {
                                    if (GameCanvas.isPointerJustRelease && GameCanvas.isPointerClick)
                                    {
                                        chatClan();
                                        GameCanvas.clearAllPointerEvent();

                                    }
                                }
                                if (GameCanvas.isPointerHoldIn((x7 + w7 * 2) - 27 - (xindau.getWidth() / 2), (y7 + h7) - 19 - (xindau.getHeight() / 2), xindau.getWidth(), xindau.getHeight()))
                                {
                                    if (GameCanvas.isPointerJustRelease && GameCanvas.isPointerClick)
                                    {
                                        Service.gI().clanMessage(1, "", -1);
                                        GameCanvas.clearAllPointerEvent();

                                    }
                                }
                                if (GameCanvas.isPointerHoldIn((x7 + w7 * 2) - 80 - (stickericon.getWidth() / 2), (y7 + h7) - 19 - (stickericon.getHeight() / 2), stickericon.getWidth(), stickericon.getHeight()))
                                {
                                    if (GameCanvas.isPointerJustRelease && GameCanvas.isPointerClick)
                                    {
                                        isPaintStickers = !isPaintStickers;
                                        GameCanvas.clearAllPointerEvent();

                                    }
                                }
                                if (GameScr.isNewClanMessage)
                                {
                                    GameScr.isNewClanMessage = false;
                                    scrollChatClan.cmtoY = scrollChatClan.cmyLim - 5;
                                }
                                g.reset();

                            }
                            else if (isShowMemClan)
                            {

                                g.setClip(x7, y7, (w7 * 2) - 10, h7);
                                g.drawImage(bgClanMain, (x7 + w7), y7 + h7 / 2, mGraphics.VCENTER | mGraphics.HCENTER);
                                g.setColor(0x967b55);
                                g.fillRect(x7 + 140, y7, w7 / 2 + 20, 20, 4);
                                g.setColor(0xefefb2);
                                g.fillRect(x7 + 142, y7 + 2, w7 / 2 + 16, 16, 4);
                                mFont.tahoma_7b_dark.drawString(g, "Thành viên bang", x7 + w7, y7 + 3, mGraphics.VCENTER | mGraphics.HCENTER);
                                paintMemberClan(g, x7 + 10, y7, (w7 * 2) - 30, h7);


                            }
                            else
                            {
                                // LEFT
                                g.setClip(x7, y7, (w7 * 2) - 10, h7);
                                g.drawImage(bgClanMain, (x7 + w7), y7 + h7 / 2, mGraphics.VCENTER | mGraphics.HCENTER);

                                g.setColor(0x000000, 0.7f); // nền body
                                g.fillRect(x7, y7, wNew, h7 / 3, 4);

                                g.setColor(0x000000, 1f); // nền body
                                g.fillRect(x7 + 3, y7 + 7, 50, 50, 4);

                                g.setColor(0xe5ddd0, 1f);
                                g.fillRect(x7 + 5, y7 + 9, 46, 46, 4);

                                g.setColor(0x000000, 1f);
                                g.fillRect(x7 + 3, y7 + 7 + (h7 / 3) - 37, 20, 20, 4);

                                g.setColor(0xf1d738, 1f);
                                g.fillRect(x7 + 5, y7 + 9 + (h7 / 3) - 37, 16, 16, 4);

                                g.setColor(0x000000, 0.7f);
                                g.fillRect(x7 + wNew + 20, y7, wNew, 20, 4);
                                var c = Char.myCharz();

                                Clan clan = c.clan;
                                mFont.tahoma_7b_white.drawString(g, "" + clan.level, x7 + 13, y7 + 7 + (h7 / 3) - 32, mFont.CENTER);
                                ClanImage fl = ClanImage.getClanImage((short)clan.imgID);
                                if (tabIcon != null && tabIcon.lastSelect != null)
                                {
                                    fl = ((ClanImage)ClanImage.vClanImage.elementAt(tabIcon.lastSelect));
                                }
                                if (fl == null)
                                {

                                    Service.gI().getClan(3, -1, null);
                                }
                                if (fl != null && fl.idImage != null && fl.idImage.Length != 0)
                                {
                                    SmallImage.drawSmallImage(g, fl.idImage[0], x7 + 20, y7 + 25, 0, 2);
                                }
                                mFont.tahoma_7b_yellow.drawStringBd(g, "" + c.clan.name, (x7 + wNew / 2) - 20, y7 + 4, mFont.LEFT, mFont.tahoma_7b_white);
                                mFont.tahoma_7_whiteSmall.drawString(g, mResources.achievement_point + ": " + clan.powerPoint, (x7 + wNew / 2) - 20, y7 + 25, mFont.LEFT, mFont.tahoma_7_grey);
                                mFont.tahoma_7_whiteSmall.drawString(g, mResources.clan_point + ": " + clan.clanPoint, (x7 + wNew / 2) - 20, y7 + 40, mFont.LEFT, mFont.tahoma_7_grey);


                                TextInfo.paint(g, clan.slogan, x7 + wNew + 20, y7 + 5, wNew, 16, mFont.tahoma_7_whiteSmall);
                                paintLeader(g, getLeader(), x7 + w7, y7 + h7 - 50);
                                g.drawImage(TileMap.bong, x7 + w7, y7 + h7 - 47, mGraphics.VCENTER | mGraphics.HCENTER);
                                mFont.tahoma_7b_dark.drawString(g, getLeader().name, x7 + w7, y7 + (h7 / 2) - 15, mFont.CENTER);
                                mFont.tahoma_7b_red.drawStringBd(g, "Bang chủ", x7 + w7, y7 + (h7 / 2) - 30, mFont.CENTER, mFont.tahoma_7b_white);

                                if (Char.myCharz().role == 0)
                                {
                                    showLeaderMenu(g, x7, y7, w7, h7);
                                }
                                else
                                {
                                    simplebutton(g, "Rời\nBang", x7, y7 + h7 - 33, 35, 35, 0xefefb2, 0x967b55, 2);
                                }
                            }

                            g.reset();

                            showRightMenuClan(g, x7, y7, w7, h7);
                        }
                        else
                        {
                            if (isShowMemClan)
                            {
                                g.reset();
                                g.setClip(x7, y7, (w7 * 2) - 10, h7);
                                g.drawImage(bgClanMain, (x7 + w7), y7 + h7 / 2, mGraphics.VCENTER | mGraphics.HCENTER);
                                simplebutton(g, "Quay lại", x7, y7, 60, 20, 0xefefb2, 0x967b55, 7);
                                paintMemberClan(g, currclan, x7 + 10, y7, (w7 * 2) - 30, h7);
                            }
                            else
                            {
                                g.setClip(x7, y7, (w7 * 2) - 10, h7);
                                g.drawImage(bgClanMain, (x7 + w7), y7 + h7 / 2, mGraphics.VCENTER | mGraphics.HCENTER);
                                paintClans(g, x7, y7, w7 - 40, h7);

                                g.reset();
                                g.setColor(0x000000, 0.7f);
                                g.fillRect(x7 + w7 + 50, y7 + h7 - 25, 70, 20, 4);
                                Char.myCharz().paintCharBody(g, x7 + w7 + 85, y7 + h7 - 30, 1, Char.myCharz().cf, true);
                                mFont.tahoma_7b_red.drawString(g, "Chưa có bang", x7 + w7 + 85, y7 + h7 - 21, mFont.CENTER, mFont.tahoma_7b_dark);
                                simplebutton(g, "Tìm Bang", x7 + w7 + 20, y7, 60, 20, 0xefefb2, 0x967b55, 3);
                                simplebutton(g, "Tạo Bang", x7 + w7 + 90, y7, 60, 20, 0xefefb2, 0x967b55, 4);
                            }
                        }

                        if (tabIcon != null && tabIcon.isShow)
                        {
                            g.reset();
                            tabIcon.paint(g);
                        }

                    }

                    #endregion
                    #endregion

                    #region BODY
                    if (currTab == HANH_TRANG)
                    {
                        int x8 = x7 + 1;
                        int y8 = y7 + h7 / 2 + 35;
                        int w8 = w7 - 2;
                        int h8 = h7 / 4 + 15;
                        var c = Char.myCharz();
                        /*g.setColor(0x080808, 1f);
                        g.fillRect(x8, y8 - 5, w8, 10, 4);*/
                        /* g.setColor(0x196da9, 1f);
                         g.fillRect(x8, y8, w8, h8, 4);*/
                        // thanh thông tin ki sức mạnh 
                        g.drawImageScale(bginfo, x8, y8, w8, h8, 0);
                        g.setColor(0xffd080, 1f);
                        g.fillRect(x8 - 3, y8 - 1, w8 + 4, 2, 4);
                        Small small = SmallImage.imgNew[c.avatarz()];
                        if (small == null)
                        {
                            SmallImage.createImage(c.avatarz());
                            return;
                        }
                        Image avt = small.img;
                        /*g.setColor(0x000000, 1f);
                        g.fillRect(x8 + (w8 / 3), y8, (w8 * 2 / 3), 35, 4);

                        g.setColor(0x1e8aeb, 1f);
                        g.fillRect(x8 + (w8 / 3) + 2, y8 + 2, (w8 * 2 / 3) - 4, 31, 4);*/
                        g.setColor(0x68b5ea, 1f);
                        g.fillRect(x8 + (w8 / 3), y8, (w8 * 2 / 3), 35, 4);
                        g.drawImageScale(khunginfo, x8 + (w8 / 3) + 2, y8 + 2, (w8 * 2 / 3) - 4, 31, 0);

                        /* g.drawImage(imgBanner, x8 + (w8 *2/3), y8 + 17, mGraphics.VCENTER | mGraphics.HCENTER);*/
                        int thoiVangQuantity = GameCanvas.GetItemQuantity(457);
                        g.setClip(x8, y8, w8 / 3, h8);
                        SmallImage.drawSmallImage(g, c.avatarz(), x8 + mGraphics.getImageHeight(avt) / 2, y8 + h8, 0, 33);
                        g.reset();
                        mFont.tahoma_7b_white.drawStringBd(g, c.cName, (x8 + w8 / 3) + 10, y8 + 4, mFont.LEFT, mFont.tahoma_7b_dark);
                        g.drawImage(Panel.imgXu, (x8 + w8 / 3) + 10, y8 + 43, 3);
                        g.drawImage(Panel.imgLuong, (x8 + w8 / 3) + 10, y8 + 59, 3);
                        g.drawImage(Panel.imgLuongKhoa, x8 + (w8 * 2 / 3) + 10, y8 + 43, 3);
                        g.drawImage(imgSlThoiVang, x8 + (w8 * 2 / 3) + 10, y8 + 59, 3);

                        mFont.tahoma_7_yellow.drawStringBorder(g, Char.myCharz().xuStr + string.Empty, (x8 + w8 / 3) + 23, y8 + 37, mFont.LEFT, mFont.tahoma_7_grey);
                        mFont.tahoma_7_yellow.drawStringBorder(g, Char.myCharz().luongStr + string.Empty, (x8 + w8 / 3) + 23, y8 + 53, mFont.LEFT, mFont.tahoma_7_grey);
                        mFont.tahoma_7_yellow.drawStringBorder(g, Char.myCharz().luongKhoaStr + string.Empty, x8 + (w8 * 2 / 3) + 10 + 13, y8 + 37, mFont.LEFT, mFont.tahoma_7_grey);
                        mFont.tahoma_7_yellow.drawStringBorder(g, thoiVangQuantity.ToString(), x8 + (w8 * 2 / 3) + 10 + 13, y8 + 53, mFont.LEFT, mFont.tahoma_7_grey);
                        if (c.cPower > 0)
                        {
                            mFont.tahoma_7_yellow.drawStringBd(g, (!c.me) ? c.currStrLevel : c.getStrLevel(), (x8 + w8 / 3) + 7, y8 + 17, mFont.LEFT, mFont.tahoma_7_grey);
                        }
                    }
                    if (currTab == THONG_TIN || currTab == DE_TU)
                    {
                        int x8 = x7 + 1;
                        int y8 = y7 + h7 / 2 + 35;
                        int w8 = w7 - 2;
                        int h8 = h7 / 4 + 15;
                        /*g.setColor(0x080808, 1f);
                        g.fillRect(x8, y8 - 5, w8, 10, 4);*/
                        var c = Char.myCharz();
                        if (currTab == DE_TU) c = Char.myPetz();
                        g.setColor(0x196da9, 1f);
                        // thanh thông tin ki sức mạnh 
                        g.fillRect(x8, y8, w8, h8, 4);
                        //Power
                        mFont.bigNumber_While.drawString(g, "SM:", x8 + mFont.tahoma_7_yellow.getWidth("Power") + 2, y8 + 4, mFont.RIGHT);
                        g.drawImage(imgBoxPowwer, x8 + mFont.tahoma_7b_white.getWidth("Thể lực: ") / 2 + imgBox[0].getWidth() / 2 + 15, y8 + 10, mGraphics.VCENTER | mGraphics.HCENTER);
                        g.setColor(2486262);
                        mFont.tahoma_7b_white.drawStringBd(g, Res.formatNumber2(c.cPower), x8 + mFont.tahoma_7b_white.getWidth("PW: ") / 2 + imgBox[0].getWidth() / 2 + 22, y8 + 5, mGraphics.VCENTER | mGraphics.HCENTER, mFont.tahoma_7b_dark);
                        //Guild
                        int xBox1 = x8 + mFont.tahoma_7_yellow.getWidth("Thể lực: ") / 2 + imgBox[0].getWidth() / 2 + 15;
                        int yBox1 = y8 + 15 + imgBox[0].getHeight();
                        mFont.tahoma_7_yellow.drawStringBd(g, (currTab == DE_TU) ? "Status" : "T.Năng", x8 + mFont.tahoma_7_yellow.getWidth("T.Năng") + 2, y8 + 20, mFont.RIGHT, mFont.tahoma_7b_dark);
                        g.drawImage(imgNameClan, x8 + mFont.tahoma_7b_white.getWidth("Thể lực: ") / 2 + imgBox[0].getWidth() / 2 + 15, y8 + 26, mGraphics.VCENTER | mGraphics.HCENTER);
                        mFont.tahoma_7b_white.drawStringBd(g, (currTab == DE_TU) ? strStatus[Char.myPetz().petStatus] : Res.formatNumber2(c.cTiemNang), xBox1 + 2, yBox1 - 5, mGraphics.VCENTER | mGraphics.HCENTER, mFont.tahoma_7_grey);
                        //Stamina
                        mFont.tahoma_7_yellow.drawStringBd(g, "Thể lực", x8 + mFont.tahoma_7_yellow.getWidth("Thể lực") + 2, y8 + 35, mFont.RIGHT, mFont.tahoma_7b_dark);
                        g.drawImage(imgBox[0], x8 + mFont.tahoma_7_yellow.getWidth("Thể lực: ") / 2 + imgBox[0].getWidth() / 2 + 15, y8 + 30 + imgBox[0].getHeight(), mGraphics.VCENTER | mGraphics.HCENTER);
                        int xBox2 = x8 + mFont.tahoma_7_yellow.getWidth("Thể lực: ") / 2 + imgBox[0].getWidth() / 2 + 15;
                        int yBox2 = y8 + 30 + imgBox[0].getHeight();
                        int clipS;
                        if (c.cMaxStamina != 0)
                        {
                            clipS = c.cStamina * imgBox[0].getWidth() / c.cMaxStamina;
                        }
                        else
                        {
                            clipS = 100;
                        }
                        g.setColor(0xffff00);
                        g.fillRect(xBox2 + 1 - imgBox[0].getWidth() / 2, yBox2 + 1 - imgBox[0].getHeight() / 2, clipS - 3, imgBox[0].getHeight() - 2);
                        mFont.tahoma_7_white.drawStringBd(g, NinjaUtil.getMoneys(c.cStamina), x8 + imgBox[0].getWidth() / 2 + 22, yBox2 + 1 - imgBox[0].getHeight() / 2 - 2, mFont.LEFT, mFont.tahoma_7b_dark);
                        //HP
                        int x9 = x8 + w8 / 2 + 20;
                        mFont.tahoma_7_yellow.drawStringBd(g, "HP", x9, y8 + 5, mFont.RIGHT, mFont.tahoma_7b_dark);
                        g.drawImage(imgBox[0], x9 + imgBox[0].getWidth() / 2 + 5, y8 + 5, mFont.RIGHT);
                        int clipHP = (int)(c.cHP * imgBox[0].getWidth() / c.cHPFull);
                        g.setColor(16713995);
                        g.fillRect(x9 + 6, y8 + 6, clipHP - 2, imgBox[0].getHeight() - 2);
                        mFont.tahoma_7_white.drawStringBd(g, Res.formatNumber2(c.cHP), x9 - mFont.tahoma_7b_white.getWidth("HP") / 2 + imgBox[0].getWidth() / 2 + 12, y8 + 4, mGraphics.VCENTER | mGraphics.HCENTER, mFont.tahoma_7b_dark);
                        //KI
                        mFont.tahoma_7_yellow.drawStringBd(g, "MP", x9, y8 + 20, mFont.RIGHT, mFont.tahoma_7b_dark);
                        g.drawImage(imgBox[0], x9 + imgBox[0].getWidth() / 2 + 5, y8 + 10 + imgBox[0].getHeight(), mFont.RIGHT);
                        int clipKI = (int)(c.cMP * imgBox[0].getWidth() / c.cMPFull);
                        g.setColor(2486262);
                        g.fillRect(x9 + 6, y8 + imgBox[0].getHeight() + 11, clipKI - 2, imgBox[0].getHeight() - 2);
                        mFont.tahoma_7_white.drawStringBd(g, Res.formatNumber2(c.cMP), x9 - mFont.tahoma_7b_white.getWidth("MP") / 2 + imgBox[0].getWidth() / 2 + 12, y8 + 20, mGraphics.VCENTER | mGraphics.HCENTER, mFont.tahoma_7b_dark);
                        //DMG
                        mFont.tahoma_7_yellow.drawStringBd(g, "SĐ", x9, y8 + 35, mFont.RIGHT, mFont.tahoma_7b_dark);
                        g.drawImage(imgBox[0], x9 + imgBox[0].getWidth() / 2 + 5, y8 + 15 + imgBox[0].getHeight() * 2, mFont.RIGHT);
                        g.setColor(0xFA078C);


                        // chung màu hành trang
                        g.fillRect(x9 + 6, y8 + 16 + imgBox[0].getHeight() * 2, imgBox[0].getWidth() - 2, imgBox[0].getHeight() - 2);
                        mFont.tahoma_7_white.drawStringBd(g, Res.formatNumber2(c.cDamFull), x9 - mFont.tahoma_7b_white.getWidth("SĐ") / 2 + imgBox[0].getWidth() / 2 + 12, y8 + 35, mGraphics.VCENTER | mGraphics.HCENTER, mFont.tahoma_7b_dark);
                        //DMG
                        mFont.tahoma_7_yellow.drawStringBd(g, "CM", x9, y8 + 50, mFont.RIGHT, mFont.tahoma_7b_dark);
                        g.drawImage(imgBox[0], x9 + imgBox[0].getWidth() / 2 + 5, y8 + 30 + imgBox[0].getHeight() * 2, mFont.RIGHT);
                        g.setColor(0xFA9D07);

                        g.fillRect(x9 + 6, y8 + 31 + imgBox[0].getHeight() * 2, imgBox[0].getWidth() - 2, imgBox[0].getHeight() - 2);
                        mFont.tahoma_7_white.drawStringBd(g, NinjaUtil.getMoneys(c.cCriticalFull) + "%", x9 - mFont.tahoma_7b_white.getWidth("CM") / 2 + imgBox[0].getWidth() / 2 + 12, y8 + 50, mGraphics.VCENTER | mGraphics.HCENTER, mFont.tahoma_7b_dark);
                        //Chí mạng
                        mFont.tahoma_7_yellow.drawStringBd(g, "Giáp", x8 + mFont.tahoma_7_yellow.getWidth("CM   ") + 2, y8 + 50, mGraphics.VCENTER | mGraphics.HCENTER, mFont.tahoma_7b_dark);
                        g.drawImage(imgBox[0], x8 + mFont.tahoma_7_yellow.getWidth("Thể lực: ") / 2 + imgBox[0].getWidth() / 2 + 15, y8 + 30 + imgBox[0].getHeight() * 2, mFont.RIGHT);
                        g.setColor(6941494);

                        int xBox0 = x8 + mFont.tahoma_7_yellow.getWidth("Thể lực: ") / 2 + imgBox[0].getWidth() / 2 + 15;
                        g.fillRect(xBox0 + 1 - imgBox[0].getWidth() / 2, y8 + 31 + imgBox[0].getHeight() * 2, imgBox[0].getWidth() - 2, imgBox[0].getHeight() - 2);
                        mFont.tahoma_7_white.drawStringBd(g, Res.formatNumber2(c.cDefull), x8 + mFont.tahoma_7b_white.getWidth("Giáp") / 2 + imgBox[0].getWidth() / 2 + 22, y8 + 50, mGraphics.VCENTER | mGraphics.HCENTER, mFont.tahoma_7b_dark);
                    }
                    #endregion
                    #region ITEMBODY

                    if (currTab == HANH_TRANG || currTab == THONG_TIN || currTab == DE_TU)
                    {
                        var c = Char.myCharz();
                        if (currTab == DE_TU) c = Char.myPetz();
                        for (int row = 0; row < numRow; row++)
                        {
                            for (int col = 0; col < numCol; col++)
                            {
                                int x = x7 + 4 + col * (btnItem.getWidth() + 17);
                                int y = y7 + 4 + row * (btnItem.getHeight() + 2);
                                if (row < 5 && (col == 0 || col == numCol - 1))
                                {


                                    Item item = null;
                                    int index = -1;
                                    if (col == 0 && row <= 5)
                                    {
                                        int indexc = currTabBody == 0 ? row : row + 10;
                                        if (c.arrItemBody != null && indexc < c.arrItemBody.Length && c.arrItemBody[indexc] != null)
                                        {
                                            item = c.arrItemBody[indexc];
                                        }
                                        index = indexc;
                                    }
                                    else if (col == numCol - 1 && row <= 5)
                                    {
                                        int indexc = currTabBody == 0 ? row + 5 : row + 15;
                                        if (c.arrItemBody != null && indexc < c.arrItemBody.Length && c.arrItemBody[indexc] != null)
                                        {
                                            item = c.arrItemBody[indexc];
                                        }
                                        index = indexc;
                                    }
                                    g.drawImage(btnItem, x, y, 0);
                                    /*g.setColor(Bag.gI().selected == (sbyte)index ? 16383818 : 0xed9056, 1f);
                                    g.fillRect(x, y, Bag.gI().btnItem.getWidth(), Bag.gI().btnItem.getHeight(), 4);*/
                                    if (item != null)
                                    {
                                        SmallImage.drawSmallImage(g, item.template.iconID, x + btnItem.getWidth() / 2, y + btnItem.getHeight() / 2, 0, mGraphics.VCENTER | mGraphics.HCENTER);
                                        g.reset();
                                        if (GameCanvas.isPointerHoldIn(x, y, btnItem.getWidth(), btnItem.getHeight()) && (GameCanvas.isPointerClick || GameCanvas.isPointerJustDown))
                                        {
                                            if (GameCanvas.isPointerJustRelease)
                                            {
                                                isBody = false;
                                                itemDetails = item;
                                                isItemInfo = true;
                                                typePaint = TypePaint.typeBody;
                                                selected = (sbyte)index;
                                                InitScrollInfo();
                                                GameCanvas.clearAllPointerEvent();
                                            }
                                        }
                                    }
                                }

                            }
                        }
                    }
                    #endregion
                    g.reset();
                }
                g.drawImageScale(imgIce[5], X, Y + H - 13, W, imgIce[5].getHeight(), 0);
                #region ITEMBAG
                int x10 = x7 + w7 + 5;
                int y10 = y7 + 20;
                int w10 = w7 - 16;
                int h10 = h7 - 20;


                if (currTab != BANG_HOI && currTab != CHUC_NANG)
                {
                    /*g.setColor(0x5d5455);
                    g.fillRect(x10, y10, w10, h10, 4);*/
                    g.drawImageScale(bginven, x10, y10, w10, h10, 0);
                    g.drawImage((currIndex == 0) ? indexBag[1] : indexBag[0], x10 + w10 / 2 - indexBag[0].getWidth(), y10 - indexBag[0].getHeight() - 5);
                    mFont.tahoma_7b_white.drawStringBd(g, "Túi Đồ", x10 + w10 / 2 - indexBag[0].getWidth() + 10, y10 - indexBag[0].getHeight(), 0, mFont.tahoma_7b_dark);
                    g.drawImage((currIndex == 1) ? indexBag[1] : indexBag[0], x10 + w10 / 2 + 5, y10 - indexBag[0].getHeight() - 5);
                    mFont.tahoma_7b_white.drawStringBd(g, (currTab == HANH_TRANG) ? " Rương" : "  Skill", x10 + w10 / 2 + 12, y10 - indexBag[0].getHeight(), 0, mFont.tahoma_7b_dark);
                }

                if (GameCanvas.isPointerHoldIn(x10 + w10 / 2 - indexBag[0].getWidth(), y10 - indexBag[0].getHeight() - 5, indexBag[0].getWidth(), indexBag[0].getHeight()))
                {
                    if (GameCanvas.isPointerJustRelease && GameCanvas.isPointerClick)
                    {
                        if (scrollBag != null) scrollBag.cmtoY = 0;
                        currIndex = 0;
                        SoundMn.gI().panelClick();
                        GameCanvas.clearAllPointerEvent();
                    }
                }
                if (GameCanvas.isPointerHoldIn(x10 + w10 / 2 + 5, y10 - indexBag[0].getHeight() - 5, indexBag[0].getWidth(), indexBag[0].getHeight()))
                {
                    if (GameCanvas.isPointerJustRelease && GameCanvas.isPointerClick)
                    {
                        if (scrollBag != null) scrollBag.cmtoY = 0;
                        currIndex = 1;
                        SoundMn.gI().panelClick();
                        GameCanvas.clearAllPointerEvent();
                    }

                }
                if (GameCanvas.isPointerHoldIn(x4 + 5, (y4 + imgTab[0].getHeight() / 2 + 5) - imgTab[0].getHeight() + 10, imgTab[0].getWidth(), imgTab[0].getHeight()))
                {
                    if (GameCanvas.isPointerJustRelease && GameCanvas.isPointerClick)
                    {
                        isItemInfo = false;
                        isBody = true;
                        currTabBody = 0;
                        currIndex = 0;
                        if (scrollBag != null) scrollBag.cmtoY = 0;
                        currTab = HANH_TRANG;
                        SoundMn.gI().panelClick();
                        GameCanvas.clearAllPointerEvent();

                    }
                }
                if (GameCanvas.isPointerHoldIn(x4 + imgTab[0].getWidth() + 5, (y4 + imgTab[0].getHeight() / 2 + 5) - imgTab[0].getHeight() + 10, imgTab[0].getWidth(), imgTab[0].getHeight()))
                {

                    if (GameCanvas.isPointerJustRelease && GameCanvas.isPointerClick)
                    {
                        isItemInfo = false;
                        isBody = true;
                        if (scrollBag != null) scrollBag.cmtoY = 0;
                        currTab = THONG_TIN;
                        SoundMn.gI().panelClick();
                        if (currentSkill != null)
                        {
                            currentSkill = null;
                        }
                        GameCanvas.clearAllPointerEvent();
                    }
                }
                if (GameCanvas.isPointerHoldIn(x4 + imgTab[0].getWidth() * 2 + 5, (y4 + imgTab[0].getHeight() / 2 + 5) - imgTab[0].getHeight() + 10, imgTab[0].getWidth(), imgTab[0].getHeight()))
                {
                    if (GameCanvas.isPointerJustRelease && GameCanvas.isPointerClick)
                    {
                        isItemInfo = false;
                        isSpeacialSkill = false;
                        isBody = true;
                        currTab = BANG_HOI;
                        if (Char.myCharz().clan != null)
                        {
                            Service.gI().requestClan((short)Char.myCharz().clan.ID);
                            Service.gI().getClan(3, -1, null);
                        }
                        else
                        {
                            if (namesearch != null)
                            {
                                namesearch = null;
                            }
                            Service.gI().searchClan(namesearch != null ? namesearch : "");
                        }
                        SoundMn.gI().panelClick();
                        GameCanvas.clearAllPointerEvent();

                    }
                }
                if (GameCanvas.isPointerHoldIn(x4 + imgTab[0].getWidth() * 3 + 5, (y4 + imgTab[0].getHeight() / 2 + 5) - imgTab[0].getHeight() + 10, imgTab[0].getWidth(), imgTab[0].getHeight()))
                {

                    if (!Char.myCharz().havePet)
                    {
                        GameScr.info1.addInfo("Bạn chưa có đệ tử", 0);
                        return;
                    }

                    if (GameCanvas.isPointerJustRelease && GameCanvas.isPointerClick)
                    {
                        Service.gI().petInfo();
                        isItemInfo = false;
                        isBody = true;
                        currTabBody = 0;
                        if (scrollBag != null) scrollBag.cmtoY = 0;
                        currTab = DE_TU;
                        SoundMn.gI().panelClick();
                        GameCanvas.clearAllPointerEvent();
                    }
                }
                /*if (GameCanvas.isPointerHoldIn(x4 + imgTab[0].getWidth() * 4 + 5, (y4 + imgTab[0].getHeight() / 2 + 5) - imgTab[0].getHeight() + 10, imgTab[0].getWidth(), imgTab[0].getHeight()))
                {
                    if (GameCanvas.isPointerJustRelease && GameCanvas.isPointerClick)
                    {
                        isBody = false;
                        currTab = CHUC_NANG;
                        SoundMn.gI().panelClick();
                        GameCanvas.clearAllPointerEvent();
                    }
                }*/
                if (currTab == HANH_TRANG || currTab == THONG_TIN || currTab == DE_TU)
                    if (GameCanvas.isPointerHoldIn(x7 + w7 / 2 - 44, y7 + h7 / 2 - 35, left.getWidth() + 15, left.getHeight() + 5))
                    {
                        if (GameCanvas.isPointerJustRelease && GameCanvas.isPointerClick)
                        {
                            currTabBody = 0;
                            // SoundMn.gI().radarItem();
                            GameCanvas.clearAllPointerEvent();
                        }
                    }
                if (GameCanvas.isPointerHoldIn(x7 + w7 / 2 + 35, y7 + h7 / 2 - 35, left.getWidth() + 15, left.getHeight() + 5))
                {
                    if (GameCanvas.isPointerJustRelease && GameCanvas.isPointerClick)
                    {
                        currTabBody = 1;
                        // SoundMn.gI().radarItem();
                        GameCanvas.clearAllPointerEvent();
                    }
                }
                if (currTab == THONG_TIN && currIndex == 1)
                {
                    if (GameCanvas.isPointerHoldIn(x10 + 5, y10 + h10 - indexTabSkill[0].getHeight(), left.getWidth() + 15, left.getHeight() + 10))
                    {
                        if (GameCanvas.isPointerJustRelease && GameCanvas.isPointerClick)
                        {
                            isBody = true;
                            isSpeacialSkill = false;
                            if (currTabSkill == 0)
                            {
                                currTabSkill = maxTabSkill;
                            }
                            else
                            {
                                currTabSkill--;
                            }
                            currentSkill = null;
                            GameCanvas.clearAllPointerEvent();
                        }
                    }
                    if (GameCanvas.isPointerHoldIn(x10 + w10 - 10, y10 + h10 - indexTabSkill[0].getHeight(), left.getWidth() + 15, left.getHeight() + 10))
                    {
                        if (GameCanvas.isPointerJustRelease && GameCanvas.isPointerClick)
                        {
                            isBody = true;
                            isSpeacialSkill = false;
                            if (currTabSkill == maxTabSkill)
                            {
                                currTabSkill = 0;
                            }
                            else
                            {
                                currTabSkill++;
                            }
                            currentSkill = null;
                            GameCanvas.clearAllPointerEvent();
                        }
                    }

                }


                /*scrollBag.setStyle(numCo
                btnItem.getWidth() + 5, x10, y10 + btnItem.getHeight(), (btnItem.getWidth() + 5) * 6, h10, true, 6);*/
                if (cellsBag == null || cellsBag.Length != (numCol2 * numRow2))
                    cellsBag = new Cell[(numCol2 * numRow2)];

                if (currTab == HANH_TRANG
                || (currTab == THONG_TIN && currIndex == 0)
                || (currTab == DE_TU && currIndex == 0))
                {
                    g.setClip(x10, y10, w10, h10);
                    g.translate(0, -scrollBag.cmy);
                    // Xử lý vẽ item từ túi đồ
                    for (int row = 0; row < numRow2; row++)
                    {
                        for (int col = 0; col < numCol2; col++)
                        {
                            int x = x10 + 6 + (col * (btnItem.getWidth() + 5));
                            int y = y10 + 5 + (row * (btnItem.getHeight() + 5));
                            int index = row * numCol2 + col;
                            if (cellsBag[index] == null)
                            {
                                cellsBag[index] = new Cell(x, y);
                            }
                            cellsBag[index].paint(g, (byte)index);
                            Item[] items = (currIndex == 0) ? Char.myCharz().arrItemBag : Char.myCharz().arrItemBox;
                            if (items != null && index < items.Length && items[index] != null)
                            {
                                paintEffectItem(g, items[index], x - 2, y - 3);
                                SmallImage.drawSmallImage(g, items[index].template.iconID, x + btnItem.getWidth() / 2, y + btnItem.getHeight() / 2, 0, mGraphics.VCENTER | mGraphics.HCENTER);
                                if (items[index].quantity > 1)
                                {
                                    mFont.tahoma_7_yellow.drawString(g, string.Empty + items[index].quantity, x + btnItem.getWidth() / 2, y + btnItem.getHeight() / 2, 0);
                                }

                                if (GameCanvas.isPointerHoldIn(cellsBag[index].x, cellsBag[index].y - scrollBag.cmy, btnItem.getWidth(), btnItem.getHeight()))
                                {
                                    if (GameCanvas.isPointerJustRelease && GameCanvas.isPointerClick)
                                    {
                                        if (scrollInfo != null) scrollInfo.clear();
                                        isBody = false;
                                        itemDetails = items[index];
                                        isItemInfo = true;
                                        typePaint = (currIndex == 0) ? TypePaint.typeBag : TypePaint.typeBox;
                                        selected = (sbyte)index;
                                        InitScrollInfo();
                                        GameCanvas.clearAllPointerEvent();
                                    }
                                }
                            }
                        }
                    }

                }
                else
                {
                    g.reset();
                    if (currTab == DE_TU)
                    {

                        Skill[] petSkills = Char.myPetz().arrPetSkill;
                        int iconSize = 37;
                        int boxWidth = 137;
                        int boxHeight = 35;
                        int margin = 10;
                        g.setColor(0xd9c8b3);
                        g.fillRect(x10, y10, iconSize + margin, h10, 4);

                        for (int i = 0; i < petSkills.Length; i++)
                        {
                            int x = x10 + 6;
                            int y = y10 + i * (iconSize); // Khoảng cách giữa các hàng

                            if (x >= x10 && x <= x10 + w10 && y >= y10 && y <= y10 + h10)
                            {

                                g.drawImage(GameScr.imgSkill, x, y + 4, 0);
                                SmallImage.drawSmallImage(g, petSkills[i].template?.iconId ?? 544, x + 4, y + 7, 0, 0);
                                g.setColor(0xE6DED1); // Màu khung
                                g.fillRect(x + iconSize, y, boxWidth, boxHeight);
                                g.setColor(6116693); // Màu chữ
                                g.drawRect(x + iconSize, y, boxWidth, boxHeight);

                                string skillName = petSkills[i].template?.name ?? petSkills[i].moreInfo;
                                mFont.tahoma_7_blue.drawString(g, skillName, x + iconSize + 5, y + 3, 0);
                                // Vẽ thông tin kỹ năng dưới tên kỹ năng
                                string skillInfo = null;
                                if (petSkills[i].template != null)
                                {
                                    skillInfo = "Cấp: " + petSkills[i].point; // Thay đổi thông tin cần hiển thị ở đây
                                    mFont.tahoma_7_green2.drawString(g, skillInfo, x + iconSize + 5, y + 16, 0);
                                }
                            }
                        }
                    }
                    else if (currTab == THONG_TIN)
                    {
                        int num = Char.myCharz().nClass.skillTemplates.Length;
                        Skill[] petSkills = Char.myPetz().arrPetSkill;
                        int iconSize = 37;
                        int boxWidth = 148;
                        int boxHeight = 35;
                        int margin = 2;
                        //  g.fillRect(x10, y10, iconSize + margin, h10, 4);
                        //
                        /*g.setColor(0x87dcf3); 
                        g.fillRect(x10, y10 + (h10) - indexTabSkill[0].getHeight() - 2, w10, right.getHeight() + 3);*/
                        g.drawImage(right, x10 + w10 - 10, y10 + h10 - indexTabSkill[0].getHeight());
                        g.drawImage(left, x10 + 5, y10 + h10 - indexTabSkill[0].getHeight());
                        for (int i = 0; i < maxTabSkill + 1; i++)
                        {
                            g.drawImage(currTabSkill == i ? indexTabSkill[1] : indexTabSkill[0], x10 + (w10 / 3) + i * 20, y10 + h10 - indexTabSkill[0].getHeight() - 1);
                            mFont.tahoma_7_white.drawStringBd(g, "" + (i + 1), x10 + (w10 / 3) + i * 20 + 6, y10 + h10 + 2 - indexTabSkill[0].getHeight() - 1, 0, mFont.tahoma_7b_dark);
                            if (GameCanvas.isPointerHoldIn(x10 + (w10 / 3) + i * 20, y10 + h10 - indexTabSkill[0].getHeight() - 1, indexTabSkill[0].getWidth(), indexTabSkill[0].getHeight()))
                            {
                                if (GameCanvas.isPointerJustRelease && GameCanvas.isPointerClick)
                                {
                                    currTabSkill = i;
                                    GameCanvas.clearAllPointerEvent();
                                }
                            }
                        }

                        if (currTabSkill == 0)
                        {
                            for (int i = 0; i < 5; i++)
                            {
                                int x = x10;
                                int y = y10 + (i * 33);
                                // Vẽ icon kỹ năng                  
                                bool isfocus = GameCanvas.isPointerHoldIn(x + iconSize - 5, y + 1, boxWidth - 55, 28);
                                g.setColor(isfocus ? 16383818 : 0xdca76b);
                                g.fillRect(x + 1, y, 30, 32);

                                g.drawImage(GameScr.imgSkill, x + 2, y + 2, 0);

                                g.setColor(isfocus ? 16383818 : 0xE6DED1);
                                g.fillRect(x + iconSize - 5, y, boxWidth, 32);


                                if (isfocus)
                                {
                                    if (GameCanvas.isPointerJustRelease && GameCanvas.isPointerClick)
                                    {
                                        if (typeNumPoint < 2)
                                        {
                                            typeNumPoint++;
                                        }
                                        else
                                        {
                                            typeNumPoint = 0;
                                        }
                                        GameCanvas.clearAllPointerEvent();
                                        typePoint = i;
                                        tabSkills[0] = new TabSkill("+ " + PointPlus[typeNumPoint] * 20, this, 7, null);
                                        tabSkills[1] = new TabSkill("+ " + PointPlus[typeNumPoint], this, 8, null);
                                        tabSkills[2] = new TabSkill("+ " + PointPlus[0], this, 9, null);
                                    }
                                }

                                g.setColor((i != selected) ? 15196114 : 16383818);
                                if (i == 0)
                                {
                                    if (tabSkills[0] != null && typePoint == 0)
                                    {
                                        tabSkills[0].x = x + boxWidth;
                                        tabSkills[0].y = y;
                                        tabSkills[0].paint(g);
                                        if (tabSkills[0].Pressed())
                                        {
                                            tabSkills[0].actionPerform();
                                        }
                                    }
                                    SmallImage.drawSmallImage(g, 567, x + 6, y + 7, 0, 0);
                                    string st = mResources.HP + " " + mResources.root + ": " + NinjaUtil.getMoneys(Char.myCharz().cHPGoc);
                                    mFont.tahoma_7b_blue.drawString(g, st, x + iconSize + 5, y + 3, 0);
                                    mFont.tahoma_7_green2.drawString(g, NinjaUtil.getMoneys(Char.myCharz().cHPGoc + 1000) + " " + mResources.potential + ": " + mResources.increase + " " + Char.myCharz().hpFrom1000TiemNang, x + iconSize + 5, y + 15, 0);
                                }
                                if (i == 1)
                                {
                                    if (tabSkills[0] != null && typePoint == 1)
                                    {
                                        tabSkills[0].x = x + boxWidth;
                                        tabSkills[0].y = y;
                                        tabSkills[0].paint(g);
                                        if (tabSkills[0].Pressed())
                                        {
                                            tabSkills[0].actionPerform();
                                        }
                                    }
                                    SmallImage.drawSmallImage(g, 569, x + 6, y + 7, 0, 0);
                                    string st2 = mResources.KI + " " + mResources.root + ": " + NinjaUtil.getMoneys(Char.myCharz().cMPGoc);
                                    mFont.tahoma_7b_blue.drawString(g, st2, x + iconSize + 5, y + 3, 0);
                                    mFont.tahoma_7_green2.drawString(g, NinjaUtil.getMoneys(Char.myCharz().cMPGoc + 1000) + " " + mResources.potential + ": " + mResources.increase + " " + Char.myCharz().mpFrom1000TiemNang, x + iconSize + 5, y + 15, 0);
                                }
                                if (i == 2)
                                {
                                    if (tabSkills[1] != null && typePoint == 2)
                                    {
                                        tabSkills[1].x = x + boxWidth;
                                        tabSkills[1].y = y;
                                        tabSkills[1].paint(g);
                                        if (tabSkills[1].Pressed())
                                        {
                                            tabSkills[1].actionPerform();
                                        }
                                    }
                                    SmallImage.drawSmallImage(g, 568, x + 6, y + 7, 0, 0);
                                    string st3 = "SĐ" + " " + mResources.root + ": " + NinjaUtil.getMoneys(Char.myCharz().cDamGoc);
                                    mFont.tahoma_7b_blue.drawString(g, st3, x + iconSize + 5, y + 3, 0);
                                    mFont.tahoma_7_green2.drawString(g, NinjaUtil.getMoneys(Char.myCharz().cDamGoc * 100) + " " + mResources.potential + ": " + mResources.increase + " " + Char.myCharz().damFrom1000TiemNang, x + iconSize + 5, y + 15, 0);
                                }
                                if (i == 3)
                                {
                                    if (tabSkills[2] != null && typePoint == 3)
                                    {
                                        tabSkills[2].x = x + boxWidth;
                                        tabSkills[2].y = y;
                                        tabSkills[2].paint(g);
                                        if (tabSkills[2].Pressed())
                                        {
                                            tabSkills[2].actionPerform();
                                        }
                                    }
                                    SmallImage.drawSmallImage(g, 721, x + 6, y + 7, 0, 0);
                                    string st4 = mResources.armor + " " + mResources.root + ": " + NinjaUtil.getMoneys(Char.myCharz().cDefGoc);
                                    mFont.tahoma_7b_blue.drawString(g, st4, x + iconSize + 5, y + 3, 0);
                                    mFont.tahoma_7_green2.drawString(g, NinjaUtil.getMoneys(500000 + Char.myCharz().cDefGoc * 100000) + " " + mResources.potential + ": " + mResources.increase + " " + Char.myCharz().defFrom1000TiemNang, x + iconSize + 5, y + 15, 0);
                                }
                                if (i == 4)
                                {
                                    if (tabSkills[2] != null && typePoint == 4)
                                    {
                                        tabSkills[2].x = x + boxWidth;
                                        tabSkills[2].y = y;
                                        tabSkills[2].paint(g);
                                        if (tabSkills[2].Pressed())
                                        {
                                            tabSkills[2].actionPerform();
                                        }
                                    }
                                    SmallImage.drawSmallImage(g, 719, x + 6, y + 7, 0, 0);
                                    string st5 = mResources.critical + " " + mResources.root + ": " + Char.myCharz().cCriticalGoc + "%";
                                    long num9 = 50000000L;
                                    int num10 = Char.myCharz().cCriticalGoc;
                                    if (num10 > Panel.t_tiemnang.Length - 1)
                                    {
                                        num10 = Panel.t_tiemnang.Length - 1;
                                    }
                                    num9 = Panel.t_tiemnang[num10];
                                    mFont.tahoma_7b_blue.drawString(g, st5, x + iconSize + 5, y + 3, 0);
                                    long number = num9;
                                    mFont.tahoma_7_green2.drawString(g, Res.formatNumber2(number) + " " + mResources.potential + ": " + mResources.increase + " " + Char.myCharz().criticalFrom1000Tiemnang, x + iconSize + 5, y + 15, 0);
                                }
                                if (i == 5)
                                {

                                    if (Panel.specialInfo != null)
                                    {
                                        SmallImage.drawSmallImage(g, Panel.spearcialImage, x + 6, y + 7, 0, 0);
                                        string[] array = mFont.tahoma_7.splitFontArray(Panel.specialInfo, 120);
                                        for (int j = 0; j < array.Length; j++)
                                        {
                                            mFont.tahoma_7_green2.drawString(g, array[j], x + iconSize + 5, y + 3 + j * 12, 0);
                                        }
                                    }
                                    else
                                    {
                                        mFont.tahoma_7_green2.drawString(g, string.Empty, x + iconSize + 5, y + 9, 0);
                                    }
                                }
                                if (i < 6)
                                {
                                    continue;
                                }

                            }
                        }
                        else if (currTabSkill == 1)
                        {
                            for (int i = 0; i < 5; i++)
                            {
                                int x = x10;
                                int y = y10 + (i * 33);
                                // Vẽ icon kỹ năng                  
                                bool isfocus = GameCanvas.isPointerHoldIn(x + iconSize - 5, y + 1, boxWidth - 55, 28);
                                g.setColor(isfocus ? 16383818 : 0xdca76b);
                                g.fillRect(x + 1, y, 30, 32);

                                g.drawImage(GameScr.imgSkill, x + 2, y + 2, 0);

                                g.setColor(isfocus ? 16383818 : 0xE6DED1);
                                g.fillRect(x + iconSize - 5, y, boxWidth, 32);
                                if (i == 0 && isfocus)
                                {
                                    if (GameCanvas.isPointerJustRelease && GameCanvas.isPointerClick)
                                    {
                                        Service.gI().speacialSkill(0);
                                        GameCanvas.clearAllPointerEvent();
                                    }
                                }
                                if (i == 0)
                                {

                                    if (Panel.specialInfo != null)
                                    {
                                        SmallImage.drawSmallImage(g, Panel.spearcialImage, x + 6, y + 7, 0, 0);
                                        string[] array = mFont.tahoma_7.splitFontArray(Panel.specialInfo, 120);
                                        for (int j = 0; j < array.Length; j++)
                                        {
                                            mFont.tahoma_7_green2.drawString(g, array[j], x + iconSize + 5, y + 5 + j * 12, 0);
                                        }
                                    }
                                    else
                                    {
                                        mFont.tahoma_7_green2.drawString(g, string.Empty, x + iconSize + 5, y + 11, 0);
                                    }
                                }
                                if (i < 1)
                                {
                                    continue;
                                }
                                int num11 = i - 1;
                                // do skill
                                SkillTemplate skillTemplate = Char.myCharz().nClass.skillTemplates[num11];
                                SmallImage.drawSmallImage(g, skillTemplate.iconId, x + 6, y + 7, 0, 0);
                                Skill skill = Char.myCharz().getSkill(skillTemplate);
                                Skill skill01 = null;
                                if (isfocus)
                                {
                                    if (GameCanvas.isPointerJustRelease && GameCanvas.isPointerClick && !GameCanvas.menu.showMenu)
                                    {
                                        if (skill != null)
                                        {
                                            if (skill.point < skillTemplate.maxPoint)
                                            {
                                                skill01 = skillTemplate.skills[skill.point];
                                            }

                                        }
                                        else
                                        {
                                            skill01 = skillTemplate.skills[0];
                                        }
                                        g.reset();
                                        if (currentSkill != skillTemplate)
                                            if (currentSkill != skillTemplate)
                                            {
                                                currentSkill = skillTemplate;
                                            }
                                            else
                                            {
                                                currentSkill = null;
                                            }
                                    }
                                }
                                if (skill != null)
                                {
                                    mFont.tahoma_7b_blue.drawString(g, skillTemplate.name, x + iconSize + 5, y + 5, 0);
                                    mFont.tahoma_7_blue.drawString(g, mResources.level + ": " + skill.point, x + boxWidth + 10, y + 5, 0);
                                    if (skill.point == skillTemplate.maxPoint)
                                    {
                                        mFont.tahoma_7_green2.drawString(g, mResources.max_level_reach, x + iconSize + 5, y + 17, 0);
                                    }
                                    else if (skill.template.isSkillSpec())
                                    {
                                        string text = mResources.proficiency + ": ";
                                        int xv = mFont.tahoma_7_green2.getWidthExactOf(text) + x + iconSize + 5;
                                        int num12 = y + 17;
                                        mFont.tahoma_7_green2.drawString(g, text, x + iconSize + 5, num12, 0);
                                        mFont.tahoma_7_green2.drawString(g, "(" + skill.strCurExp() + ")", x + boxWidth + 30, num12, mFont.RIGHT);
                                        num12 += 6;
                                        g.setColor(7169134);
                                        g.fillRect(xv, num12, 50, 5);
                                        int num13 = skill.curExp * 50 / 1000;
                                        g.setColor(11992374);
                                        g.fillRect(xv, num12, num13, 5);
                                        if (skill.curExp < 1000)
                                        {
                                        }
                                    }
                                    else
                                    {
                                        Skill skill2 = skillTemplate.skills[skill.point];
                                        mFont.tahoma_7_green2.drawString(g, mResources.level + " " + (skill.point + 1) + " " + mResources.need + " " + Res.formatNumber2(skill2.powRequire) + " " + mResources.potential, x + iconSize + 5, y + 17, 0);
                                    }
                                }
                                else
                                {
                                    Skill skill3 = skillTemplate.skills[0];
                                    mFont.tahoma_7b_green.drawString(g, skillTemplate.name, x + iconSize + 5, y + 5, 0);
                                    mFont.tahoma_7_green2.drawString(g, mResources.need_upper + " " + Res.formatNumber2(skill3.powRequire) + " " + mResources.potential_to_learn, x + iconSize + 5, y + 17, 0);
                                }
                            }
                        }
                        else if (currTabSkill == 2)
                        {
                            for (int i = 0; i < 5; i++)
                            {
                                int x = x10;
                                int y = y10 + (i * 33);
                                int num11 = i - 1;
                                int skillIndex = num11 + 5;
                                if (skillIndex < 0 || skillIndex >= Char.myCharz().nClass.skillTemplates.Length)
                                {
                                    continue;
                                }
                                // Vẽ icon kỹ năng                  
                                bool isfocus = GameCanvas.isPointerHoldIn(x + iconSize - 5, y + 1, boxWidth - 55, 28);
                                g.setColor(isfocus ? 16383818 : 0xdca76b);
                                g.fillRect(x + 1, y, 30, 32);

                                g.drawImage(GameScr.imgSkill, x + 2, y + 2, 0);

                                g.setColor(isfocus ? 16383818 : 0xE6DED1);
                                g.fillRect(x + iconSize - 5, y, boxWidth, 32);
                                // Vẽ icon kỹ năng
                                SkillTemplate skillTemplate = Char.myCharz().nClass.skillTemplates[num11 + 5];
                                SmallImage.drawSmallImage(g, skillTemplate.iconId, x + 6, y + 7, 0, 0);
                                Skill skill = Char.myCharz().getSkill(skillTemplate);
                                Skill skill01 = null;
                                if (isfocus)
                                {
                                    if (GameCanvas.isPointerJustRelease && GameCanvas.isPointerClick && !GameCanvas.menu.showMenu)
                                    {
                                        if (skill != null)
                                        {
                                            if (skill.point < skillTemplate.maxPoint)
                                            {
                                                skill01 = skillTemplate.skills[skill.point];
                                            }

                                        }
                                        else
                                        {
                                            skill01 = skillTemplate.skills[0];
                                        }
                                        g.reset();
                                        if (currentSkill != skillTemplate)
                                        {
                                            currentSkill = skillTemplate;
                                        }
                                        else
                                        {
                                            currentSkill = null;
                                        }
                                        GameCanvas.clearAllPointerEvent();
                                    }
                                }
                                if (skill != null)
                                {
                                    mFont.tahoma_7b_blue.drawString(g, skillTemplate.name, x + iconSize + 5, y + 5, 0);
                                    mFont.tahoma_7_blue.drawString(g, mResources.level + ": " + skill.point, x + boxWidth + 10, y + 5, 0);
                                    if (skill.point == skillTemplate.maxPoint)
                                    {
                                        mFont.tahoma_7_green2.drawString(g, mResources.max_level_reach, x + iconSize + 5, y + 17, 0);
                                    }
                                    else if (skill.template.isSkillSpec())
                                    {
                                        string text = mResources.proficiency + ": ";
                                        int xv = mFont.tahoma_7_green2.getWidthExactOf(text) + x + iconSize + 5;
                                        int num12 = y + 17;
                                        mFont.tahoma_7_green2.drawString(g, text, x + iconSize + 5, num12, 0);
                                        mFont.tahoma_7_green2.drawString(g, "(" + skill.strCurExp() + ")", x + boxWidth + 30, num12, mFont.RIGHT);
                                        num12 += 6;
                                        g.setColor(7169134);
                                        g.fillRect(xv, num12, 50, 5);
                                        int num13 = skill.curExp * 50 / 1000;
                                        g.setColor(11992374);
                                        g.fillRect(xv, num12, num13, 5);
                                        if (skill.curExp < 1000)
                                        {
                                        }
                                    }
                                    else
                                    {
                                        Skill skill2 = skillTemplate.skills[skill.point];
                                        mFont.tahoma_7_green2.drawString(g, mResources.level + " " + (skill.point + 1) + " " + mResources.need + " " + Res.formatNumber2(skill2.powRequire) + " " + mResources.potential, x + iconSize + 5, y + 17, 0);
                                    }
                                }
                                else
                                {
                                    Skill skill3 = skillTemplate.skills[0];
                                    mFont.tahoma_7b_green.drawString(g, skillTemplate.name, x + iconSize + 5, y + 5, 0);
                                    mFont.tahoma_7_green2.drawString(g, mResources.need_upper + " " + Res.formatNumber2(skill3.powRequire) + " " + mResources.potential_to_learn, x + iconSize + 5, y + 17, 0);
                                }
                            }
                        }
                        else if (currTabSkill == 3)
                        {
                            for (int i = 0; i < 5; i++)
                            {
                                int num11 = i - 1;
                                int skillIndex = num11 + 10;
                                if (skillIndex < 0 || skillIndex >= Char.myCharz().nClass.skillTemplates.Length)
                                {
                                    continue;
                                }
                                int x = x10;
                                int y = y10 + (i * 33);
                                // Vẽ icon kỹ năng                  
                                bool isfocus = GameCanvas.isPointerHoldIn(x + iconSize - 5, y + 1, boxWidth - 55, 28);
                                g.setColor(isfocus ? 16383818 : 0xdca76b);
                                g.fillRect(x + 1, y, 30, 32);

                                g.drawImage(GameScr.imgSkill, x + 2, y + 2, 0);

                                g.setColor(isfocus ? 16383818 : 0xE6DED1);
                                g.fillRect(x + iconSize - 5, y, boxWidth, 32);

                                SkillTemplate skillTemplate = Char.myCharz().nClass.skillTemplates[skillIndex];
                                SmallImage.drawSmallImage(g, skillTemplate.iconId, x + 6, y + 7, 0, 0);
                                Skill skill = Char.myCharz().getSkill(skillTemplate);
                                Skill skill01 = null;
                                if (isfocus)
                                {
                                    if (GameCanvas.isPointerJustRelease && GameCanvas.isPointerClick && !GameCanvas.menu.showMenu)
                                    {
                                        if (skill != null)
                                        {
                                            if (skill.point < skillTemplate.maxPoint)
                                            {
                                                skill01 = skillTemplate.skills[skill.point];
                                            }

                                        }
                                        else
                                        {
                                            skill01 = skillTemplate.skills[0];
                                        }
                                        g.reset();
                                        if (currentSkill != skillTemplate)
                                        {
                                            currentSkill = skillTemplate;
                                        }
                                        else
                                        {
                                            currentSkill = null;
                                        }
                                        GameCanvas.clearAllPointerEvent();
                                    }
                                }
                                if (skill != null)
                                {
                                    mFont.tahoma_7b_blue.drawString(g, skillTemplate.name, x + iconSize + 5, y + 5, 0);
                                    mFont.tahoma_7_blue.drawString(g, mResources.level + ": " + skill.point, x + boxWidth + 10, y + 5, 0);
                                    if (skill.point == skillTemplate.maxPoint)
                                    {
                                        mFont.tahoma_7_green2.drawString(g, mResources.max_level_reach, x + iconSize + 5, y + 17, 0);
                                    }
                                    else if (skill.template.isSkillSpec())
                                    {
                                        string text = mResources.proficiency + ": ";
                                        int xv = mFont.tahoma_7_green2.getWidthExactOf(text) + x + iconSize + 5;
                                        int num12 = y + 17;
                                        mFont.tahoma_7_green2.drawString(g, text, x + iconSize + 5, num12, 0);
                                        mFont.tahoma_7_green2.drawString(g, "(" + skill.strCurExp() + ")", x + boxWidth + 30, num12, mFont.RIGHT);
                                        num12 += 6;
                                        g.setColor(7169134);
                                        g.fillRect(xv, num12, 50, 5);
                                        int num13 = skill.curExp * 50 / 1000;
                                        g.setColor(11992374);
                                        g.fillRect(xv, num12, num13, 5);
                                        if (skill.curExp < 1000)
                                        {
                                        }
                                    }
                                    else
                                    {
                                        Skill skill2 = skillTemplate.skills[skill.point];
                                        mFont.tahoma_7_green2.drawString(g, mResources.level + " " + (skill.point + 1) + " " + mResources.need + " " + Res.formatNumber2(skill2.powRequire) + " " + mResources.potential, x + iconSize + 5, y + 17, 0);
                                    }
                                }
                                else
                                {
                                    Skill skill3 = skillTemplate.skills[0];
                                    mFont.tahoma_7b_green.drawString(g, skillTemplate.name, x + iconSize + 5, y + 5, 0);
                                    mFont.tahoma_7_green2.drawString(g, mResources.need_upper + " " + Res.formatNumber2(skill3.powRequire) + " " + mResources.potential_to_learn, x + iconSize + 5, y + 17, 0);
                                }
                            }
                        }
                    }
                }


                g.reset();
                paintItemDetails(g, x7, y7, w7, h7);
                if (currTabSkill == 1)
                {

                    paintSpeacialSkill(g, x7, y7, w7, h7);


                }
                if (currentSkill != null)
                {
                    Skill skill = Char.myCharz().getSkill(currentSkill);
                    Skill skill01 = null;
                    if (skill != null)
                    {
                        if (skill.point < currentSkill.maxPoint)
                        {
                            skill01 = currentSkill.skills[skill.point];
                        }

                    }
                    else
                    {
                        skill01 = currentSkill.skills[0];
                    }

                    addSkillDetail(g, currentSkill, skill, skill01, x7, y7, w7, h7);
                }
                #endregion
                if (!isSpeacialSkill)
                {
                    Babyshark.render(g, X + 40, Y + H - 40, W - 80);
                }
            }
            catch (Exception e)
            {
                Res.err(e.ToString());
            }
        }



        public void onChatFromMe(string text, string to)
        {

            if (ChatTextField.gI().strChat.Equals(mResources.chat_clan))
            {


                InfoDlg.showWait();
                chatTField.isShow = false;
                Service.gI().clanMessage(0, text, -1);
                return;
            }
            if (chatTField.strChat.Equals(mResources.input_clan_slogan))
            {
                if (chatTField.tfChat.getText() == string.Empty)
                {
                    GameScr.info1.addInfo(mResources.clan_slogan_blank, 0);
                    return;
                }
                Service.gI().getClan(4, (sbyte)Char.myCharz().clan.imgID, chatTField.tfChat.getText());
                chatTField.isShow = false;
                return;
            }
            if (chatTField.strChat.Equals(mResources.input_clan_name_to_create))
            {
                if (chatTField.tfChat.getText() == string.Empty)
                {
                    GameScr.info1.addInfo(mResources.clan_name_blank, 0);
                    return;
                }
                if (Bag.gI().tabIcon == null)
                {
                    Bag.gI().tabIcon = new TabClanIcon();
                }
                Bag.gI().tabIcon.text = chatTField.tfChat.getText();
                Bag.gI().tabIcon.show(false);
                return;
            }
            if (chatTField.strChat.Equals(mResources.input_clan_name))
            {
                InfoDlg.showWait();
                chatTField.isShow = false;
                Bag.gI().isShow = true;
                Bag.gI().namesearch = chatTField.tfChat.getText();
                Service.gI().searchClan(Bag.gI().namesearch);
                return;
            }
        }
        public void onCancelChat()
        {
            throw new System.NotImplementedException();
        }
        public void paintPopupSticker(mGraphics g, int x, int y, int w, int h, int x1, int y1, int w1, int h1)
        {
            try
            {
                if (currTab == BANG_HOI && isShowchatClan && isPaintStickers)
                {
                    PopUp.paintPopUp(g, x, y, w, h, 16777215, false);
                    isItemInfo = false;
                    scrollSticker.setStyle(sticker.Length, sticker[0].getWidth() + 10, x, y, w, h, true, 2);
                    g.setClip(x, y, w, h);
                    g.translate(scrollSticker.cmx, -scrollSticker.cmy);

                    int stickerWidth = sticker[0].getWidth() + 5;  // Khoảng cách giữa các sticker
                    int stickerHeight = sticker[0].getHeight() + 10; // Khoảng cách giữa các hàng

                    for (int j = 0; j < sticker.Length; j++)
                    {
                        // Tính chỉ số hàng và cột
                        int row = j / 2;
                        int col = j % 2;

                        // Tính tọa độ mới cho từng sticker
                        int xnew = x + 5 + col * stickerWidth;
                        int ynew = y + row * stickerHeight;

                        // Kiểm tra nếu sticker được chọn (focus)
                        bool isfocus = GameCanvas.isPointerHoldIn(
                            xnew, ynew - scrollSticker.cmy,
                            stickerWidth, stickerHeight
                        );
                        bool isPointBar = GameCanvas.isPointerHoldIn(
                            x1, y1, w1, h1
                        );

                        if (isfocus && !isPointBar)
                        {
                            g.setColor(16383818);
                            g.fillRect(xnew + 3, ynew, stickerWidth - 10, stickerHeight - 6, 4);

                            if (GameCanvas.isPointerJustRelease && GameCanvas.isPointerClick)
                            {
                                Service.gI().clanMessage(0, "Gửi Sticker " + j, -1);
                            }
                        }

                        // Vẽ sticker
                        g.drawImage(sticker[j], xnew, ynew);
                    }
                }
            }
            catch (Exception e)
            {
                Res.err(e.ToString());
            }
        }
        public void paintPopupInfoMember(mGraphics g, Member m, int x, int y, int w, int h)
        {
            try
            {
                if (currTab == BANG_HOI && isShowMemClan)
                {
                    isShowInfomem = true;
                    int hnew = h;
                    int[] ints = new int[3] { -1, -1, -1 };
                    if (Char.myCharz().role < 2 && Char.myCharz().charID != currmember.ID)
                    {
                        if (currmember.role == 2)
                        {
                            ints[1] = 1;
                        }
                        if (Char.myCharz().role == 0)
                        {
                            ints[0] = 0;
                            if (currmember.role == 1)
                            {
                                ints[1] = 3;
                            }
                        }
                    }
                    if (Char.myCharz().role < currmember.role)
                    {
                        ints[2] = 2;
                    }
                    if (ints[0] == -1 && ints[1] == -1 && ints[2] == -1)
                    {

                        hnew = h - buttonmem[0].getHeight();
                    }
                    PopUp.paintPopUp(g, x, y, w, hnew, 16777215, false);

                    string text = "|0|" + m.name;
                    string text2 = "\n|2|";
                    if (m.role == 0)
                    {
                        text2 = "\n|7|";
                    }
                    if (m.role == 1)
                    {
                        text2 = "\n|1|";
                    }
                    if (m.role == 2)
                    {
                        text2 = "\n|0|";
                    }
                    text = text + text2 + Member.getRole(m.role);
                    string text3 = text;
                    text = text3 + "\n|2|" + mResources.power + ": " + m.powerPoint;
                    text += "\n--";
                    text3 = text;
                    text = text3 + "\n|5|" + mResources.clan_capsuledonate + ": " + m.clanPoint;
                    text3 = text;
                    text = text3 + "\n|5|" + mResources.clan_capsuleself + ": " + m.curClanPoint;
                    text3 = text;
                    text = text3 + "\n|4|" + mResources.give_pea + ": " + m.donate + mResources.time;
                    text3 = text;
                    text = text3 + "\n|4|" + mResources.receive_pea + ": " + m.receive_donate + mResources.time;
                    text3 = text;
                    text = text3 + "\n|6|" + mResources.join_date + ": " + m.joinTime;
                    string[] array = mFont.tahoma_7b_blue.splitFontArray(text, w - 10);
                    int num5 = -1;
                    for (int i = 0; i < array.Length; i++)
                    {
                        if (array[i].StartsWith("--"))
                        {
                            g.setColor(0);
                            g.fillRect(x + 10, y + 5 + i * 12 + 6, w - 20, 1);
                            continue;
                        }
                        mFont mFont2 = mFont.tahoma_7;
                        int num6 = 2;
                        string st = array[i];
                        int num7 = 0;
                        if (array[i].StartsWith("|"))
                        {
                            string[] array1 = Res.split(array[i], "|", 0);
                            if (array1.Length == 3)
                            {
                                st = array1[2];
                            }
                            if (array1.Length == 4)
                            {
                                st = array1[3];
                                num6 = int.Parse(array1[2]);
                            }
                            num7 = int.Parse(array1[1]);
                            num5 = num7;
                        }
                        else
                        {
                            num7 = num5;
                        }
                        switch (num7)
                        {
                            case -1:
                                mFont2 = mFont.tahoma_7;
                                break;
                            case 0:
                                mFont2 = mFont.tahoma_7b_dark;
                                break;
                            case 1:
                                mFont2 = mFont.tahoma_7b_green;
                                break;
                            case 2:
                                mFont2 = mFont.tahoma_7b_blue;
                                break;
                            case 3:
                                mFont2 = mFont.tahoma_7_red;
                                break;
                            case 4:
                                mFont2 = mFont.tahoma_7_green;
                                break;
                            case 5:
                                mFont2 = mFont.tahoma_7_blue;
                                break;
                            case 7:
                                mFont2 = mFont.tahoma_7b_red;
                                break;
                            case 8:
                                mFont2 = mFont.tahoma_7b_yellow;
                                break;
                        }
                        if (num6 == 2)
                        {
                            mFont2.drawString(g, st, x + (w / 2), y + ((i + 1) * 10), num6);
                        }
                        if (num6 == 1)
                        {
                            mFont2.drawString(g, st, x + (w / 2), y + ((i + 1) * 10), num6);
                        }
                    }
                    paintLeader(g, m, x + w - 30, y + 40);
                    g.drawImage(imgX, x + w - imgX.getWidth(), y, 0);
                    if (Char.myCharz().clan != null)
                    {
                        int xbutton = x - buttonmem[0].getWidth() + 6;
                        int space = 10;
                        if (ints.Contains(-1))
                        {
                            xbutton = x - (buttonmem[0].getWidth() * 2) - 3;
                            space = 20;
                        }
                        for (int i = 0; i < ints.Length; i++)
                        {
                            if (ints[i] != -1)
                            {
                                bool selected = GameCanvas.isPointerHoldIn(xbutton + ((i + 1) * (buttonmem[ints[i]].getWidth() + space)), y + h - buttonmem[ints[i]].getHeight() - 5, buttonmem[ints[i]].getWidth(), buttonmem[ints[i]].getHeight());
                                if (selected)
                                {
                                    if (GameCanvas.isPointerJustRelease && GameCanvas.isPointerClick)
                                    {
                                        if (ints[i] == 0)
                                        {
                                            Service.gI().clanRemote(currmember.ID, 0);

                                        }
                                        if (ints[i] == 1)
                                        {
                                            Service.gI().clanRemote(currmember.ID, 1);

                                        }
                                        if (ints[i] == 2)
                                        {
                                            Service.gI().clanRemote(currmember.ID, -1);

                                        }
                                        if (ints[i] == 3)
                                        {
                                            Service.gI().clanRemote(currmember.ID, 2);

                                        }
                                        currmember = null;
                                        isShowInfomem = false;
                                    }
                                }
                                g.setColor(0x000000);
                                g.fillRect(xbutton + ((i + 1) * (buttonmem[ints[i]].getWidth() + space)) - 2, y + h - buttonmem[ints[i]].getHeight() - 7, buttonmem[ints[i]].getWidth() + 4, buttonmem[ints[i]].getHeight() + 4, 4);
                                g.setColor(selected ? 16383818 : 0x2596be);
                                g.fillRect(xbutton + ((i + 1) * (buttonmem[ints[i]].getWidth() + space)), y + h - buttonmem[ints[i]].getHeight() - 5 - 2, buttonmem[ints[i]].getWidth(), buttonmem[ints[i]].getHeight(), 4);
                                g.drawImage(buttonmem[ints[i]], xbutton + ((i + 1) * (buttonmem[ints[i]].getWidth() + space)), y + h - buttonmem[ints[i]].getHeight() - 5, mGraphics.LEFT);
                            }

                        }

                    }
                    if (GameCanvas.isPointerHoldIn(x + w - imgX.getWidth(), y, imgX.getWidth(), imgX.getHeight()))
                    {
                        if (GameCanvas.isPointerJustRelease && GameCanvas.isPointerClick)
                        {
                            currmember = null;
                            isShowInfomem = false;
                            SoundMn.gI().buttonClose();
                        }
                    }

                }
            }
            catch (Exception e)
            {
                Res.err(e.ToString());
            }
        }
        public void paintPopupInfoClan(mGraphics g, Clan clan, int x, int y, int w, int h)
        {
            try
            {
                if (currTab == BANG_HOI && Char.myCharz().clan == null)
                {
                    g.reset();
                    isShowInfomem = true;
                    int hnew = h;
                    PopUp.paintPopUp(g, x, y, w, hnew, 16777215, false);
                    string text = "|0|" + clan.name;
                    if (clan.slogan != string.Empty)
                    {
                        string[] array11 = mFont.tahoma_7_green.splitFontArray(clan.slogan, w - 25);
                        text = text + "\n|2|" + array11[0];
                        if (array11.Length > 1)
                        {
                            text = text + "...";
                        }

                    }
                    text += "\n--";
                    string text2 = text;
                    text = text2 + "\n|7|" + mResources.clan_leader + ": " + clan.leaderName;
                    text2 = text;
                    text = text2 + "\n|1|" + mResources.power_point + ": " + clan.powerPoint;
                    text2 = text;
                    text = text2 + "\n|4|" + mResources.member + ": " + clan.currMember + "/" + clan.maxMember;
                    text2 = text;
                    text = text2 + "\n|4|" + mResources.level + ": " + clan.level;
                    text2 = text;
                    text = text2 + "\n|4|" + mResources.clan_birthday + "" + NinjaUtil.getDate(clan.date);
                    string[] array = mFont.tahoma_7b_blue.splitFontArray(text, w - 10);
                    int num5 = -1;
                    for (int i = 0; i < array.Length; i++)
                    {
                        if (array[i].StartsWith("--"))
                        {
                            g.setColor(0);
                            g.fillRect(x + 10, y + 5 + i * 12 + 6, w - 20, 1);
                            continue;
                        }
                        mFont mFont2 = mFont.tahoma_7;
                        int num6 = 2;
                        string st = array[i];
                        int num7 = 0;
                        if (array[i].StartsWith("|"))
                        {
                            string[] array1 = Res.split(array[i], "|", 0);
                            if (array1.Length == 3)
                            {
                                st = array1[2];
                            }
                            if (array1.Length == 4)
                            {
                                st = array1[3];
                                num6 = int.Parse(array1[2]);
                            }
                            num7 = int.Parse(array1[1]);
                            num5 = num7;
                        }
                        else
                        {
                            num7 = num5;
                        }
                        switch (num7)
                        {
                            case -1:
                                mFont2 = mFont.tahoma_7;
                                break;
                            case 0:
                                mFont2 = mFont.tahoma_7b_dark;
                                break;
                            case 1:
                                mFont2 = mFont.tahoma_7b_green;
                                break;
                            case 2:
                                mFont2 = mFont.tahoma_7b_blue;
                                break;
                            case 3:
                                mFont2 = mFont.tahoma_7_red;
                                break;
                            case 4:
                                mFont2 = mFont.tahoma_7_green;
                                break;
                            case 5:
                                mFont2 = mFont.tahoma_7_blue;
                                break;
                            case 7:
                                mFont2 = mFont.tahoma_7b_red;
                                break;
                            case 8:
                                mFont2 = mFont.tahoma_7b_yellow;
                                break;
                        }
                        if (num6 == 2)
                        {
                            mFont2.drawString(g, st, x + (w / 2), y + ((i + 1) * 10), num6);
                        }
                        if (num6 == 1)
                        {
                            mFont2.drawString(g, st, x + (w / 2), y + ((i + 1) * 10), num6);
                        }
                    }
                    g.drawImage(imgX, x + w - imgX.getWidth(), y, 0);
                    if (ClanImage.getClanImage((short)clan.imgID).idImage != null)
                    {
                        SmallImage.drawSmallImage(g, ClanImage.getClanImage((short)clan.imgID).idImage[0], x + 20, y + 10, 0, StaticObj.VCENTER_HCENTER);
                    }
                    simplebutton(g, "Xin vào", x + 10, y + h - 40, 50, 30, 0xefefb2, 0x967b55, 5);
                    simplebutton(g, "Xem thành\nviên", x + (w / 2) + 20, y + h - 40, 50, 30, 0xefefb2, 0x967b55, 6);

                    if (GameCanvas.isPointerHoldIn(x + w - imgX.getWidth(), y, imgX.getWidth(), imgX.getHeight()))
                    {
                        if (GameCanvas.isPointerJustRelease && GameCanvas.isPointerClick)
                        {
                            currclan = null;
                            isShowInfomem = false;
                            SoundMn.gI().buttonClose();
                        }
                    }
                }
            }
            catch (Exception e)
            {
                Res.err(e.ToString());
            }
        }
        public void paintPopupInfoSkill(mGraphics g, String text, int x, int y, int w, int h)
        {
            try
            {
                if (currTab == THONG_TIN)
                {
                    g.reset();
                    PopUp.paintPopUp(g, x, y, w, h, 16777215, false);
                    isItemInfo = false;
                    string[] array = mFont.tahoma_7b_blue.splitFontArray(text, w - 10);

                    g.setClip(x, y, w, h);
                    if (array.Length >= 15)
                    {
                        scrollInfoSkill.setStyle(array.Length + 5, mFont.tahoma_7b_blue.getHeight() + 5, x, y, w, h, true, 1);
                        g.translate(scrollInfoSkill.cmx, -scrollInfoSkill.cmy);
                    }

                    int num5 = -1;
                    for (int i = 0; i < array.Length; i++)
                    {
                        if (array[i].StartsWith("--"))
                        {
                            g.setColor(0);
                            g.fillRect(x + 10, y + 5 + i * 12 + 6, w - 20, 1);
                            continue;
                        }
                        mFont mFont2 = mFont.tahoma_7;
                        int num6 = 2;
                        string st = array[i];
                        int num7 = 0;
                        if (array[i].StartsWith("|"))
                        {
                            string[] array1 = Res.split(array[i], "|", 0);
                            if (array1.Length == 3)
                            {
                                st = array1[2];
                            }
                            if (array1.Length == 4)
                            {
                                st = array1[3];
                                num6 = int.Parse(array1[2]);
                            }
                            num7 = int.Parse(array1[1]);
                            num5 = num7;
                        }
                        else
                        {
                            num7 = num5;
                        }
                        switch (num7)
                        {
                            case -1:
                                mFont2 = mFont.tahoma_7;
                                break;
                            case 0:
                                mFont2 = mFont.tahoma_7b_dark;
                                break;
                            case 1:
                                mFont2 = mFont.tahoma_7b_green;
                                break;
                            case 2:
                                mFont2 = mFont.tahoma_7b_blue;
                                break;
                            case 3:
                                mFont2 = mFont.tahoma_7_red;
                                break;
                            case 4:
                                mFont2 = mFont.tahoma_7_green;
                                break;
                            case 5:
                                mFont2 = mFont.tahoma_7_blue;
                                break;
                            case 7:
                                mFont2 = mFont.tahoma_7b_red;
                                break;
                            case 8:
                                mFont2 = mFont.tahoma_7b_yellow;
                                break;
                        }
                        if (num6 == 2)
                        {
                            mFont2.drawString(g, st, x + (w / 2), y + ((i + 1) * 10), num6);
                        }
                        if (num6 == 1)
                        {
                            mFont2.drawString(g, st, x + (w / 2), y + ((i + 1) * 10), num6);
                        }
                    }
                    int ySlotSkill = y + (array.Length * 10) + 20;
                    if (Char.myCharz().getSkill(currentSkill) != null)
                    {
                        for (int i = 0; i < 10; i++)
                        {
                            int row = i / 5; // Xác định hàng (0 hoặc 1)
                            int col = i % 5; // Xác định cột (0 -> 4)

                            int xPos = (x + 50) + (col * 20); // Tọa độ x theo cột
                            int yPos = ySlotSkill + (row * 20); // Tọa độ y theo hàng
                            bool iskey = false;
                            Skill skill3 = Char.myCharz().getSkill(currentSkill);
                            for (int j = 0; j < GameScr.keySkill.Length; j++)
                            {
                                if (GameScr.keySkill[j] == skill3)
                                {
                                    if (j == i)
                                    {
                                        iskey = true;
                                    }
                                }
                            }
                            g.drawImage(iskey == true ? indexTabSkill[1] : indexTabSkill[0], xPos, yPos);
                            mFont.tahoma_7_white.drawStringBd(g, "" + (i + 1), xPos + 6, yPos, 0, mFont.tahoma_7b_dark);
                            if ((GameCanvas.isPointerHoldIn(xPos + 6, yPos, indexTabSkill[0].getWidth(), indexTabSkill[0].getHeight()))
                                || array.Length >= 15 && (GameCanvas.isPointerHoldIn(xPos + 6, yPos - scrollInfoSkill.cmy, indexTabSkill[0].getWidth(), indexTabSkill[0].getHeight())))
                            {
                                if (GameCanvas.isPointerJustRelease && GameCanvas.isPointerClick)
                                {
                                    for (int j = 0; j < GameScr.keySkill.Length; j++)
                                    {
                                        if (GameScr.keySkill[j] == skill3)
                                        {
                                            GameScr.keySkill[j] = null;
                                        }
                                    }
                                    GameScr.keySkill[i] = skill3;
                                    GameScr.info1.addInfo("Gán " + currentSkill.name + " vào ô " + (i + 1), 0);
                                    GameScr.gI().saveKeySkillToRMS();
                                    GameCanvas.clearAllPointerEvent();
                                }
                            }
                        }
                    }

                }
            }
            catch (Exception e)
            {
                Res.err(e.ToString());
            }
        }

        public void paintMessClan(mGraphics g, ClanMessage clanMessage, int x, int y)
        {

            mFont mFontText = mFont.tahoma_7_greySmall;
            if (clanMessage.type == 1)
            {
                mFontText = mFont.tahoma_7b_green2;
            }
            else if (clanMessage.type == 2)
            {
                mFontText = mFont.tahoma_7_blue;
            }
            mFont mFont2 = mFont.tahoma_7b_dark;
            if (clanMessage.role == 0)
            {
                mFont2 = mFont.tahoma_7b_red;
            }
            else if (clanMessage.role == 1)
            {
                mFont2 = mFont.tahoma_7b_green;
            }
            else if (clanMessage.role == 2)
            {
                mFont2 = mFont.tahoma_7b_green2;
            }
            ClanMessage cm = clanMessage;
            string chatfull = "";
            if (clanMessage.type == 1)
            {
                chatfull = "Xin đậu (" + clanMessage.recieve + "/" + clanMessage.maxCap + ")";

            }

            else if (clanMessage.type == 2)
            {
                chatfull = mResources.request_join_clan;

            }
            else
            {
                for (int j = 0; j < cm.chat.Length; j++)
                {
                    chatfull += cm.chat[j];
                }
            }
            int wName = mFont.tahoma_7b_green.getWidth(clanMessage.playerName + ": ");
            int wText = mFont.tahoma_7b_green.getWidth(chatfull);

            if (!chatfull.Contains("Gửi Sticker "))
            {
                ClanMessage.PaintHeadMem(g, clanMessage.playerId, x, y + 10);
            }
            string[] array = mFont.tahoma_7_greySmall.splitFontArray(chatfull, 200);
            int hPop = array.Length == 1 ? (20 * array.Length) : (15 * array.Length);
            if (clanMessage.type >= 1)
            {
                if (clanMessage.type == 1 && clanMessage.recieve < 5)
                {
                    hPop = 40;
                }
                else if (clanMessage.type == 2 && Char.myCharz().role == 0)
                {
                    hPop = 40;

                }

            }
            if (chatfull.Contains("Gửi Sticker "))
            {
                int idSticker = -1;
                chatfull = chatfull.Replace("Gửi Sticker ", "").Trim();
                if (int.TryParse(chatfull, out idSticker))
                {
                    g.drawImage(sticker[idSticker], x + 25, y + 7);
                }
                else
                {
                    Console.WriteLine("Lỗi: Không thể chuyển đổi chuỗi thành số nguyên.");
                }
                ClanMessage.PaintHeadMem(g, clanMessage.playerId, x, y + (sticker[idSticker].getHeight() / 2) + 10);

                mFont.tahoma_7_greySmall.drawString(g, NinjaUtil.getTimeAgo(clanMessage.timeAgo) + " " + mResources.ago, x + sticker[idSticker].getWidth() + 25, y + (sticker[idSticker].getHeight() / 2), mFont.LEFT);
            }
            else
            {
                PopUp.paintPopUp(g, x + 25, y + 7, (array.Length == 1 ? (wText + wName + 10) : (210 + wName)), hPop, 16777215, false);

                mFont2.drawString(g, clanMessage.playerName + ": ", x + 30, y + 11, 0);
                if (clanMessage.color == 0)
                {

                    for (int j = 0; j < array.Length; j++)
                    {
                        mFontText.drawString(g, array[j], x + 30 + wName, y + 11 + (j * 10), 0);
                    }
                    mFont.tahoma_7_greySmall.drawString(g, NinjaUtil.getTimeAgo(clanMessage.timeAgo) + " " + mResources.ago, x + (array.Length == 1 ? (wText + wName + 50) : (240 + wName)), y + 15, mFont.LEFT);
                }
                else
                {
                    for (int j = 0; j < array.Length; j++)
                    {
                        mFontText.drawString(g, array[j], x + 30 + wName, y + 11 + (j * 10), 0);
                    }
                }
                if (clanMessage.type == 1 && clanMessage.recieve < 5)
                {
                    int num10 = x + wText + wName + 15;

                    g.drawImage(GameScr.imgLbtn2, num10, y + 35, StaticObj.VCENTER_HCENTER);
                    mFont.tahoma_7b_dark.drawString(g, "Cho", num10, y + 30, mFont.CENTER);

                    if (GameCanvas.isPointerHoldIn(num10 - GameScr.imgLbtn2.getWidth() / 2, (y + 35 - GameScr.imgLbtn2.getHeight() / 2) - scrollChatClan.cmy, GameScr.imgLbtn2.getWidth(), GameScr.imgLbtn2.getHeight()))
                    {
                        if (GameCanvas.isPointerJustRelease && GameCanvas.isPointerClick)
                        {
                            Service.gI().clanDonate(clanMessage.id);
                        }
                    }

                }
                else if (clanMessage.type == 2 && Char.myCharz().role == 0)
                {
                    int num10 = x + wText + wName + 15;

                    g.drawImage(GameScr.imgLbtn2, num10, y + 35, StaticObj.VCENTER_HCENTER);
                    mFont.tahoma_7b_dark.drawString(g, "Nhận", num10, y + 30, mFont.CENTER);

                    g.drawImage(GameScr.imgLbtn2, num10 - 40, y + 35, StaticObj.VCENTER_HCENTER);
                    mFont.tahoma_7b_dark.drawString(g, "Hủy", num10 - 40, y + 30, mFont.CENTER);

                    if (GameCanvas.isPointerHoldIn(num10 - GameScr.imgLbtn2.getWidth() / 2, (y + 35 - GameScr.imgLbtn2.getHeight() / 2) - scrollChatClan.cmy, GameScr.imgLbtn2.getWidth(), GameScr.imgLbtn2.getHeight()))
                    {
                        if (GameCanvas.isPointerJustRelease && GameCanvas.isPointerClick)
                        {
                            Service.gI().joinClan(clanMessage.id, 0);
                        }
                    }
                    else if (GameCanvas.isPointerHoldIn((num10 - 40) - GameScr.imgLbtn2.getWidth() / 2, (y + 35 - GameScr.imgLbtn2.getHeight() / 2) - scrollChatClan.cmy, GameScr.imgLbtn2.getWidth(), GameScr.imgLbtn2.getHeight()))
                    {
                        if (GameCanvas.isPointerJustRelease && GameCanvas.isPointerClick)
                        {
                            Service.gI().joinClan(clanMessage.id, 1);
                        }
                    }

                }
            }
        }
        private Member getLeader()
        {
            for (int j = 0; j < GameCanvas.panel.myMember.size(); j++)
            {
                Member member = (Member)GameCanvas.panel.myMember.elementAt(j);
                if (member.role == 0)
                {
                    return member;
                }
            }
            return null;
        }
        private void paintLeader(mGraphics g, Member member, int x, int y)
        {
            int[] partID = new int[3] { member.head, member.leg, member.body };
            Part part = GameScr.parts[partID[0]];
            Part part2 = GameScr.parts[partID[1]];
            Part part3 = GameScr.parts[partID[2]];
            SmallImage.drawSmallImage(g, part.pi[Char.CharInfo[0][0][0]].id, x + Char.CharInfo[0][0][1] + part.pi[Char.CharInfo[0][0][0]].dx, y - Char.CharInfo[0][0][2] + part.pi[Char.CharInfo[0][0][0]].dy, 0, 0);
            SmallImage.drawSmallImage(g, part2.pi[Char.CharInfo[0][1][0]].id, x + Char.CharInfo[0][1][1] + part2.pi[Char.CharInfo[0][1][0]].dx, y - Char.CharInfo[0][1][2] + part2.pi[Char.CharInfo[0][1][0]].dy, 0, 0);
            SmallImage.drawSmallImage(g, part3.pi[Char.CharInfo[0][2][0]].id, x + Char.CharInfo[0][2][1] + part3.pi[Char.CharInfo[0][2][0]].dx, y - Char.CharInfo[0][2][2] + part3.pi[Char.CharInfo[0][2][0]].dy, 0, 0);
        }
        private void paintItemDetails(mGraphics g, int x, int y, int w, int h)
        {
            try
            {
                if (isItemInfo && itemDetails != null)
                {
                    g.setColor(0xa2471a);
                    g.fillRect(x, y, w, h, 10);
                    g.setColor(0xfefefe);
                    g.fillRect(x + 2, y + 2, w - 4, h - 4, 10);
                    Small small = SmallImage.imgNew[itemDetails.template.iconID];
                    if (small == null)
                    {
                        SmallImage.createImage(itemDetails.template.iconID);
                        return;
                    }
                    Image iconitem = small.img;
                    paintEffectItem(g, itemDetails, x + 6, y + 6);
                    SmallImage.drawSmallImage(g, itemDetails.template.iconID, x + 13, y + 13, 0, 0);

                    string text3 = string.Empty;
                    mFont mFont4 = mFont.tahoma_7_green2;
                    if (itemDetails.itemOption != null)
                    {
                        for (int num31 = 0; num31 < itemDetails.itemOption.Length; num31++)
                        {
                            if (itemDetails.itemOption[num31].optionTemplate.id == 72)
                            {
                                text3 = " [+" + itemDetails.itemOption[num31].param.ToString() + "]";
                            }
                            if (itemDetails.itemOption[num31].optionTemplate.id == 225)
                            {
                                text3 = " [+" + itemDetails.itemOption[num31].param.ToString() + "]";
                            }
                            if (itemDetails.itemOption[num31].optionTemplate.id == 225)
                            {
                                if (itemDetails.itemOption[num31].param >= 1 && itemDetails.itemOption[num31].param <= 2)
                                {
                                    mFont4 = Panel.GetFont(0);
                                }
                                else if (itemDetails.itemOption[num31].param >= 3 && itemDetails.itemOption[num31].param <= 4)
                                {
                                    mFont4 = Panel.GetFont(2);
                                }
                                else if (itemDetails.itemOption[num31].param >= 5 && itemDetails.itemOption[num31].param <= 6)
                                {
                                    mFont4 = Panel.GetFont(8);
                                }
                                else if (itemDetails.itemOption[num31].param >= 7 && itemDetails.itemOption[num31].param <= 10)
                                {
                                    mFont4 = Panel.GetFont(7);
                                }
                            }
                            if (itemDetails.itemOption[num31].optionTemplate.id == 72)
                            {
                                if (itemDetails.itemOption[num31].param >= 1 && itemDetails.itemOption[num31].param <= 5)
                                {
                                    mFont4 = Panel.GetFont(2);
                                }
                                else if (itemDetails.itemOption[num31].param >= 6 && itemDetails.itemOption[num31].param <= 7)
                                {
                                    mFont4 = Panel.GetFont(8);
                                }
                                else if (itemDetails.itemOption[num31].param >= 8 && itemDetails.itemOption[num31].param <= 10)
                                {
                                    mFont4 = Panel.GetFont(7);
                                }
                            }
                        }
                    }
                    mFont4.drawString(g, string.Concat(new string[]
                    {

                            itemDetails.template.name,
                            text3
                    }), x + btnItem.getWidth() + 20, y + 10, 0);

                    g.setColor(UnityEngine.Color.black);
                    g.fillRect(x + 5, y + 35, w - 10, 2, 10);
                    GameCanvas.panel.addItemDetail(itemDetails);
                    int xStar = x + w - 60;
                    int spacestars = Panel.imgMaxStar.getWidth();
                    var cp = GameCanvas.panel.cp;
                    if (cp.maxStarSlot > 4)
                    {
                        cp.nMaxslot_tren = (cp.maxStarSlot + 1) / 2;
                        cp.nMaxslot_duoi = cp.maxStarSlot - cp.nMaxslot_tren;
                        for (int j = 0; j < cp.nMaxslot_tren; j++)
                        {
                            g.drawImage(Panel.imgMaxStar, xStar + j * spacestars + Panel.imgMaxStar.getWidth(), y + 10);
                        }
                        for (int k = 0; k < cp.nMaxslot_duoi; k++)
                        {
                            g.drawImage(Panel.imgMaxStar, xStar + k * spacestars + Panel.imgMaxStar.getWidth(), y + 19);
                        }
                        if (cp.starSlot > 0)
                        {
                            cp.imgStar = Panel.imgStar;
                            if (cp.starSlot >= cp.nMaxslot_tren)
                            {
                                cp.nslot_duoi = cp.starSlot - cp.nMaxslot_tren;
                                for (int l = 0; l < cp.nMaxslot_tren; l++)
                                {
                                    g.drawImage(cp.imgStar, xStar + l * spacestars + Panel.imgMaxStar.getWidth(), y + 10);
                                }
                                for (int m = 0; m < cp.nslot_duoi; m++)
                                {
                                    if (m + cp.nMaxslot_tren >= 7)
                                    {
                                        cp.imgStar = Panel.imgStar8;
                                    }
                                    g.drawImage(cp.imgStar, xStar + m * spacestars + Panel.imgMaxStar.getWidth(), y + 19);
                                }
                            }
                            else
                            {
                                for (int n = 0; n < cp.starSlot; n++)
                                {
                                    g.drawImage(cp.imgStar, xStar + n * spacestars + Panel.imgMaxStar.getWidth(), y + 10);
                                }
                            }
                        }
                    }
                    else
                    {
                        for (int num8 = 0; num8 < cp.maxStarSlot; num8++)
                        {
                            g.drawImage(Panel.imgMaxStar, xStar + num8 * spacestars + Panel.imgMaxStar.getWidth(), y + 6);
                        }
                        if (cp.starSlot > 0)
                        {
                            for (int num9 = 0; num9 < cp.starSlot; num9++)
                            {
                                g.drawImage(Panel.imgStar, xStar + num9 * spacestars + Panel.imgMaxStar.getWidth(), y + 6);
                            }
                        }
                    }
                    if (itemDetails.quantity > 1)
                    {
                        mFont.tahoma_7b_blue.drawString(g, $"x{itemDetails.quantity}", x + w - 30, y + 19, 0);
                    }

                    g.setClip(x + 5, y + 39, w - 10, h - 80);
                    g.setColor(0);
                    g.fillRect(x + w - 6, y + 40, 4, h - 80, 10);
                    g.setColor(UnityEngine.Color.white);
                    g.fillRect(x + w - 6, y + 41, 2, 20, 10);
                    if (scrollInfo != null)
                    {
                        g.translate(scrollInfo.cmx, -scrollInfo.cmy);
                    }
                    /*g.translate(scrollInfo.cmx, -scrollInfo.cmy);*/
                    /*if (itemDetails.itemOption.Length > 5)
                    {
                        InitScrollInfo();
                        scrollInfo.setStyle(itemDetails.itemOption.Length, mFont.tahoma_7b_green.getHeight(), x + 5, y + 39, w - 10, h - 80, true, 1);
                        g.translate(scrollInfo.cmx, -scrollInfo.cmy);
                    }*/


                    int yPaint = y;

                    if (itemDetails.itemOption.Length > 0)
                    {
                        string text = string.Empty;
                        yPaint = y + 41;
                        for (int i = 0; i < itemDetails.itemOption.Length; i++)
                        {
                            if (itemDetails.itemOption[i].optionTemplate.id != 107 && itemDetails.itemOption[i].optionTemplate.id != 72 && itemDetails.itemOption[i].optionTemplate.id != 102)
                            {
                                mFont.tahoma_7b_greenSmall.drawString(g, itemDetails.itemOption[i].getOptionString(), x + 7, yPaint, 0);
                                yPaint += 12;
                            }
                        }
                    }

                    string textdes = string.Empty;
                    if (!itemDetails.template.description.Equals(string.Empty))
                    {
                        if (!itemDetails.template.description.Equals(string.Empty))
                        {
                            yPaint += 10;
                        }
                        textdes = textdes + "\n" + itemDetails.template.description;
                    }
                    string[] array = mFont.tahoma_7.splitFontArray(textdes, w - 20);
                    g.setColor(0, 0.5f);
                    g.fillRect(x + 10, yPaint + 10, w - 20, (array.Length * 10), mGraphics.VCENTER | mGraphics.HCENTER);
                    for (int j = 0; j < array.Length; j++)
                    {
                        mFont.tahoma_7.drawString(g, array[j], x + w / 2, yPaint + 5 + (j * 10), mFont.CENTER);
                    }

                    g.reset();

                    switch (typePaint)
                    {
                        case TypePaint.typeBody:
                        case TypePaint.typeBox:
                            if (tabs[0] != null)
                            {
                                tabs[0].x = x + 5;
                                tabs[0].y = y + h - tabs[0].tabs[0].getHeight() * 3;
                                tabs[0].paint(g);
                                if (tabs[0].Pressed())
                                {
                                    tabs[0].actionPerform();
                                }
                            }
                            if (tabs[5] != null)
                            {
                                tabs[5].x = x + 10 + tabs[5].tabs[0].getWidth();
                                tabs[5].y = y + h - tabs[5].tabs[0].getHeight() * 3;
                                tabs[5].paint(g);
                                if (tabs[5].Pressed())
                                {
                                    tabs[5].actionPerform();
                                }
                            }
                            break;
                        case TypePaint.typeBag:
                            if (tabs[1] != null)
                            {
                                tabs[1].x = x + 5;
                                tabs[1].y = y + h - tabs[1].tabs[0].getHeight() * 3;
                                tabs[1].paint(g);
                                if (tabs[1].Pressed())
                                {
                                    tabs[1].actionPerform();
                                }
                            }
                            if (tabs[2] != null)
                            {
                                tabs[2].x = x + 15 + tabs[2].tabs[0].getWidth() * 2;
                                tabs[2].y = y + h - tabs[2].tabs[0].getHeight() * 3;
                                tabs[2].paint(g);
                                if (tabs[2].Pressed())
                                {
                                    tabs[2].actionPerform();
                                }
                            }
                            if (tabs[3] != null)
                            {
                                tabs[3].x = x + 10 + tabs[3].tabs[0].getWidth();
                                tabs[3].y = y + h - tabs[3].tabs[0].getHeight() * 3;
                                tabs[3].paint(g);
                                if (tabs[3].Pressed())
                                {
                                    tabs[3].actionPerform();
                                }
                            }
                            if (tabs[4] != null)
                            {
                                tabs[4].x = x + 5;
                                tabs[4].y = y + h - tabs[3].tabs[0].getHeight() - 10;
                                tabs[4].paint(g);
                                if (tabs[4].Pressed())
                                {
                                    tabs[4].actionPerform();
                                }
                            }
                            if (tabs[5] != null)
                            {
                                tabs[5].x = x + 10 + tabs[5].tabs[0].getWidth();
                                tabs[5].y = y + h - tabs[3].tabs[0].getHeight() - 10;
                                tabs[5].paint(g);
                                if (tabs[5].Pressed())
                                {
                                    tabs[5].actionPerform();
                                }
                            }
                            break;
                    }

                }
            }
            catch (Exception e)
            {
                Res.err(e.ToString());
            }
        }
        public void paintSnow(mGraphics g)
        {

            int numsnow = (W - (imgIce[4].getWidth() * 2)) / (imgIce[4].getWidth() - 5);
            int xFirst = (X - 10) + imgIce[4].getWidth() - 15;
            g.setClip(X, Y - 5, W - 5, H);
            for (int j = 0; j < numsnow + 2; j++)
            {
                g.drawImage(imgIce[4], xFirst + j * (imgIce[4].getWidth() - 5), Y - 5, 0);
                //  g.drawImage(imgIce[5], xFirst + j * (imgIce[4].getWidth() - 5), Y + H - 13, 0);
            }
            g.reset();
            g.drawImage(beach, X + W / 2 + 25, Y - 19, 0);
            g.drawImage(beach, X + W / 2 + 25, Y - 19, 0);

            g.drawImage(imgIce[0], X - 12, Y - 17, 0);
            g.drawImage(imgIce[1], X + W - imgIce[1].getWidth() + 12, Y - 18, 0);

            /*g.drawImage(imgIce[2], X - 5, Y + H + 3, StaticObj.BOTTOM_LEFT);
            g.drawImage(imgIce[3], X + W - imgIce[1].getWidth() + 5, Y + H + 3, StaticObj.BOTTOM_LEFT);*/

        }
        public void paintSpeacialSkill(mGraphics g, int x, int y, int w, int h)
        {
            try
            {
                if (currTab == THONG_TIN && currTabSkill == 1 && isSpeacialSkill)
                {
                    isBody = false;
                    g.reset();
                    if (currentSkill != null)
                    {
                        currentSkill = null;
                    }

                    g.setColor(0xd9c8b3);
                    g.fillRect(x, y, w, h);
                    g.setColor(0xefefb2);
                    g.fillRect(x + (w / 2) - 25, y, 50, 20, 4);
                    mFont.tahoma_7b_dark.drawString(g, "Nội tại", x + w / 2, y + 3, mGraphics.VCENTER | mGraphics.HCENTER);
                    isItemInfo = false;
                    g.drawImage(imgX, x + w - imgX.getWidth(), y, 0);
                    g.setClip(x, y + 20, w, h - 20);

                    scrollNoitai.setStyle(sizeSpecialSkill, 25, x, y + 20, w, h - 20, true, 1);


                    g.translate(0, -scrollNoitai.cmy);

                    for (int j = 0; j < sizeSpecialSkill; j++)
                    {
                        int Ymem = y + 20 + (j * 25);
                        bool isfocus = GameCanvas.isPointerHoldIn(x, Ymem - scrollNoitai.cmy, w, 25);
                        g.setColor(isfocus ? 16383818 : 0xe5ddd0);
                        g.fillRect(x, Ymem, w, 25);
                        g.setColor(0xd9c8b3);
                        g.drawRect(x, Ymem, w, 25);
                        g.setColor(0x977b55);
                        g.fillRect(x + 1, Ymem + 1, 23, 23);

                        string[] array = mFont.tahoma_7_grey.splitFontArray(Char.myCharz().infoSpeacialSkill[0][j], w - 60);
                        SmallImage.drawSmallImage(g, Char.myCharz().imgSpeacialSkill[0][j], x + 3, Ymem + 3, 0, mGraphics.LEFT | mGraphics.TOP);

                        for (int i = 0; i < array.Length; i++)
                        {
                            mFont.tahoma_7_grey.drawString(g, array[i], x + 28, Ymem + (i * 10), 0);
                        }

                        if (GameCanvas.isPointerHoldIn(x + w - imgX.getWidth(), y, imgX.getWidth(), imgX.getHeight()))
                        {
                            if (GameCanvas.isPointerJustRelease && GameCanvas.isPointerClick)
                            {
                                isBody = true;
                                isSpeacialSkill = false;
                                GameCanvas.clearAllPointerEvent();
                            }
                        }

                        else if (isfocus)
                        {
                            if (GameCanvas.isPointerJustRelease && GameCanvas.isPointerClick)
                            {
                                string info = Char.myCharz().infoSpeacialSkill[0][j];
                                GameScr.info1.addInfo(info, j);
                                currspecialskill = j;


                            }
                        }

                    }
                    Cout.println("currspecialskill " + currspecialskill);
                    if (currspecialskill != -1)
                    {
                        simplebutton(g, "Auto", x + w - 25, y + 20 + (currspecialskill * 25) + 1, 25, 23, 0xefefb2, 0x967b55, 8011);
                    }

                    // scrollInfo.setStyle(sizeSpecialSkill, mFont.tahoma_7_grey.getHeight() + sizeSpecialSkill, x, y, w - 10, h - 80, true, 1);                                      
                }

            }
            catch (Exception e)
            {
                Res.err(e.ToString());
            }
        }
        public void paintMemberClan(mGraphics g, int x, int y, int w, int h)
        {
            try
            {
                if (currTab == BANG_HOI)
                {
                    if (isShowMemClan)
                    {
                        g.setColor(0x000000, 0.5f);
                        g.fillRect(x, y + 20, w, h - 20, 4);
                        int padding = 10;
                        int membersPerRow = 2; // Số thành viên mỗi hàng
                        int HMem = 50; // Chiều cao mỗi ô thành viên
                        int WMem = (w - (membersPerRow - 1) * padding) / membersPerRow; // Chiều rộng mỗi ô
                        g.setClip(x, y + 20, w, h - 20);
                        if (GameCanvas.panel.myMember.size() > 6)
                        {
                            scrollMemClan.setStyle(GameCanvas.panel.myMember.size() / 2, WMem, x, y + 20, w, h - 20, true, 2);
                            g.translate(scrollMemClan.cmx, -scrollMemClan.cmy);
                        }

                        for (int j = 0; j < GameCanvas.panel.myMember.size(); j++)
                        {
                            // Tính toán vị trí x, y của từng thành viên
                            int row = j / membersPerRow;
                            int col = j % membersPerRow;
                            int Xmem = x + col * (WMem + padding);
                            int Ymem = y + 20 + row * (HMem + padding);
                            bool iscantouchall = true;
                            if (Xmem > x + (w / 2) && isshowmenuright)
                            {
                                iscantouchall = false;
                            }
                            // Kiểm tra nếu con trỏ đang trỏ vào ô thành viên
                            bool selected = GameCanvas.isPointerHoldIn(Xmem, Ymem - scrollMemClan.cmy, iscantouchall ? WMem : WMem - 20, HMem);


                            g.setColor(0x967b55);
                            g.fillRect(Xmem, Ymem, WMem, HMem, 4);
                            g.setColor(selected && !isShowInfomem ? 16383818 : 0xefefb2);
                            g.fillRect(Xmem + 2, Ymem + 2, WMem - 4, HMem - 4, 4);

                            Member member = (Member)GameCanvas.panel.myMember.elementAt(j);
                            int[] partID = new int[3] { member.head, member.leg, member.body };
                            int yMempaint = Ymem + HMem - 5;
                            Part part = GameScr.parts[partID[0]];
                            Part part2 = GameScr.parts[partID[1]];
                            Part part3 = GameScr.parts[partID[2]];
                            SmallImage.drawSmallImage(g, part.pi[Char.CharInfo[0][0][0]].id, Xmem + 15 + Char.CharInfo[0][0][1] + part.pi[Char.CharInfo[0][0][0]].dx, yMempaint - Char.CharInfo[0][0][2] + part.pi[Char.CharInfo[0][0][0]].dy, 0, 0);
                            SmallImage.drawSmallImage(g, part2.pi[Char.CharInfo[0][1][0]].id, Xmem + 15 + Char.CharInfo[0][1][1] + part2.pi[Char.CharInfo[0][1][0]].dx, yMempaint - Char.CharInfo[0][1][2] + part2.pi[Char.CharInfo[0][1][0]].dy, 0, 0);
                            SmallImage.drawSmallImage(g, part3.pi[Char.CharInfo[0][2][0]].id, Xmem + 15 + Char.CharInfo[0][2][1] + part3.pi[Char.CharInfo[0][2][0]].dx, yMempaint - Char.CharInfo[0][2][2] + part3.pi[Char.CharInfo[0][2][0]].dy, 0, 0);
                            /*}*/

                            // Đặt màu chữ theo vai trò
                            mFont mFont2 = mFont.tahoma_7b_dark;
                            if (member.role == 0) mFont2 = mFont.tahoma_7b_red;
                            else if (member.role == 1) mFont2 = mFont.tahoma_7b_green;
                            else if (member.role == 2) mFont2 = mFont.tahoma_7b_green2;

                            // Vẽ tên và thông tin thành viên
                            mFont2.drawString(g, member.name, Xmem + 43, Ymem + 5, 0);
                            mFont.tahoma_7_blue.drawString(g, mResources.power + ": " + member.powerPoint,
                                Xmem + 40, Ymem + 20, 0);

                            // Vẽ biểu tượng và điểm clan
                            SmallImage.drawSmallImage(g, 7223, Xmem + WMem - 7, Ymem + 10, 0, 3);
                            mFont.tahoma_7_blue.drawString(g, "" + member.clanPoint,
                                Xmem + WMem - 15, Ymem + 5, mFont.RIGHT);
                            if (selected && !isShowInfomem)
                            {
                                if (GameCanvas.isPointerJustRelease && GameCanvas.isPointerClick)
                                {
                                    if (currmember != member)
                                    {
                                        currmember = member;
                                    }
                                    else
                                    {
                                        currmember = null;
                                        isShowInfomem = false;
                                    }

                                    GameCanvas.clearAllPointerEvent();
                                }
                            }

                        }
                        g.reset();
                        if (currmember != null)
                        {
                            paintPopupInfoMember(g, currmember, x + w / 4, y + 20, w / 2, 160);
                        }
                    }
                }
            }
            catch (Exception e)
            {
                Res.err(e.ToString());
            }
        }
        public void paintMemberClan(mGraphics g, Clan clan, int x, int y, int w, int h)
        {
            try
            {
                if (currTab == BANG_HOI)
                {
                    if (isShowMemClan)
                    {

                        int padding = 10;
                        int membersPerRow = 2; // Số thành viên mỗi hàng
                        int HMem = 50; // Chiều cao mỗi ô thành viên
                        int WMem = (w - (membersPerRow - 1) * padding) / membersPerRow; // Chiều rộng mỗi ô
                        g.reset();
                        g.setClip(x, y + 20, w, h - 20);
                        if (GameCanvas.panel.member == null)
                        {
                            Service.gI().clanMember(currclan.ID);
                        }
                        if (GameCanvas.panel.member.size() > 6)
                        {
                            scrollMemClan.setStyle(GameCanvas.panel.member.size() / 2, WMem, x, y + 20, w, h - 20, true, 2);
                            g.translate(scrollMemClan.cmx, -scrollMemClan.cmy);
                        }
                        for (int j = 0; j < GameCanvas.panel.member.size(); j++)
                        {
                            // Tính toán vị trí x, y của từng thành viên
                            int row = j / membersPerRow;
                            int col = j % membersPerRow;
                            int Xmem = x + col * (WMem + padding);
                            int Ymem = y + 20 + row * (HMem + padding);
                            bool iscantouchall = true;

                            // Kiểm tra nếu con trỏ đang trỏ vào ô thành viên
                            bool selected = GameCanvas.isPointerHoldIn(Xmem, Ymem - scrollMemClan.cmy, WMem, HMem);


                            g.setColor(0x967b55);
                            g.fillRect(Xmem, Ymem, WMem, HMem, 4);
                            g.setColor(selected ? 16383818 : 0xefefb2);
                            g.fillRect(Xmem + 2, Ymem + 2, WMem - 4, HMem - 4, 4);

                            Member member = (Member)GameCanvas.panel.member.elementAt(j);
                            int[] partID = new int[3] { member.head, member.leg, member.body };
                            int yMempaint = Ymem + HMem - 5;
                            Part part = GameScr.parts[partID[0]];
                            Part part2 = GameScr.parts[partID[1]];
                            Part part3 = GameScr.parts[partID[2]];
                            SmallImage.drawSmallImage(g, part.pi[Char.CharInfo[0][0][0]].id, Xmem + 15 + Char.CharInfo[0][0][1] + part.pi[Char.CharInfo[0][0][0]].dx, yMempaint - Char.CharInfo[0][0][2] + part.pi[Char.CharInfo[0][0][0]].dy, 0, 0);
                            SmallImage.drawSmallImage(g, part2.pi[Char.CharInfo[0][1][0]].id, Xmem + 15 + Char.CharInfo[0][1][1] + part2.pi[Char.CharInfo[0][1][0]].dx, yMempaint - Char.CharInfo[0][1][2] + part2.pi[Char.CharInfo[0][1][0]].dy, 0, 0);
                            SmallImage.drawSmallImage(g, part3.pi[Char.CharInfo[0][2][0]].id, Xmem + 15 + Char.CharInfo[0][2][1] + part3.pi[Char.CharInfo[0][2][0]].dx, yMempaint - Char.CharInfo[0][2][2] + part3.pi[Char.CharInfo[0][2][0]].dy, 0, 0);
                            /*}*/

                            // Đặt màu chữ theo vai trò
                            mFont mFont2 = mFont.tahoma_7b_dark;
                            if (member.role == 0) mFont2 = mFont.tahoma_7b_red;
                            else if (member.role == 1) mFont2 = mFont.tahoma_7b_green;
                            else if (member.role == 2) mFont2 = mFont.tahoma_7b_green2;

                            // Vẽ tên và thông tin thành viên
                            mFont2.drawString(g, member.name, Xmem + 43, Ymem + 5, 0);
                            mFont.tahoma_7_blue.drawString(g, mResources.power + ": " + member.powerPoint,
                                Xmem + 40, Ymem + 20, 0);

                            // Vẽ biểu tượng và điểm clan
                            SmallImage.drawSmallImage(g, 7223, Xmem + WMem - 7, Ymem + 10, 0, 3);
                            mFont.tahoma_7_blue.drawString(g, "" + member.clanPoint,
                                Xmem + WMem - 15, Ymem + 5, mFont.RIGHT);
                            if (selected)
                            {
                                if (GameCanvas.isPointerJustRelease && GameCanvas.isPointerClick)
                                {
                                    if (currmember != member)
                                    {
                                        currmember = member;
                                    }
                                    else
                                    {
                                        currmember = null;
                                        isShowInfomem = false;
                                    }
                                }
                            }

                        }

                        if (currmember != null)
                        {
                            g.reset();
                            paintPopupInfoMember(g, currmember, x + w / 4, y, w / 2, 130);
                        }

                    }
                }
            }
            catch (Exception e)
            {
                Res.err(e.ToString());
            }
        }
        public void paintClans(mGraphics g, int x, int y, int w, int h)
        {
            try
            {
                if (currTab == BANG_HOI)
                {

                    Clan[] clans = GameCanvas.panel.clans;

                    g.setColor(0x967b55);
                    g.fillRect(x + 30, y, w / 2 + 20, 20, 4);
                    g.setColor(0xefefb2);
                    g.fillRect(x + 32, y + 2, w / 2 + 16, 16, 4);

                    mFont.tahoma_7b_dark.drawString(g, "Danh Sách Bang", x + 47, y + 3, 0);

                    g.setClip(x, y + 20, w, h - 20);
                    g.setColor(0x967b55, 0.5f);
                    g.fillRect(x, y + 20, w, h - 20, 4);
                    int padding = 0;
                    int membersPerRow = 1; // Số thành viên mỗi hàng
                    int HMem = 40; // Chiều cao mỗi ô thành viên
                    int WMem = (w - (membersPerRow - 1) * padding) / membersPerRow; // Chiều rộng mỗi ô
                    if (clans == null)
                    {
                        mFont.tahoma_7b_dark.drawString(g, "Không tìm thấy bang", x + 42, y + (h / 2), 0);
                        return;
                    }
                    if (clans.Length > 5)
                    {
                        scrollClans.setStyle(clans.Length, HMem, x, y + 20, w, h - 20, true, 1);
                        g.translate(0, -scrollClans.cmy);
                    }

                    for (int j = 0; j < clans.Length; j++)
                    {
                        if (clans == null || clans[j] == null)
                        {
                            continue;
                        }
                        int row = j / membersPerRow;
                        int col = j % membersPerRow;
                        int Xmem = x + col * (WMem + padding);
                        int Ymem = y + 20 + (HMem * j);
                        int Yfl = y + (HMem * (j + 1));
                        bool iscantouchall = true;
                        bool selected = GameCanvas.isPointerHoldIn(Xmem, Ymem - scrollClans.cmy, iscantouchall ? WMem : WMem - 20, HMem);
                        g.setColor(0x967b55);
                        g.fillRect(Xmem, Ymem, WMem, HMem, 4);
                        g.setColor(selected ? 16383818 : 0xefefb2);
                        g.fillRect(Xmem + 2, Ymem + 2, WMem - 4, HMem - 4, 4);
                        g.setColor(0x967b55);
                        g.fillRect(Xmem, Ymem, HMem, HMem, 4);
                        g.setColor(selected ? 16383818 : 0x080808, 0.7f);
                        g.fillRect(Xmem + 2, Ymem + 2, HMem - 4, HMem - 4, 4);

                        if (ClanImage.isExistClanImage(clans[j].imgID))
                        {

                            if (ClanImage.getClanImage((short)clans[j].imgID).idImage != null)
                            {
                                SmallImage.drawSmallImage(g, ClanImage.getClanImage((short)clans[j].imgID).idImage[0], Xmem + 20, Ymem + 20, 0, StaticObj.VCENTER_HCENTER);
                            }
                        }
                        else
                        {

                            ClanImage clanImage = new ClanImage();
                            clanImage.ID = clans[j].imgID;
                            if (!ClanImage.isExistClanImage(clanImage.ID))
                            {
                                ClanImage.addClanImage(clanImage);
                            }
                        }

                        mFont mFont2 = mFont.tahoma_7b_red;
                        mFont2.drawString(g, clans[j].name, Xmem + 45, Ymem + 5, 0);
                        mFont.tahoma_7_blue.drawString(g, clans[j].slogan,
                            Xmem + 45, Ymem + 20, 0);
                        mFont.tahoma_7_green2.drawString(g, clans[j].currMember + "/" + clans[j].maxMember, Xmem + WMem - 10, Ymem + 3, mFont.RIGHT);
                        if (selected)
                        {
                            if (GameCanvas.isPointerJustRelease && GameCanvas.isPointerClick)
                            {
                                if (currclan != clans[j])
                                {
                                    currclan = clans[j];
                                }
                                else
                                {
                                    currclan = null;
                                    isShowInfomem = false;
                                }


                            }
                        }

                    }

                    if (currclan != null)
                    {

                        paintPopupInfoClan(g, currclan, x + w + 50, y + 25, w, 130);
                    }

                }
            }
            catch (Exception e)
            {
                Res.err(e.ToString());
            }
        }
        private void paintEffectItem(mGraphics g, Item item, int x, int y)
        {
            try
            {
                Image[] bg = null;
                Image[] eff = null;
                if (item != null && item.itemOption != null)
                {
                    foreach (var option in item.itemOption)
                    {
                        if (option != null && (option.optionTemplate.id == 72 || option.optionTemplate.id == 225 || (option.optionTemplate.id >= 241 && option.optionTemplate.id <= 247)))
                        {
                            switch (option.param)
                            {
                                case 1:
                                    eff = Panel.effxanhnhat;
                                    break;
                                case 2:
                                    bg = Panel.bgxanhnhat;
                                    eff = Panel.effxanhnhat;
                                    break;
                                case 3:
                                    eff = Panel.effxanhla;
                                    break;
                                case 4:
                                    bg = Panel.bgxanhla;
                                    eff = Panel.effxanhla;
                                    break;
                                case 5:
                                    eff = Panel.efftim;
                                    break;
                                case 6:
                                    bg = Panel.bgtim;
                                    eff = Panel.efftim;
                                    break;
                                case 7:
                                    eff = Panel.effcam;
                                    break;
                                case 8:
                                    bg = Panel.bgcam;
                                    eff = Panel.effcam;
                                    break;
                                case 9:
                                    eff = Panel.effdo;
                                    break;
                                case 10:
                                    bg = Panel.bgdo;
                                    eff = Panel.effdo;
                                    break;
                            }
                            //Res.err(eff != null ? "non null" : "null");
                            /*if (eff != null)
                                g.drawImage(eff[GameCanvas.gameTick / 4 % 7], x + 2, y + 2);
                            if (bg != null)
                                g.drawImage(bg[GameCanvas.gameTick / 4 % 7], x - 1, y - 1);*/
                            //Res.err(x + " " + y);
                        }
                        //if (option != null && option.optionTemplate.id == 225)
                        //{
                        //    switch (option.param)
                        //    {
                        //        case 0:
                        //        case 1:
                        //        case 2:
                        //        case 3:
                        //            bg = bgxanhnhat;
                        //            break;
                        //        case 4:
                        //        case 5:
                        //            bg = bgxanhnhat;
                        //            eff = effxanhnhat;
                        //            break;
                        //        case 6:
                        //        case 7:
                        //        case 8:
                        //            bg = bgxanhla;
                        //            break;
                        //        case 9:                               
                        //        case 10:
                        //            bg = bgxanhla;
                        //            eff = effxanhla;
                        //            break;
                        //        case 11:
                        //        case 12:
                        //        case 13:
                        //            bg = bgtim;
                        //            break;
                        //        case 14:
                        //        case 15:
                        //            bg = bgtim;
                        //            eff = efftim;
                        //            break;
                        //        case 16:
                        //        case 17:
                        //        case 18:
                        //        case 19:
                        //            bg = bgcam;
                        //            break;
                        //        case 20:
                        //            bg = bgcam;
                        //            eff = effcam;
                        //            break;
                        //    }
                        //    //Res.err(eff != null ? "non null" : "null");
                        //    if (eff != null)
                        //        g.drawImage(eff[GameCanvas.gameTick / 4 % 7], x + 2, y + 2);
                        //    if (bg != null)
                        //        g.drawImage(bg[GameCanvas.gameTick / 4 % 7], x - 1, y - 1);
                        //    //Res.err(x + " " + y);
                        //}
                    }
                }
            }
            catch (Exception e)
            {
                Debug.LogException(e);
            }
        }
        public void showLeaderMenu(mGraphics g, int x, int y, int w, int h)
        {
            try
            {
                if (currTab == BANG_HOI)
                {
                    bool isMain = !isShowMemClan && !isShowchatClan;
                    if (isMain)
                    {
                        int x1 = x + w - 55;
                        int y1 = y + h - 15;
                        int wbar = 110;
                        int hbar = 10;
                        g.setColor(0x080808);
                        g.fillRect(x1 - 5, y1 + 5, wbar + 10, hbar, 4);

                        g.setColor(0x5d5455);
                        g.fillRect(x1, y1, wbar, hbar, 4);
                        simplebutton(g, "Biểu\nTượng", x1 + 15, y1 - 30, 35, 35, 0xefefb2, 0x967b55, 0);
                        simplebutton(g, "Khẩu\nHiệu", x1 + 60, y1 - 30, 35, 35, 0xefefb2, 0x967b55, 1);


                    }

                }
            }
            catch (Exception e)
            {
                Res.err(e.ToString());
            }
        }
        public void simplebutton(mGraphics g, String text, int x, int y, int w, int h, int rgbin, int rgbout, int action)
        {
            try
            {
                bool selected = GameCanvas.isPointerHoldIn(x, y, w, h);

                g.setColor(rgbout);
                g.fillRect(x, y, w, h, 4);
                if (action == 8011)
                {
                    selected = GameCanvas.isPointerHoldIn(x, y - scrollNoitai.cmy, w, h);
                    g.setColor(selected ? 0x06d7fe : rgbin);
                }
                else
                {
                    g.setColor(selected ? 16383818 : rgbin);
                }
                g.fillRect(x + 2, y + 2, w - 4, h - 4, 4);
                int ytext = y + 5;
                string[] array = mFont.tahoma_7b_white.splitFontArray(text, w - 10);
                if (array.Length == 1)
                {
                    ytext = (y + h / 2) - mFont.tahoma_7b_white.getHeight() / 2;
                }
                mFont.tahoma_7b_white.drawStringBd(g, text, x + w / 2 + 2, ytext, mGraphics.VCENTER | mGraphics.HCENTER, mFont.tahoma_7b_red);

                if (selected)
                {
                    if (GameCanvas.isPointerJustRelease && GameCanvas.isPointerClick)
                    {
                        switch (action)
                        {
                            case 0:
                                Service.gI().getClan(3, -1, null);
                                changeIcon();
                                break;
                            case 1:
                                changeslogan();
                                break;
                            case 2:
                                isShow = false;
                                GameScr.isPaintOther = false;
                                Service.gI().leaveClan();
                                break;
                            case 3:
                                isShow = false;
                                GameScr.isPaintOther = false;
                                searchClan();
                                break;
                            case 4:
                                isShow = false;
                                GameScr.isPaintOther = false;
                                creatClan();
                                Service.gI().getClan(1, -1, null);
                                break;
                            case 5:
                                Service.gI().clanMessage(2, null, currclan.ID);
                                break;
                            case 6:
                                Service.gI().clanMember(currclan.ID);
                                isShowMemClan = true;
                                break;
                            case 7:
                                currclan = null;
                                isShowMemClan = false;
                                break;
                            case 8011:
                                string info = Char.myCharz().infoSpeacialSkill[0][currspecialskill];
                                /*MyVector myVector8 = new();
                                myVector8.addElement(new Command(ModFunc.strChooseIntrinsic, this, 8011, info));
                                GameCanvas.menu.startAt(myVector8, 3);*/
                                if (chatTField == null)
                                {
                                    chatTField = new ChatTextField();
                                    chatTField.tfChat.y = GameCanvas.h - 35 - ChatTextField.gI().tfChat.height;
                                    chatTField.initChatTextField();
                                    chatTField.parentScreen = this;
                                }
                                string infoIntrinsic = info;
                                ModFunc.GI().curSelectIntrinsic = infoIntrinsic;

                                autoskillspecial();
                                break;
                        }
                        GameCanvas.clearAllPointerEvent();
                    }
                }
            }
            catch (Exception e)
            {
                Res.err(e.ToString());
            }
        }
        private void autoskillspecial()
        {
            isShow = false;
            GameScr.isPaintOther = false;
            ChatTextField.gI().strChat = ModFunc.strChooseIntrinsic;
            ChatTextField.gI().tfChat.name = ModFunc.strChooseIntrinsic;
            ChatTextField.gI().to = string.Empty;
            ChatTextField.gI().isShow = true;
            ChatTextField.gI().tfChat.isFocus = true;
            ChatTextField.gI().tfChat.setIputType(TField.INPUT_TYPE_ANY);

            if (Main.isWindowsPhone)
            {
                ChatTextField.gI().tfChat.strInfo = ChatTextField.gI().strChat;

            }
            if (!Main.isPC)
            {
                ChatTextField.gI().startChat2(GameScr.gI(), string.Empty);


            }
        }
        private void changeslogan()
        {
            isShow = false;
            GameScr.isPaintOther = false;
            ChatTextField.gI().strChat = mResources.input_clan_slogan;
            ChatTextField.gI().tfChat.name = mResources.input_clan_slogan;
            ChatTextField.gI().to = string.Empty;
            ChatTextField.gI().isShow = true;
            ChatTextField.gI().tfChat.isFocus = true;
            ChatTextField.gI().tfChat.setIputType(TField.INPUT_TYPE_ANY);

            if (Main.isWindowsPhone)
            {
                ChatTextField.gI().tfChat.strInfo = ChatTextField.gI().strChat;

            }
            if (!Main.isPC)
            {
                ChatTextField.gI().startChat2(GameScr.gI(), string.Empty);


            }
        }

        public void creatClan()
        {
            ChatTextField.gI().strChat = mResources.input_clan_name_to_create;
            ChatTextField.gI().tfChat.name = mResources.input_clan_name;
            ChatTextField.gI().to = string.Empty;
            ChatTextField.gI().isShow = true;
            ChatTextField.gI().tfChat.isFocus = true;
            ChatTextField.gI().tfChat.setIputType(TField.INPUT_TYPE_ANY);
            if (Main.isWindowsPhone)
            {
                ChatTextField.gI().tfChat.strInfo = ChatTextField.gI().strChat;
            }
            if (!Main.isPC)
            {
                ChatTextField.gI().startChat2(GameScr.gI(), string.Empty);
            }
        }
        private void searchClan()
        {
            ChatTextField.gI().strChat = mResources.input_clan_name;
            ChatTextField.gI().tfChat.name = mResources.clan_name;
            ChatTextField.gI().to = string.Empty;
            ChatTextField.gI().isShow = true;
            ChatTextField.gI().tfChat.isFocus = true;
            ChatTextField.gI().tfChat.setIputType(TField.INPUT_TYPE_ANY);
            if (Main.isWindowsPhone)
            {
                ChatTextField.gI().tfChat.strInfo = ChatTextField.gI().strChat;
            }
            if (!Main.isPC)
            {
                ChatTextField.gI().startChat2(GameScr.gI(), string.Empty);
            }
        }
        private void chatClan()
        {
            isShow = false;
            GameScr.isPaintOther = false;
            ChatTextField.gI().strChat = mResources.chat_clan;
            ChatTextField.gI().tfChat.name = mResources.CHAT;
            ChatTextField.gI().to = string.Empty;
            ChatTextField.gI().isShow = true;
            ChatTextField.gI().tfChat.isFocus = true;
            ChatTextField.gI().tfChat.setIputType(TField.INPUT_TYPE_ANY);

            if (Main.isWindowsPhone)
            {
                ChatTextField.gI().tfChat.strInfo = ChatTextField.gI().strChat;

            }
            if (!Main.isPC)
            {
                ChatTextField.gI().startChat2(GameScr.gI(), string.Empty);


            }
        }
        public void changeIcon()
        {
            if (tabIcon == null)
            {
                tabIcon = new TabClanIcon();
            }
            tabIcon.text = "ok";
            tabIcon.show(false);
            if (GameCanvas.panel.chatTField != null)
            {
                GameCanvas.panel.chatTField.isShow = false;
            }
        }
        public void showRightMenuClan(mGraphics g, int x, int y, int w, int h)
        {
            try
            {
                if (currTab == BANG_HOI)
                {
                    bool isMain = !isShowMemClan && !isShowchatClan;
                    int xbar = (x + (w * 2) - (iconMainClan.getWidth() + 10));
                    int yEach = (iconMainClan.getHeight() / 2) + 5;
                    g.drawImage(isshowmenuright ? selectmenu[0] : selectmenu[1], xbar + 12, y, mGraphics.LEFT);
                    if (GameCanvas.isPointerHoldIn(xbar + 12, y, selectmenu[0].getWidth(), selectmenu[0].getHeight()))
                    {
                        if (GameCanvas.isPointerJustRelease && GameCanvas.isPointerClick)
                        {
                            isshowmenuright = !isshowmenuright;
                            GameCanvas.clearAllPointerEvent();
                        }
                    }

                    if (!isPaintStickers && isshowmenuright)
                    {
                        g.setColor(0x967b55);
                        g.fillRect(xbar, y + yEach, iconMemClan.getWidth(), (iconMemClan.getHeight() * 3) + 20, 4);
                        g.drawImage(iconMainClan, isMain ? xbar - 10 : xbar, y + yEach, mGraphics.LEFT);
                        g.drawImage(iconChatClan, isShowchatClan ? xbar - 10 : xbar, y + yEach * 3, mGraphics.LEFT);
                        g.drawImage(iconMemClan, isShowMemClan ? xbar - 10 : xbar, y + yEach * 5, mGraphics.LEFT);

                        if (GameCanvas.isPointerHoldIn(isMain ? xbar - 10 : xbar, y + yEach, iconMainClan.getWidth(), iconMainClan.getHeight()))
                        {
                            if (GameCanvas.isPointerJustRelease && GameCanvas.isPointerClick)
                            {
                                if (!isMain)
                                {
                                    isShowMemClan = false;
                                    isShowchatClan = false;
                                }

                                GameCanvas.clearAllPointerEvent();
                            }
                        }
                        if (GameCanvas.isPointerHoldIn(isShowchatClan ? xbar - 10 : xbar, y + yEach * 3, iconMainClan.getWidth(), iconMainClan.getHeight()))
                        {
                            if (GameCanvas.isPointerJustRelease && GameCanvas.isPointerClick)
                            {
                                if (isShowMemClan)
                                {
                                    isShowMemClan = false;
                                }
                                if (!isShowchatClan)
                                {
                                    isShowchatClan = true;
                                    isShowInfomem = false;
                                }
                                GameCanvas.clearAllPointerEvent();
                            }
                        }
                        if (GameCanvas.isPointerHoldIn(isShowMemClan ? xbar - 10 : xbar, y + yEach * 5, iconMainClan.getWidth(), iconMainClan.getHeight()))
                        {
                            if (GameCanvas.isPointerJustRelease && GameCanvas.isPointerClick)
                            {
                                if (isShowchatClan)
                                {
                                    isShowchatClan = false;
                                }
                                if (!isShowMemClan)
                                {
                                    if (currclan != null)
                                    {
                                        currclan = null;
                                    }
                                    isShowMemClan = true;
                                }
                                GameCanvas.clearAllPointerEvent();
                            }
                        }
                    }

                }
            }
            catch (Exception e)
            {
                Res.err(e.ToString());
            }
        }


        public void perform(int idAction, object p)
        {
            switch (idAction)
            {
                case 1:
                    if (GameScr.gI().isBagFull())
                    {
                        GameScr.info1.addInfo("|2|Hành Trang Đã Đầy!", 0);
                    }
                    else
                    {
                        if (typePaint == TypePaint.typeBody)
                        {
                            if (currTab == DE_TU)
                            {
                                Service.gI().getItem(Panel.PET_BAG, (sbyte)selected);
                            }
                            else
                            {
                                Service.gI().getItem(Panel.BODY_BAG, (sbyte)selected);
                            }
                        }
                        else
                        {
                            Service.gI().getItem(Panel.BOX_BAG, (sbyte)selected);
                        }
                    }
                    isItemInfo = false;
                    isSpeacialSkill = false;
                    isBody = true;
                    break;
                case 2:
                    if (!itemDetails.isTypeBody())
                    {
                        Service.gI().useItem(0, 1, (sbyte)itemDetails.indexUI, -1);
                    }
                    else
                    {
                        Service.gI().getItem(Panel.BAG_BODY, (sbyte)selected);
                    }
                    currTab = HANH_TRANG;
                    isItemInfo = false;
                    isSpeacialSkill = false;
                    isBody = true;
                    break;
                case 3:
                    Service.gI().useItem(2, 1, (sbyte)itemDetails.indexUI, -1);
                    isItemInfo = false;
                    isSpeacialSkill = false;
                    isBody = true;
                    break;
                case 4:
                    if (GameScr.gI().isBoxFull())
                    {
                        GameScr.info1.addInfo("|2|Rương Đồ Đã Đầy!", 0);
                    }
                    else
                    {
                        Service.gI().getItem(Panel.BAG_BOX, (sbyte)selected);
                    }
                    isItemInfo = false;
                    isSpeacialSkill = false;
                    isBody = true;
                    break;
                case 5:
                    Service.gI().getItem(Panel.BAG_PET, (sbyte)selected);
                    currTab = DE_TU;
                    isItemInfo = false;
                    isSpeacialSkill = false;
                    isBody = true;
                    break;
                case 6:
                    isItemInfo = false;
                    isSpeacialSkill = false;
                    isBody = true;
                    break;
                case 7:
                    Service.gI().upPotential(false, typePoint, PointPlus[typeNumPoint]);
                    break;
                case 8:
                    Service.gI().upPotential(false, typePoint, PointPlus[typeNumPoint]);
                    break;
                case 9:
                    Service.gI().upPotential(false, typePoint, PointPlus[1]);
                    break;
                case 1000:
                    GameCanvas.panel.setTypeMain();
                    GameCanvas.panel.show();
                    break;
                case 999:
                    isShow = true;
                    Service.gI().petInfo();
                    SoundMn.gI().panelOpen();
                    break;
            }
        }
        public void addSkillDetail(mGraphics g, SkillTemplate tp, Skill skill, Skill nextSkill, int x, int y, int w, int h)
        {
            cp = new ChatPopup();
            string text = "|0|" + tp.name;
            for (int i = 0; i < tp.description.Length; i++)
            {
                text = text + "\n|4|" + tp.description[i];
            }
            text += "\n--";
            if (skill != null)
            {
                string text2 = text;
                text = text2 + "\n|2|" + mResources.cap_do + ": " + skill.point;
                text = text + "\n|5|" + NinjaUtil.Replace(tp.damInfo, "#", skill.damage + string.Empty);
                text2 = text;
                text = text2 + "\n|5|" + mResources.KI_consume + skill.manaUse + ((tp.manaUseType != 1) ? string.Empty : "%");
                text2 = text;
                text = text2 + "\n|5|" + mResources.cooldown + ": " + skill.strTimeReplay() + "s";
                text += "\n--";
                if (skill.point == tp.maxPoint)
                {
                    text = text + "\n\n|0|" + mResources.max_level_reach;
                }
                else
                {
                    if (!skill.template.isSkillSpec())
                    {
                        text2 = text;
                        text = text2 + "\n\n|1|" + mResources.next_level_require + Res.formatNumber(nextSkill.powRequire) + " " + mResources.potential;
                    }
                    text = text + "\n|4|" + NinjaUtil.Replace(tp.damInfo, "#", nextSkill.damage + string.Empty);
                }
                text += "\n\n|7|Gán phím";
            }
            else
            {
                text = text + "\n|2|" + mResources.not_learn;
                string text2 = text;
                text = text2 + "\n|1|" + mResources.learn_require + Res.formatNumber(nextSkill.powRequire) + " " + mResources.potential;
                text = text + "\n|4|" + NinjaUtil.Replace(tp.damInfo, "#", nextSkill.damage + string.Empty);
                text2 = text;
                text = text2 + "\n|4|" + mResources.KI_consume + nextSkill.manaUse + ((tp.manaUseType != 1) ? string.Empty : "%");
                text2 = text;
                text = text2 + "\n|4|" + mResources.cooldown + ": " + nextSkill.strTimeReplay() + "s";
            }

            string[] array = mFont.tahoma_7_grey.splitFontArray(text, w - 10);


            // popUpDetailInit1(cp,text,x,y);
            paintPopupInfoSkill(g, text, x, y, w, h);
        }

        public void popUpDetailInit1(ChatPopup cp, string chat, int x, int y)
        {
            cp.sayWidth = 180;
            cp.cx = x;
            cp.says = mFont.tahoma_7.splitFontArray(chat, cp.sayWidth - 8);
            cp.delay = 10000000;
            cp.c = null;
            cp.ch = cp.says.Length * 12;
            cp.cy = y;
            cp.strY = 10;
            cp.lim = cp.ch - 10;
            if (cp.lim < 0)
            {
                cp.lim = 0;
            }
            // ChatPopup.currChatPopup = cp;
        }
        public void popUpDetailInit(ChatPopup cp, string chat, int x, int y)
        {
            cp.isClip = false;
            cp.sayWidth = 180;
            cp.cx = x;
            cp.says = mFont.tahoma_7_red.splitFontArray(chat, cp.sayWidth - 10);
            cp.delay = 10000000;
            cp.c = null;
            cp.sayRun = 7;
            cp.ch = 15 - cp.sayRun + cp.says.Length * 12 + 10;
            if (cp.ch > GameCanvas.h - 80)
            {
                cp.ch = GameCanvas.h - 80;
                cp.lim = cp.says.Length * 12 - cp.ch + 17;
                if (cp.lim < 0)
                {
                    cp.lim = 0;
                }
                ChatPopup.cmyText = 0;
                cp.isClip = true;
            }
            cp.cy = y;
            while (cp.cy < 10)
            {
                cp.cy++;
                GameCanvas.menu.menuY++;
            }
            cp.mH = 0;
            cp.strY = 10;
            ChatPopup.currChatPopup = cp;
        }
        private static Image loadIMG(string path)
        {
            return Image.createImage((Resources.Load("res/x4" + path) as TextAsset).bytes);
        }
        public string[] strStatus = new string[6]
    {
        mResources.follow,
        mResources.defend,
        mResources.attack,
        mResources.gohome,
        mResources.fusion,
        mResources.fusionForever
    };
    }


}