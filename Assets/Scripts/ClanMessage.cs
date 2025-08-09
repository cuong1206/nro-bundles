using System;
using C_U_O_N_G;

public class ClanMessage : IActionListener
{
    public int id;

    public int type;

    public int playerId;

    public string playerName;

    public long time;

    public static TabSkill[] button;

    public int headId;

    public string[] chat;

    public sbyte color;

    public sbyte role;

    public int timeAgo;

    public int recieve;

    public int maxCap;

    public string[] option;

    public static MyVector vMessage = new MyVector();

    public static void addMessage(ClanMessage cm, int index, bool upToTop)
    {
        for (int i = 0; i < vMessage.size(); i++)
        {
            ClanMessage clanMessage = (ClanMessage)vMessage.elementAt(i);
            if (clanMessage.id == cm.id)
            {
                vMessage.removeElement(clanMessage);
                if (!upToTop)
                {
                    vMessage.insertElementAt(cm, i);
                }
                else
                {
                    vMessage.insertElementAt(cm, 0);
                }
                return;
            }
            if (clanMessage.maxCap != 0 && clanMessage.recieve == clanMessage.maxCap)
            {
                vMessage.removeElement(clanMessage);
            }
        }
        if (index == -1)
        {
            vMessage.addElement(cm);
        }
        else
        {
            vMessage.insertElementAt(cm, 0);
        }
        if (vMessage.size() > 20)
        {
            vMessage.removeElementAt(vMessage.size() - 1);
        }

    }

    public void paint(mGraphics g, int x, int y)
    {

        mFont mFont2 = mFont.tahoma_7b_dark;
        if (role == 0)
        {
            mFont2 = mFont.tahoma_7b_red;
        }
        else if (role == 1)
        {
            mFont2 = mFont.tahoma_7b_green;
        }
        else if (role == 2)
        {
            mFont2 = mFont.tahoma_7b_green2;
        }
        if (type == 0)
        {
            mFont2.drawString(g, playerName, x + 3, y + 1, 0);
            if (color == 0)
            {
                mFont.tahoma_7_grey.drawString(g, chat[0] + ((chat.Length <= 1) ? string.Empty : "..."), x + 3, y + 11, 0);
            }
            else
            {
                mFont.tahoma_7_red.drawString(g, chat[0] + ((chat.Length <= 1) ? string.Empty : "..."), x + 3, y + 11, 0);
            }
            mFont.tahoma_7_grey.drawString(g, NinjaUtil.getTimeAgo(timeAgo) + " " + mResources.ago, x + GameCanvas.panel.wScroll - 3, y + 1, mFont.RIGHT);
        }
        if (type == 1)
        {

            mFont2.drawString(g, playerName + " (" + recieve + "/" + maxCap + ")", x + 3, y + 1, 0);
            mFont.tahoma_7_blue.drawString(g, mResources.request_pea + " " + NinjaUtil.getTimeAgo(timeAgo) + " " + mResources.ago, x + 3, y + 11, 0);
        }
        if (type == 2)
        {

            mFont2.drawString(g, playerName, x + 3, y + 1, 0);
            mFont.tahoma_7_blue.drawString(g, mResources.request_join_clan, x + 3, y + 11, 0);
        }
    }

