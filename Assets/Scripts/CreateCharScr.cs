using System;
using C_U_O_N_G;
using UnityEngine;

public class CreateCharScr : mScreen, IActionListener
{
    public static CreateCharScr instance;

    private PopUp p;

    public static bool isCreateChar = false;

    public static TField tAddName;

    public static int indexGender;

    public static int indexHair;

    public static int selected;

    public static int fkinh;

    public static int[][] hairID = new int[3][]
    {
        new int[3] { 64, 30, 31 },
        new int[3] { 9, 29, 32 },
        new int[3] { 6, 27, 28 }
    };

    public static int[] defaultLeg = new int[3] { 2, 13, 8 };

    public static int[] defaultBody = new int[3] { 1, 12, 7 };

    private int yButton;

    private int disY;

    private int[] bgID = new int[3] { 0, 4, 8 };

    public int yBegin;

    private int curIndex;

    private static int cx = (GameCanvas.w / 2) - 60;

    private static int cy = (GameCanvas.h / 2) + 25;

    private static int dy = 45;

    private int xr = 0;

    private int cp1;

    private int cf;

    private static Image[] bgnewplayer = new Image[3];

    private static Image[] kinhdosm0 = new Image[27];

    private static Image[] kinhdosm1 = new Image[27];

    private static Image[] kinhdosm2 = new Image[27];

    public static Image[] bttGender = new Image[3];

    public static Image[] bttFGender = new Image[3];

    public static Image[] angten = new Image[22];

    public static Image[] candauvan = new Image[9];

    public static Image[] beplua = new Image[9];

    public static Image switch_l;

    public static Image switch_r;

    public static Image switchf_l;

    public static Image switchf_r;

    public static Image cloudbg;

    private int zoomout;

    public CreateCharScr()
    {
        loadBg();
        try
        {
            if (!GameCanvas.lowGraphic)
            {
                loadMapFromResource(new sbyte[3] { 39, 40, 41 });
            }
            loadMapTableFromResource(new sbyte[3] { 39, 40, 41 });
        }
        catch (Exception ex)
        {
            Cout.LogError("Tao char loi " + ex.ToString());
        }
        if (GameCanvas.w <= 200)
        {
            GameScr.setPopupSize(128, 100);
            GameScr.popupX = (GameCanvas.w - 128) / 2;
            GameScr.popupY = 10;
            cy += 15;
            dy -= 15;
        }
        indexGender = 1;
        tAddName = new TField();
        tAddName.width = GameCanvas.loginScr.tfUser.width;
        if (GameCanvas.w < 200)
        {
            tAddName.width = 60;
        }
        tAddName.height = mScreen.ITEM_HEIGHT + 2;
        if (GameCanvas.w < 200)
        {
            tAddName.x = GameScr.popupX + 45;
            tAddName.y = GameScr.popupY + 12;
        }
        else
        {
            tAddName.x = GameCanvas.w / 2 - tAddName.width / 2;
            tAddName.y = 35;
        }
        if (!GameCanvas.isTouch)
        {
            tAddName.isFocus = true;
        }
        tAddName.setIputType(TField.INPUT_TYPE_ANY);
        tAddName.showSubTextField = false;
        tAddName.strInfo = mResources.char_name;
        if (tAddName.getText().Equals("@"))
        {
            tAddName.setText(GameCanvas.loginScr.tfUser.getText().Substring(0, GameCanvas.loginScr.tfUser.getText().IndexOf("@")));
        }
        tAddName.name = mResources.char_name;
        indexGender = 1;
        indexHair = 0;
        center = new Command(mResources.NEWCHAR, this, 8000, null);
        left = new Command(mResources.BACK, this, 8001, null);
        if (!GameCanvas.isTouch)
        {
            right = tAddName.cmdClear;
        }
        yBegin = tAddName.y;
    }

    public static CreateCharScr gI()
    {
        if (instance == null)
        {
            instance = new CreateCharScr();
        }
        return instance;
    }

