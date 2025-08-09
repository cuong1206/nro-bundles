

using Assets.src.e;
using System;
using System.Drawing;
using UnityEngine;
using UnityEngine.TextCore.Text;
using UnityEngine.UIElements;
using C_U_O_N_G;
using Assets.src.g;
namespace C_U_O_N_G
{
    public class ZoneUI
    {
        public string caption;
        public IActionListener listener;
        public int idAction;
        public Image[] tabs = new Image[3];
         public Image imageZoneUI;
         private static ZoneUI instance;
        public int x, y;
        public bool isFocus;
        public bool isShow;
        public static ZoneUI gI()
        {
            if (instance == null)
            {
                instance = new ZoneUI();
            }
            return instance;
        }
       
        public void update()
        {
            if (!isShow) return;
                if (imageZoneUI == null)
                {
                    imageZoneUI = GameCanvas.loadImage("/bag/zoneUI.png");
                }
        }
        public void paint(mGraphics g)
        {
            if (!isShow) return;
            g.drawImage(imageZoneUI, GameCanvas.w/2, GameCanvas.h/2,mGraphics.VCENTER | mGraphics.HCENTER);
        }
        
    }
}