    public void paint2(mGraphics g, int x, int y)
    {

        mFont mFontText = mFont.tahoma_7_greySmall;
        if (type == 1)
        {
            mFontText = mFont.tahoma_7b_green2;
        }
        else if (type == 2)
        {
            mFontText = mFont.tahoma_7_blue;
        }
        mFont mFont2 = mFont.tahoma_7b_dark;
        if (role == 0)
        {
            mFont2 = mFont.tahoma_7b_red;
        }
        else if (role == 1)
        {
            mFont2 = mFont.tahoma_7b_green;
        }
        else if (role == 2)
        {
            mFont2 = mFont.tahoma_7b_green2;
        }
        ClanMessage cm = this;
        string chatfull = "";
        if (type == 1)
        {
            chatfull = "Xin đậu (" + recieve + "/" + maxCap + ")";

        }
        else if (type == 2)
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
        int wName = mFont.tahoma_7b_green.getWidth(playerName + ": ");
        int wText = mFont.tahoma_7b_green.getWidth(chatfull);

        PaintHeadMem(g, playerId, x, y + 10);
        string[] array = mFont.tahoma_7_greySmall.splitFontArray(chatfull, 200);
        int hPop = array.Length == 1 ? (20 * array.Length) : (15 * array.Length);
        if (type == 1 && recieve < 5)
        {
            hPop = 40;

        }
        PopUp.paintPopUp(g, x + 25, y + 7, (array.Length == 1 ? (wText + wName + 10) : (210 + wName)), hPop, 16777215, false);

        mFont2.drawString(g, playerName + ": ", x + 30, y + 11, 0);
        if (color == 0)
        {

            for (int j = 0; j < array.Length; j++)
            {
                mFontText.drawString(g, array[j], x + 30 + wName, y + 11 + (j * 10), 0);
            }
            mFont.tahoma_7_greySmall.drawString(g, NinjaUtil.getTimeAgo(timeAgo) + " " + mResources.ago, x + (array.Length == 1 ? (wText + wName + 50) : (240 + wName)), y + 15, mFont.LEFT);
        }
        else
        {
            for (int j = 0; j < array.Length; j++)
            {
                mFontText.drawString(g, array[j], x + 30 + wName, y + 11 + (j * 10), 0);
            }
        }
        if (type == 1 && recieve < 5)
        {
            int num10 = x + wText + wName + 15;

            g.drawImage(GameScr.imgLbtn2, num10, y + 35, StaticObj.VCENTER_HCENTER);
            mFont.tahoma_7b_dark.drawString(g, "Cho", num10, y + 30, mFont.CENTER);

            if (GameCanvas.isPointerHoldIn(num10 - GameScr.imgLbtn2.getWidth() / 2, y + 35 - GameScr.imgLbtn2.getHeight() / 2, GameScr.imgLbtn2.getWidth(), GameScr.imgLbtn2.getHeight()))
            {
                if (GameCanvas.isPointerJustRelease && GameCanvas.isPointerClick)
                {
                    Service.gI().clanDonate(id);
                }
            }

        }

        /*if (type == 1)
        {
            Cout.println("type mess 1");
            mFont2.drawString(g, playerName + " (" + recieve + "/" + maxCap + ")", x + 3, y + 1, 0);
            mFont.tahoma_7_blue.drawString(g, mResources.request_pea + " " + NinjaUtil.getTimeAgo(timeAgo) + " " + mResources.ago, x + 3, y + 11, 0);
        }*/
        /* if (type == 2)
         {
             Cout.println("type mess 2");
             mFont2.drawString(g, playerName, x + 3, y + 1, 0);
             mFont.tahoma_7_blue.drawString(g, mResources.request_join_clan, x + 3, y + 11, 0);
         }*/
    }
    public static string ConvertPlayerName(string playerName)
    {
        if (playerName.Length > 6)
        {
            return playerName.Substring(0, 6) + "...";
        }
        return playerName;
    }
    public static void PaintHeadMem(mGraphics g, int playerid, int x, int y)
    {
        short headid = -1;
        for (int j = 0; j < GameCanvas.panel.myMember.size(); j++)
        {
            Member member = (Member)GameCanvas.panel.myMember.elementAt(j);
            if (member.ID == playerid)
            {
                headid = member.head;
            }
        }
        if (headid != -1)
        {
            Part part = GameScr.parts[headid];
            SmallImage.drawSmallImage(g, part.pi[Char.CharInfo[0][0][0]].id, x + part.pi[Char.CharInfo[0][0][0]].dx, y + 3 + part.pi[Char.CharInfo[0][0][0]].dy, 0, 0);
        }
    }
    public void perform(int idAction, object p)
    {
        switch (idAction)
        {
            case 10:
                break;
        }
    }

    public void update()
    {

        if (time != 0)
        {
            timeAgo = (int)(mSystem.currentTimeMillis() / 1000 - time);
        }
    }
}