    private static string namegender(int indexGen)
    {
        if (indexGen == 0)
        {
            return "trai dat ";
        }
        else if (indexGen == 1)
        {
            return "namek ";
        }
        else
        {
            return "Saiyan ";
        }
    }
    private static mFont fonthair(int indexGen)
    {
        if (indexGen == 0)
        {
            return mFont.tahoma_7b_blue;
        }
        else if (indexGen == 1)
        {
            return mFont.tahoma_7b_green;
        }
        else
        {
            return mFont.tahoma_7b_yellow;
        }
    }
    public static int getx()
    {
        int w = bgnewplayer[indexGender].getWidth();
        int wg = GameCanvas.w - (bgnewplayer[indexGender].getWidth());
        if (wg > 0)
        {
            return (((GameCanvas.w / 2) - 60) - (wg / mGraphics.zoomLevel));
        }
        else
        {
            return (GameCanvas.w / 2) - 60;
        }

    }
    public static int gety(int y)
    {
        int h = bgnewplayer[indexGender].getHeight();

        return (GameCanvas.h / 2);
    }
    public static void paintKinhsm(mGraphics g)
    {
        int idf = Math.min(fkinh / 5, 27);

        if (indexGender == 0)
        {
            if (idf >= kinhdosm0.Length)
            {
                idf = kinhdosm0.Length - 1;
            }
            if (kinhdosm0[idf] != null)
            {
                int imgW = (kinhdosm0[idf].getWidth());
                int imgH = (kinhdosm0[idf].getHeight());
                g.drawImageScale(kinhdosm0[idf], (GameCanvas.w / 2) + tAddName.width / 2, 30, imgW / 2 * 3, imgH / 2 * 3);
            }
        }
        else if (indexGender == 1)
        {
            if (idf >= kinhdosm1.Length)
            {
                idf = kinhdosm1.Length - 1;
            }
            if (kinhdosm1[idf] != null)
            {
                int imgW = (kinhdosm1[idf].getWidth());
                int imgH = (kinhdosm1[idf].getHeight());
                g.drawImageScale(kinhdosm1[idf], (GameCanvas.w / 2) + tAddName.width / 2, 30, imgW / 2 * 3, imgH / 2 * 3);
            }
        }
        else if (indexGender == 2)
        {
            if (idf >= kinhdosm2.Length)
            {
                idf = kinhdosm2.Length - 1;
            }
            if (kinhdosm2[idf] != null)
            {
                int imgW = (kinhdosm2[idf].getWidth());
                int imgH = (kinhdosm2[idf].getHeight());
                g.drawImageScale(kinhdosm2[idf], (GameCanvas.w / 2) + tAddName.width / 2, 30, imgW / 2 * 3, imgH / 2 * 3);
            }
        }
    }
    public static void painteffbg(mGraphics g)
    {
        int idfr = GameCanvas.gameTick / 5 % 22;
        int idfr1 = GameCanvas.gameTick / 5 % 9;
        if (indexGender == 0)
        {
            if (candauvan[idfr1] != null)
            {
                int imgW = (candauvan[idfr1].getWidth());
                int imgH = (candauvan[idfr1].getHeight());
                g.drawImage(candauvan[idfr1], cx - 10, cy, mGraphics.TOP | mGraphics.RIGHT);
            }
        }
        else if (indexGender == 1)
        {
            if (beplua[idfr1] != null)
            {
                int imgW = (beplua[idfr1].getWidth());
                int imgH = (beplua[idfr1].getHeight());
                g.drawImageScale(beplua[idfr1], cx + 5, cy + 25, imgW, imgH);
            }
        }
        else if (indexGender == 2)
        {
            if (angten[idfr] != null)
            {
                int imgW = (angten[idfr].getWidth());
                int imgH = (angten[idfr].getHeight());
                g.drawImage(angten[idfr], cx - 25, cy + 25, mGraphics.BOTTOM | mGraphics.RIGHT);
            }
        }
        /*else if (indexGender == 2)
        {
            if (idfr >= kinhdosm2.Length)
            {
                idfr = kinhdosm2.Length - 1;
            }
            if (kinhdosm2[idfr] != null)
            {
                int imgW = (kinhdosm2[idfr].getWidth());
                int imgH = (kinhdosm2[idfr].getHeight());
                g.drawImageScale(kinhdosm2[idfr], (GameCanvas.w / 2) + tAddName.width / 2, 30, imgW / 2 * 3, imgH / 2 * 3);
            }
        }*/
    }
    public static void paintbutton(mGraphics g)
    {
        int imgH = (bttFGender[0].getHeight() * 2);
        for (int i = 0; i < bttFGender.Length; i++)
        {
            if (i == indexGender)
            {
                g.drawImage(bttGender[i], 0, 25 + (i * imgH), 0);
            }
            else
            {
                g.drawImage(bttFGender[i], 5, 30 + (i * imgH), 0);
            }
        }
        g.drawImage(switch_l, 60, 150);
        g.drawImage(switch_r, GameCanvas.w / 2, 150);
        updateswitch(g);
    }
    public static void init()
    {
    }
    public static void loadBg()
    {
        switch_l = GameCanvas.loadImage($"/cuong/buttNewPl/Switch button_l.png");
        switch_r = GameCanvas.loadImage($"/cuong/buttNewPl/Switch button_r.png");
        switchf_l = GameCanvas.loadImage($"/cuong/buttNewPl/Switchf button_l.png");
        switchf_r = GameCanvas.loadImage($"/cuong/buttNewPl/Switchf button_r.png");
        cloudbg = GameCanvas.loadImage($"/cuong/effbg/cloudbg.png");
        for (int i = 0; i < bgnewplayer.Length; i++)
        {
            if (bgnewplayer[i] == null)
            {
                bgnewplayer[i] = GameCanvas.loadImage($"/cuong/bgnew_{i}.png");
            }
        }
        for (int i = 0; i < kinhdosm0.Length; i++)
        {
            if (kinhdosm0[i] == null)
            {
                kinhdosm0[i] = GameCanvas.loadImage($"/cuong/kinhsucmanh/kinh do suc manh-" + namegender(0) + (i < 10 ? ("0" + i) : i) + ".png");
            }
        }
        for (int i = 0; i < kinhdosm1.Length; i++)
        {
            if (kinhdosm1[i] == null)
            {
                kinhdosm1[i] = GameCanvas.loadImage($"/cuong/kinhsucmanh/kinh do suc manh-" + namegender(1) + (i < 10 ? ("0" + i) : i) + ".png");
            }
        }
        for (int i = 0; i < kinhdosm2.Length; i++)
        {
            if (kinhdosm2[i] == null)
            {
                kinhdosm2[i] = GameCanvas.loadImage($"/cuong/kinhsucmanh/kinh do suc manh-" + namegender(2) + (i < 10 ? ("0" + i) : i) + ".png");
            }
        }
        for (int i = 0; i < bttGender.Length; i++)
        {
            if (bttGender[i] == null)
            {
                bttGender[i] = GameCanvas.loadImage($"/cuong/buttNewPl/hanhtinh_" + i + ".png");
            }
            if (bttFGender[i] == null)
            {
                bttFGender[i] = GameCanvas.loadImage($"/cuong/buttNewPl/hanhtinhf_" + i + ".png");
            }
        }
        for (int i = 0; i < candauvan.Length; i++)
        {
            if (candauvan[i] == null)
            {
                candauvan[i] = GameCanvas.loadImage($"/cuong/effbg/can dau van-animation " + i + ".png");
            }
            if (beplua[i] == null)
            {
                beplua[i] = GameCanvas.loadImage($"/cuong/effbg/cui lua-animation " + i + ".png");
            }
        }
        for (int i = 0; i < angten.Length; i++)
        {
            if (angten[i] == null)
            {
                angten[i] = GameCanvas.loadImage($"/cuong/effbg/angten-dung im " + i + ".png");
            }
        }
    }
    public static void loadMapFromResource(sbyte[] mapID)
    {
        Res.outz("newwwwwwwwww =============");
        DataInputStream dataInputStream = null;
        for (int i = 0; i < mapID.Length; i++)
        {
            dataInputStream = MyStream.readFile("/mymap/" + mapID[i]);
            MapTemplate.tmw[i] = (ushort)dataInputStream.read();
            MapTemplate.tmh[i] = (ushort)dataInputStream.read();
            Cout.println("Thong TIn : " + MapTemplate.tmw[i] + "::" + MapTemplate.tmh[i]);
            MapTemplate.maps[i] = new int[dataInputStream.available()];
            Cout.println("lent= " + MapTemplate.maps[i].Length);
            for (int j = 0; j < MapTemplate.tmw[i] * MapTemplate.tmh[i]; j++)
            {
                MapTemplate.maps[i][j] = dataInputStream.read();
            }
            MapTemplate.types[i] = new int[MapTemplate.maps[i].Length];
        }
    }

