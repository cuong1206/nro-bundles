namespace C_U_O_N_G
{
    public class Tab
    {
        public string caption;
        public IActionListener listener;
        public int idAction;
        public object p;
        public Image[] tabs = new Image[3];
        public int x, y, w, h;
        public bool isFocus;
        public Tab(string caption, IActionListener listener, int idAction, object p)
        {
            this.caption = caption;
            this.listener = listener;
            this.idAction = idAction;
            this.p = p;
            this.w = 20;
            this.h = 10;
            for (int i = 0; i < tabs.Length; i++)
            {   
                if (tabs[i] == null)
                {
                    tabs[i] = GameCanvas.loadImage($"/bag/cmd{i}.png");
                }
            }
        }
        public void paint(mGraphics g)
        {
            if (isFocus)
            {
                /* g.drawImage(tabs[1], x, y);*/
                g.setColor(0xa2471a);
                g.fillRect(x, y, tabs[0].getWidth(), tabs[0].getHeight(), 4);
                g.setColor(0xffe749);
                g.fillRect(x + 2, y + 2, tabs[0].getWidth() - 4, tabs[0].getHeight() - 4, 10);
            }
            else if (GameCanvas.isMouseFocus(x, y, tabs[0].getWidth(), tabs[0].getHeight()))
            {
                /*g.drawImage(tabs[2], x, y);*/
                g.setColor(0xa2471a);
                g.fillRect(x, y, tabs[0].getWidth(), tabs[0].getHeight(), 4);
                g.setColor(0xffe749);
                g.fillRect(x + 2, y + 2, tabs[0].getWidth() - 4, tabs[0].getHeight() - 4,4);
            }
            else
            {
                /* g.drawImage(tabs[0], x, y);*/
                g.setColor(0xa2471a);
                g.fillRect(x, y, tabs[0].getWidth(), tabs[0].getHeight(), 4);
                g.setColor(0xf5b971);
                g.fillRect(x + 2, y + 2, tabs[0].getWidth() - 4, tabs[0].getHeight() - 4, 10);
            }
            if (caption != "")
            {
                mFont.tahoma_7_white.drawStringBd(g, caption, x + tabs[0].getWidth() / 2, y + 3, mGraphics.VCENTER | mGraphics.HCENTER, mFont.tahoma_7b_dark);
            }
        }
        public void actionPerform()
        {
            if (listener != null)
            {
                listener.perform(idAction, p);
            }
            else
            {
                GameScr.gI().actionPerform(idAction, p);
            }
        }
        public bool Pressed()
        {
            isFocus = false;
            if (GameCanvas.isPointerHoldIn(x, y, tabs[0].getWidth(), tabs[0].getHeight()))
            {
                if (GameCanvas.isPointerDown)
                {
                    isFocus = true;
                }
                if (GameCanvas.isPointerJustRelease && GameCanvas.isPointerClick)
                {
                    return true;
                }
            }
            return false;
        }
    }
}