    public void loadMapTableFromResource(sbyte[] mapID)
    {
        if (GameCanvas.lowGraphic)
        {
            return;
        }
        DataInputStream dataInputStream = null;
        try
        {
            for (int i = 0; i < mapID.Length; i++)
            {
                dataInputStream = MyStream.readFile("/mymap/mapTable" + mapID[i]);
                Cout.println("mapTable : " + mapID[i]);
                short num = dataInputStream.readShort();
                MapTemplate.vCurrItem[i] = new MyVector();
                Res.outz("nItem= " + num);
                for (int j = 0; j < num; j++)
                {
                    short id = dataInputStream.readShort();
                    short num2 = dataInputStream.readShort();
                    short num3 = dataInputStream.readShort();
                    if (TileMap.getBIById(id) != null)
                    {
                        BgItem bIById = TileMap.getBIById(id);
                        BgItem bgItem = new BgItem();
                        bgItem.id = id;
                        bgItem.idImage = bIById.idImage;
                        bgItem.dx = bIById.dx;
                        bgItem.dy = bIById.dy;
                        bgItem.x = num2 * TileMap.size;
                        bgItem.y = num3 * TileMap.size;
                        bgItem.layer = bIById.layer;
                        MapTemplate.vCurrItem[i].addElement(bgItem);
                        if (!BgItem.imgNew.containsKey(bgItem.idImage + string.Empty))
                        {
                            try
                            {
                                Image image = GameCanvas.loadImage("/mapBackGround/" + bgItem.idImage + ".png");
                                if (image == null)
                                {
                                    BgItem.imgNew.put(bgItem.idImage + string.Empty, Image.createRGBImage(new int[1], 1, 1, true));
                                    Service.gI().getBgTemplate(bgItem.idImage);
                                }
                                else
                                {
                                    BgItem.imgNew.put(bgItem.idImage + string.Empty, image);
                                }
                            }
                            catch (Exception)
                            {
                                Image image2 = GameCanvas.loadImage("/mapBackGround/" + bgItem.idImage + ".png");
                                if (image2 == null)
                                {
                                    image2 = Image.createRGBImage(new int[1], 1, 1, true);
                                    Service.gI().getBgTemplate(bgItem.idImage);
                                }
                                BgItem.imgNew.put(bgItem.idImage + string.Empty, image2);
                            }
                            BgItem.vKeysLast.addElement(bgItem.idImage + string.Empty);
                        }
                        if (!BgItem.isExistKeyNews(bgItem.idImage + string.Empty))
                        {
                            BgItem.vKeysNew.addElement(bgItem.idImage + string.Empty);
                        }
                        bgItem.changeColor();
                    }
                    else
                    {
                        Res.outz("item null");
                    }
                }
            }
        }
        catch (Exception ex2)
        {
            Cout.println("LOI TAI loadMapTableFromResource" + ex2.ToString());
        }
    }

    public override void switchToMe()
    {
        LoginScr.isContinueToLogin = false;
        GameCanvas.menu.showMenu = false;
        GameCanvas.endDlg();
        base.switchToMe();
        indexGender = Res.random(0, 3);
        indexHair = Res.random(0, 3);
        doChangeMap();
        Char.isLoadingMap = false;
        tAddName.setFocusWithKb(true);
    }

    public void doChangeMap()
    {
        TileMap.maps = new int[MapTemplate.maps[indexGender].Length];
        for (int i = 0; i < MapTemplate.maps[indexGender].Length; i++)
        {
            TileMap.maps[i] = MapTemplate.maps[indexGender][i];
        }
        TileMap.types = MapTemplate.types[indexGender];
        TileMap.pxh = MapTemplate.pxh[indexGender];
        TileMap.pxw = MapTemplate.pxw[indexGender];
        TileMap.tileID = MapTemplate.pxw[indexGender];
        TileMap.tmw = MapTemplate.tmw[indexGender];
        TileMap.tmh = MapTemplate.tmh[indexGender];
        TileMap.tileID = bgID[indexGender] + 1;
        /*TileMap.loadMainTile();
        TileMap.loadTileCreatChar();*/
        // GameCanvas.loadBG(bgID[indexGender]);
        GameScr.loadCamera(fullmScreen: true, -1, -1);
    }

    public override void keyPress(int keyCode)
    {
        tAddName.keyPressed(keyCode);
    }

    public override void update()
    {
        cp1++;
        fkinh++;
        if (cp1 > 30)
        {
            cp1 = 0;
        }
        if (cp1 % 15 < 5)
        {
            cf = 0;
        }
        else
        {
            cf = 1;
        }
        tAddName.update();
        if (selected != 0)
        {
            tAddName.isFocus = false;
        }
    }

    public override void updateKey()
    {
        if (GameCanvas.keyPressed[(!Main.isPC) ? 2 : 21])
        {
            SoundMn.gI().buttonClick();
            indexGender--;
            if (indexGender < 0)
            {
                indexGender = mResources.MENUGENDER.Length - 1;
            }
            doChangeMap();
            fkinh = 0;
        }
        if (GameCanvas.keyPressed[(!Main.isPC) ? 8 : 22])
        {
            SoundMn.gI().buttonClick();
            indexGender++;
            if (indexGender > mResources.MENUGENDER.Length - 1)
            {
                indexGender = 0;
            }
            doChangeMap();
            fkinh = 0;
        }
        if (selected == 0)
        {
            if (!GameCanvas.isTouch)
            {
                right = tAddName.cmdClear;
            }
            tAddName.update();
        }

        if (GameCanvas.keyPressed[(!Main.isPC) ? 4 : 23])
        {
            SoundMn.gI().buttonClick();
            indexHair--;
            if (indexHair < 0)
            {
                indexHair = mResources.hairStyleName[0].Length - 1;
            }
        }
        if (GameCanvas.keyPressed[(!Main.isPC) ? 6 : 24])
        {
            SoundMn.gI().buttonClick();
            indexHair++;
            if (indexHair > mResources.hairStyleName[0].Length - 1)
            {
                indexHair = 0;
            }
        }
        right = null;
        if (GameCanvas.isPointerJustRelease)
        {
            int num = 110;
            int num2 = 60;
            int num3 = 78;
            int hbtt = bttFGender[0].getHeight();
            int wbtt = bttFGender[0].getWidth();
            if (GameCanvas.w > GameCanvas.h)
            {
                num = 100;
                num2 = 40;
            }
            if (GameCanvas.isPointerHoldIn(GameCanvas.w / 2 - 3 * num3 / 2, 15, num3 * 3, 80))
            {
                selected = 0;
                tAddName.isFocus = true;
            }

            if (GameCanvas.isPointerHoldIn(5, 30, wbtt, hbtt))
            {
                SoundMn.gI().buttonClick();
                selected = 1;
                int num4 = indexGender;
                indexGender = 0;
                if (indexGender < 0)
                {
                    indexGender = 0;
                }
                if (indexGender > mResources.MENUGENDER.Length - 1)
                {
                    indexGender = mResources.MENUGENDER.Length - 1;
                }
                if (num4 != indexGender)
                {
                    doChangeMap();
                    fkinh = 0;
                }
            }
            if (GameCanvas.isPointerHoldIn(5, 30 + (hbtt * 2), wbtt, hbtt))
            {
                SoundMn.gI().buttonClick();
                selected = 1;
                int num4 = indexGender;
                indexGender = 1;
                if (indexGender < 0)
                {
                    indexGender = 0;
                }
                if (indexGender > mResources.MENUGENDER.Length - 1)
                {
                    indexGender = mResources.MENUGENDER.Length - 1;
                }
                if (num4 != indexGender)
                {
                    doChangeMap();
                    fkinh = 0;
                }
            }
            if (GameCanvas.isPointerHoldIn(5, 30 + (2 * hbtt * 2), wbtt, hbtt))
            {
                SoundMn.gI().buttonClick();
                selected = 1;
                int num4 = indexGender;
                indexGender = 2;
                if (indexGender < 0)
                {
                    indexGender = 0;
                }
                if (indexGender > mResources.MENUGENDER.Length - 1)
                {
                    indexGender = mResources.MENUGENDER.Length - 1;
                }
                if (num4 != indexGender)
                {
                    doChangeMap();
                    fkinh = 0;
                }
            }
            /*if (GameCanvas.isPointerHoldIn(GameCanvas.w / 2 - 3 * num3 / 2, num - 30, num3 * 3, num2 + 5))
            {
                selected = 1;
                int num4 = indexGender;
                indexGender = (GameCanvas.px - (GameCanvas.w / 2 - 3 * num3 / 2)) / num3;
                if (indexGender < 0)
                {
                    indexGender = 0;
                }
                if (indexGender > mResources.MENUGENDER.Length - 1)
                {
                    indexGender = mResources.MENUGENDER.Length - 1;
                }
                if (num4 != indexGender)
                {
                    doChangeMap();
                    fkinh = 0;
                }
            }*/
            /* if (GameCanvas.isPointerHoldIn(GameCanvas.w / 2 - 3 * num3 / 2, num - 30 + num2 + 5, num3 * 3, 65))
             {
                 selected = 2;
                 int num5 = indexHair;
                 indexHair = (GameCanvas.px - (GameCanvas.w / 2 - 3 * num3 / 2)) / num3;
                 if (indexHair < 0)
                 {
                     indexHair = 0;
                 }
                 if (indexHair > mResources.hairStyleName[0].Length - 1)
                 {
                     indexHair = mResources.hairStyleName[0].Length - 1;
                 }
                 if (num5 != selected)
                 {
                     doChangeMap();

                 }
             }*/
            int wsw = switch_l.getWidth();
            int hsw = switch_l.getHeight();
            if (GameCanvas.isPointerHoldIn(60, 150, wsw, hsw))
            {
                SoundMn.gI().buttonClick();


                selected = 2;
                int num5 = indexHair;
                indexHair--;
                if (indexHair < 0)
                {
                    indexHair = mResources.hairStyleName[0].Length - 1;
                }
                if (num5 != selected)
                {
                    doChangeMap();

                }
            }
            if (GameCanvas.isPointerHoldIn(GameCanvas.w / 2, 150, wsw, hsw))
            {
                SoundMn.gI().buttonClick();
                selected = 2;
                int num5 = indexHair;
                indexHair++;
                if (indexHair > mResources.hairStyleName[0].Length - 1)
                {
                    indexHair = 0;
                }
                if (num5 != selected)
                {
                    doChangeMap();

                }
            }

        }
        if (!TouchScreenKeyboard.visible)
        {
            base.updateKey();
        }
        GameCanvas.clearKeyHold();
        GameCanvas.clearKeyPressed();
    }
    public static void updateswitch(mGraphics g)
    {
        int wsw = switch_l.getWidth();
        int hsw = switch_l.getHeight();
        if (GameCanvas.isPointerHoldIn(60, 150, wsw, hsw))
        {
            g.drawImage(switchf_l, 60, 150);
        }
        if (GameCanvas.isPointerHoldIn(GameCanvas.w / 2, 150, wsw, hsw))
        {
            g.drawImage(switchf_r, GameCanvas.w / 2, 150);
        }
    }
    public override void paint(mGraphics g)
    {
        if (Char.isLoadingMap)
        {
            return;
        }
        //GameCanvas.paintBGGameScr(g);
        g.drawImageScale(bgnewplayer[indexGender], 0, 0, GameCanvas.w, GameCanvas.h, 0);






        g.translate(-GameScr.cmx, -GameScr.cmy);
        if (!GameCanvas.lowGraphic)
        {
            for (int i = 0; i < MapTemplate.vCurrItem[indexGender].size(); i++)
            {
                BgItem bgItem = (BgItem)MapTemplate.vCurrItem[indexGender].elementAt(i);
                if (bgItem.idImage != -1 && bgItem.layer == 1)
                {
                    bgItem.paint(g);
                }
            }
        }
        /*updateswitch(g);*/
        // TileMap.paintTilemap(g);
        int num = 30;
        if (GameCanvas.w == 128)
        {
            num = 20;
        }
        int num2 = hairID[indexGender][indexHair];
        int num3 = defaultLeg[indexGender];
        int num4 = defaultBody[indexGender];
        g.drawImage(TileMap.bong, cx, cy + dy, 3);
        Part part = GameScr.parts[num2];
        Part part2 = GameScr.parts[num3];
        Part part3 = GameScr.parts[num4];
        mFont.tahoma_7b_white.drawStringBd(g, mResources.hairStyleName[indexGender][indexHair], cx - TileMap.bong.getWidth() / 2, cy - Char.CharInfo[cf][0][2] + part.pi[Char.CharInfo[cf][0][0]].dy + dy - 20, mFont.LEFT, fonthair(indexGender));
        SmallImage.drawSmallImage(g, part.pi[Char.CharInfo[cf][0][0]].id, cx + Char.CharInfo[cf][0][1] + part.pi[Char.CharInfo[cf][0][0]].dx, cy - Char.CharInfo[cf][0][2] + part.pi[Char.CharInfo[cf][0][0]].dy + dy, 0, 0);
        SmallImage.drawSmallImage(g, part2.pi[Char.CharInfo[cf][1][0]].id, cx + Char.CharInfo[cf][1][1] + part2.pi[Char.CharInfo[cf][1][0]].dx, cy - Char.CharInfo[cf][1][2] + part2.pi[Char.CharInfo[cf][1][0]].dy + dy, 0, 0);
        SmallImage.drawSmallImage(g, part3.pi[Char.CharInfo[cf][2][0]].id, cx + Char.CharInfo[cf][2][1] + part3.pi[Char.CharInfo[cf][2][0]].dx, cy - Char.CharInfo[cf][2][2] + part3.pi[Char.CharInfo[cf][2][0]].dy + dy, 0, 0);
        if (!GameCanvas.lowGraphic)
        {
            for (int j = 0; j < MapTemplate.vCurrItem[indexGender].size(); j++)
            {
                BgItem bgItem2 = (BgItem)MapTemplate.vCurrItem[indexGender].elementAt(j);
                if (bgItem2.idImage != -1 && bgItem2.layer == 3)
                {
                    bgItem2.paint(g);
                }
            }
        }
        painteffbg(g);
        g.translate(-g.getTranslateX(), -g.getTranslateY());
        int ycloud = 70;
        if (indexGender == 0 || indexGender == 2)
        {
            g.setClip(GameCanvas.w / 4 - 5, 0, GameCanvas.w, GameCanvas.h / 3);
        }
        if (indexGender == 0)
        {
            ycloud = 0;
        }

        int imgW = cloudbg.getWidth();
        int imgH = cloudbg.getHeight();
        int xcloud = (GameCanvas.gameTick / 5) % cloudbg.getWidth();
        g.drawImage(cloudbg, xcloud, ycloud, 0);
        g.drawImage(cloudbg, xcloud - cloudbg.getWidth(), ycloud, 0);
        if (GameCanvas.w < 200)
        {
            GameCanvas.paintz.paintFrame(GameScr.popupX, GameScr.popupY, GameScr.popupW, GameScr.popupH, g);
            SmallImage.drawSmallImage(g, part.pi[Char.CharInfo[0][0][0]].id, GameCanvas.w / 2 + Char.CharInfo[0][0][1] + part.pi[Char.CharInfo[0][0][0]].dx, GameScr.popupY + 30 + 3 * num - Char.CharInfo[0][0][2] + part.pi[Char.CharInfo[0][0][0]].dy + dy, 0, 0);
            SmallImage.drawSmallImage(g, part2.pi[Char.CharInfo[0][1][0]].id, GameCanvas.w / 2 + Char.CharInfo[0][1][1] + part2.pi[Char.CharInfo[0][1][0]].dx, GameScr.popupY + 30 + 3 * num - Char.CharInfo[0][1][2] + part2.pi[Char.CharInfo[0][1][0]].dy + dy, 0, 0);
            SmallImage.drawSmallImage(g, part3.pi[Char.CharInfo[0][2][0]].id, GameCanvas.w / 2 + Char.CharInfo[0][2][1] + part3.pi[Char.CharInfo[0][2][0]].dx, GameScr.popupY + 30 + 3 * num - Char.CharInfo[0][2][2] + part3.pi[Char.CharInfo[0][2][0]].dy + dy, 0, 0);
            for (int k = 0; k < mResources.MENUNEWCHAR.Length; k++)
            {
                if (selected == k)
                {
                    g.drawRegion(GameScr.arrow, 0, 0, 13, 16, 2, GameScr.popupX + 10 + ((GameCanvas.gameTick % 7 > 3) ? 1 : 0), GameScr.popupY + 35 + k * num, StaticObj.VCENTER_HCENTER);
                    g.drawRegion(GameScr.arrow, 0, 0, 13, 16, 0, GameScr.popupX + GameScr.popupW - 10 - ((GameCanvas.gameTick % 7 > 3) ? 1 : 0), GameScr.popupY + 35 + k * num, StaticObj.VCENTER_HCENTER);
                }
                mFont.tahoma_7b_dark.drawString(g, mResources.MENUNEWCHAR[k], GameScr.popupX + 20, GameScr.popupY + 30 + k * num, 0);
            }
            mFont.tahoma_7b_dark.drawString(g, mResources.MENUGENDER[indexGender], GameScr.popupX + 70, GameScr.popupY + 30 + num, mFont.LEFT);
            mFont.tahoma_7b_dark.drawString(g, mResources.hairStyleName[indexGender][indexHair], GameScr.popupX + 55, GameScr.popupY + 30 + 2 * num, mFont.LEFT);
            tAddName.paint(g);
        }
        else
        {
            if (!Main.isPC)
            {
                if (mGraphics.addYWhenOpenKeyBoard != 0)
                {
                    yButton = 110;
                    disY = 60;
                    if (GameCanvas.w > GameCanvas.h)
                    {
                        yButton = GameScr.popupY + 30 + 3 * num + part3.pi[Char.CharInfo[0][2][0]].dy + dy - 15;
                        disY = 35;
                    }
                }
                else
                {
                    yButton = 110;
                    disY = 60;
                    if (GameCanvas.w > GameCanvas.h)
                    {
                        yButton = 100;
                        disY = 45;
                    }
                }
                tAddName.y = yButton - tAddName.height - disY + 5;
            }
            else
            {
                yButton = 110;
                disY = 60;
                if (GameCanvas.w > GameCanvas.h)
                {
                    yButton = 100;
                    disY = 45;
                }
                tAddName.y = yBegin;
            }
            /*for (int l = 0; l < 3; l++)
            {
                int num5 = 78;
                if (l != indexGender)
                {
                    g.drawImage(GameScr.imgLbtn, GameCanvas.w / 2 - num5 + l * num5, yButton, 3);
                }
                else
                {
                    if (selected == 1)
                    {
                        g.drawRegion(GameScr.arrow, 0, 0, 13, 16, 4, GameCanvas.w / 2 - num5 + l * num5, yButton - 20 + ((GameCanvas.gameTick % 7 > 3) ? 1 : 0), StaticObj.VCENTER_HCENTER);
                    }
                    g.drawImage(GameScr.imgLbtnFocus, GameCanvas.w / 2 - num5 + l * num5, yButton, 3);
                }
                mFont.tahoma_7b_dark.drawString(g, mResources.MENUGENDER[l], GameCanvas.w / 2 - num5 + l * num5, yButton - 5, mFont.CENTER);
            }
            for (int m = 0; m < 3; m++)
            {
                int num6 = 78;
                if (m != indexHair)
                {
                    g.drawImage(GameScr.imgLbtn, GameCanvas.w / 2 - num6 + m * num6, yButton + disY, 3);
                }
                else
                {
                    if (selected == 2)
                    {
                        g.drawRegion(GameScr.arrow, 0, 0, 13, 16, 4, GameCanvas.w / 2 - num6 + m * num6, yButton + disY - 20 + ((GameCanvas.gameTick % 7 > 3) ? 1 : 0), StaticObj.VCENTER_HCENTER);
                    }
                    g.drawImage(GameScr.imgLbtnFocus, GameCanvas.w / 2 - num6 + m * num6, yButton + disY, 3);
                }
                mFont.tahoma_7b_dark.drawString(g, mResources.hairStyleName[indexGender][m], GameCanvas.w / 2 - num6 + m * num6, yButton + disY - 5, mFont.CENTER);
            }*/
            tAddName.paint(g);
        }

        g.setClip(0, 0, GameCanvas.w, GameCanvas.h);
        mFont.tahoma_7b_white.drawString(g, mResources.server + " " + LoginScr.serverName, 5, 5, 0, mFont.tahoma_7b_dark);
        if (!TouchScreenKeyboard.visible)
        {
            base.paint(g);
        }
        paintKinhsm(g);
        paintbutton(g);

    }

    public void perform(int idAction, object p)
    {
        switch (idAction)
        {
            case 8000:
                if (tAddName.getText().Equals(string.Empty))
                {
                    GameCanvas.startOKDlg(mResources.char_name_blank);
                    break;
                }
                if (tAddName.getText().Length < 5)
                {
                    GameCanvas.startOKDlg(mResources.char_name_short);
                    break;
                }
                if (tAddName.getText().Length > 15)
                {
                    GameCanvas.startOKDlg(mResources.char_name_long);
                    break;
                }
                InfoDlg.showWait();
                Service.gI().createChar(tAddName.getText(), indexGender, hairID[indexGender][indexHair]);
                break;
            case 8001:
                if (GameCanvas.loginScr.isLogin2)
                {
                    GameCanvas.startYesNoDlg(mResources.note, new Command(mResources.YES, this, 10019, null), new Command(mResources.NO, this, 10020, null));
                    break;
                }
                if (Main.isWindowsPhone)
                {
                    GameMidlet.isBackWindowsPhone = true;
                }
                Session_ME.gI().close();
                GameCanvas.serverScreen.switchToMe();
                break;
            case 10020:
                GameCanvas.endDlg();
                break;
            case 10019:
                Session_ME.gI().close();
                GameCanvas.endDlg();
                GameCanvas.serverScreen.switchToMe();
                break;
        }
    }
}
